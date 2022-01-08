using Krypton.Toolkit;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SocialScraper
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            KryptonMessageBox.Show(str, "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            KryptonMessageBox.Show(str, "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************Exception****************************");
            sb.AppendLine("【Date】：" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            if (ex != null)
            {
                sb.AppendLine("【Type】：" + ex.GetType().Name);
                sb.AppendLine("【Message】：" + ex.Message);
                sb.AppendLine("【StackTrace】：" + ex.StackTrace);
                sb.AppendLine("【TargetSite】：" + ex.TargetSite);
            }
            else
            {
                sb.AppendLine("【Untreated Exception】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}
