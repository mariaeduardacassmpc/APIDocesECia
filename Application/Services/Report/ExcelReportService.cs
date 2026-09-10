using ClosedXML.Excel;

namespace ApiDoces.Services.Report;

public record ReportColumn<T>(string Header, Func<T, object?> ValueSelector, string? Format = null);

public class ExcelReportService
{
    public byte[] Generate<T>(string sheetName, IEnumerable<T> data, params ReportColumn<T>[] columns)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        for (int col = 0; col < columns.Length; col++)
            worksheet.Cell(1, col + 1).Value = columns[col].Header;

        var rows = data.ToList();
        for (int row = 0; row < rows.Count; row++)
        {
            for (int col = 0; col < columns.Length; col++)
            {
                var value = columns[col].ValueSelector(rows[row]);
                worksheet.Cell(row + 2, col + 1).Value = XLCellValue.FromObject(value);
            }
        }

        for (int col = 0; col < columns.Length; col++)
        {
            if (columns[col].Format is not null)
                worksheet.Column(col + 1).Style.NumberFormat.Format = columns[col].Format;
        }

        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}