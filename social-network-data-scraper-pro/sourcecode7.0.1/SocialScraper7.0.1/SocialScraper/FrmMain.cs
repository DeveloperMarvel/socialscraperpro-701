using Krypton.Toolkit;
using ScraperCore;
using ScraperCore.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocialScraper
{
    public partial class FrmMain : KryptonForm
    {
        private string _url = "https://www.google.com/search?num=20";
        private SocialInfoScraper _scraper = new SocialInfoScraper();
        private List<SocialModel> _rList = new List<SocialModel>();
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public FrmMain()
        {
            InitializeComponent();
            this.BindComBox();
        }

        private void BindComBox()
        {
            var list = new List<SocialItem>
            {
                new SocialItem{  Name="LinkedIn", Site="site:LinkedIn.com"},
                new SocialItem{  Name="Facebook", Site="site:facebook.com"},
                new SocialItem{  Name="Instagram", Site="site:instagram.com"},
                new SocialItem{  Name="Youtube", Site="site:youtube.com"},
                new SocialItem{  Name="Pinterest", Site="site:pinterest.com"},
                 new SocialItem{  Name="Twitter", Site="site:twitter.com"},

            };
            this.comBoxType.DataSource = list;
            this.comBoxType.DisplayMember = "Name";
            this.comBoxType.ValueMember = "Site";
            this.comBoxType.SelectedIndex = 0;

        }
        private void AddRow(List<SocialModel> list)
        {
            this._rList.AddRange(list);
            var avatar = this.imageList1.Images[0];
            foreach (var item in list)
            {
                this.Invoke(new Action(() =>
                {
                    this.kryptonDataGridView1.Rows.Insert(0, new object[]
                    {
                       avatar,
                       item.Name,
                       item.Tel,
                       item.Email,
                       item.Position,
                       item.Company,
                       item.Address,
                       item.HomePage,
                       item.Description
                    });
                    this.labTotal.Text = this.kryptonDataGridView1.RowCount.ToString();
                }));
            }

        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            var maxNum = (int)this.numMax.Value;
            var keyword = this.txtKeywrod.Text.Trim();
            var delay = (int)this.nupDelay.Value;
            var social = this.comBoxType.SelectedValue;
            var isEmail = this.cbEmail.Checked;
            var searchKey = isEmail ? $"{keyword} \"email\"  and {social}" : $"{keyword}  and {social}";
            if (string.IsNullOrEmpty(keyword))
            {
                KryptonMessageBox.Show("Please enter key words！", "Info",
                    MessageBoxButtons.OK, KryptonMessageBoxIcon.WARNING, showCtrlCopy: false);
                return;
            }

            if (this.btnStart.Text == "Start")
            {
                this.btnStart.Text = "Stop";
                Action<List<SocialModel>> aciton = (list) => this.AddRow(list);
                var token = new CancellationToken();
                token = this._tokenSource.Token;
                Task.Factory.StartNew(() =>
                {
                    _scraper.Start<string>(this._url);
                    this._scraper.Search(searchKey);
                    if (!this._scraper.IsExitsVerifyCode())
                    {
                        this._scraper.ExtractData(maxNum, aciton, delay, token, isEmail);
                    }
                    else
                    {
                        KryptonMessageBox.Show("A verification code appears, please complete the verification first！", "Info",
                                 MessageBoxButtons.OK, KryptonMessageBoxIcon.ERROR, showCtrlCopy: false);
                        return;
                    }


                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                    .ContinueWith(t =>
                   {
                       if (t.IsCompleted)
                       {
                           this.Invoke(new Action(() =>
                           {
                               this.btnStart.Text = "Start";
                               this.btnExport.Enabled = true;
                               KryptonMessageBox.Show("Extraction Finish！", "Info",
                                   MessageBoxButtons.OK, KryptonMessageBoxIcon.INFORMATION, showCtrlCopy: false);
                           }));
                       }
                   });
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    this.btnStart.Text = "Stopping";
                }));
                this._tokenSource.Cancel();
                this._tokenSource = new CancellationTokenSource();

            }



        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (KryptonMessageBox.Show("Are you sure exit?", "Info",
                                MessageBoxButtons.YesNo, KryptonMessageBoxIcon.QUESTION, showCtrlCopy: false)
                         == DialogResult.Yes)
            {
                this._scraper.Quit();
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this._rList.Count == 0)
            {
                KryptonMessageBox.Show("No Data！", "Info",
                                    MessageBoxButtons.OK, KryptonMessageBoxIcon.WARNING, showCtrlCopy: false);
                return;
            }
            var r = ExcelUtils.ExportToExcel(this._rList);
            var msg = r.Item1 ? "Export Success!" : r.Item2;
            KryptonMessageBox.Show(msg, "Info", MessageBoxButtons.OK, KryptonMessageBoxIcon.WARNING, showCtrlCopy: false);

        }
    }
}
