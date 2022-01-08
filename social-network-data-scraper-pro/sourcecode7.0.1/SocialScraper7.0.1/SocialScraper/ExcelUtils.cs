using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ScraperCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SocialScraper
{
    public class ExcelUtils
    {
        public static void ExprotData(string path, List<SocialModel> list)
        {
            var spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            var workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };
            sheets.Append(sheet);
            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            var nameList = typeof(SocialModel).GetProperties().ToList();
            var rowName = new Row();
            foreach (var p in nameList)
            {
                var cell = new Cell
                {
                    CellValue = new CellValue(p.Name),
                    DataType = CellValues.String
                };
                rowName.AppendChild(cell);
            }
            sheetData.Append(rowName);
            foreach (var item in list)
            {
                var row = new Row();
                var pList = item.GetType().GetProperties().ToList();
                foreach (var p in pList)
                {
                    var cell = new Cell
                    {
                        CellValue = new CellValue(p.GetValue(item)?.ToString()),
                        DataType = CellValues.String
                    };
                    row.AppendChild(cell);
                }
                sheetData.Append(row);
            }
            workbookpart.Workbook.Save();
            spreadsheetDocument.Close();
        }


        public static Tuple<bool, string> ExportToExcel(List<SocialModel> list)
        {
            try
            {
                var saveFile = new SaveFileDialog
                {
                    FileName = $"{Environment.TickCount}.xlsx",
                    Filter = "CSV File|*.xlsx"
                };
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    var path = saveFile.FileName;
                    ExprotData(path, list);
                }
                return System.Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {

                return System.Tuple.Create(true, ex.Message);
            }

        }
    }
}
