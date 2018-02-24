using OfficeOpenXml;
using System.Data;
using System.Linq;

namespace TranslationFileGen
{
    public static class ExcelPackageExtensions
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            bool ifAllEmpty = true;
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                ifAllEmpty = true;
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    if (!string.IsNullOrEmpty(cell.Text))
                        ifAllEmpty = false;
                    newRow[cell.Start.Column - 1] = cell.Text;
                }

                if (ifAllEmpty)
                    break;

                table.Rows.Add(newRow);
            }
            return table;
        }
    }
}