using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SocialScraper
{
    public class ExportUtils
    {
        private void ExportToCsv<T>(string path, List<T> list)
        {
            var resultList = new List<string>();
            var first = list.FirstOrDefault();
            var pList = first.GetType().GetProperties().ToList();
            var colName = pList.Select(p => p.Name).Aggregate((c, p) => c + "," + p);
            resultList.Add(colName);

            list.ForEach(item =>
            {
                var itemStr = pList.Select(p => p.GetValue(item) == null ? string.Empty
                             : p.GetValue(item).ToString())
                             .Aggregate((c, p) => c + "," + p);
                resultList.Add(itemStr);
            });

            File.WriteAllLines(path, resultList, Encoding.UTF8);

        }

        public Tuple<bool, string> ExportToCsv<T>(List<T> list)
        {
            try
            {
                var saveFile = new SaveFileDialog
                {
                    FileName = $"{Environment.TickCount}.csv",
                    Filter = "CSV File|*.csv"
                };
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    var path = saveFile.FileName;
                    this.ExportToCsv(path, list);
                }
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {

                return Tuple.Create(true, ex.Message);
            }

        }
    }
}
