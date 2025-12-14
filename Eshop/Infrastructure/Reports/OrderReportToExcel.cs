using Common.Application;
using Common.Application.DateUtil;
using DataLayer.Context;
using DomainLayer.Models.Orders;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayer.Services.Orders
{
    public class ReportModel
    {
        public string Format { get; set; } = "XLSX";
        public SaveOptions Options => FormatMappingDictionary[Format];
        public IDictionary<string, SaveOptions> FormatMappingDictionary => new Dictionary<string, SaveOptions>()
        {
            ["XLSX"] = new XlsxSaveOptions(),
            ["XLS"] = new XlsSaveOptions(),
            ["ODS"] = new OdsSaveOptions(),
            ["CSV"] = new CsvSaveOptions(CsvType.CommaDelimited),
            ["PDF"] = new PdfSaveOptions(),
            ["HTML"] = new HtmlSaveOptions() { EmbedImages = true },
            ["XPS"] = new XpsSaveOptions(),
            ["BMP"] = new ImageSaveOptions(ImageSaveFormat.Bmp),
            ["PNG"] = new ImageSaveOptions(ImageSaveFormat.Png),
            ["JPG"] = new ImageSaveOptions(ImageSaveFormat.Jpeg),
            ["GIF"] = new ImageSaveOptions(ImageSaveFormat.Gif),
            ["TIF"] = new ImageSaveOptions(ImageSaveFormat.Tiff)
        };
    }
    public class OrderReportToExcel : BaseService
    {

        public OrderReportToExcel(AppDbContext context) : base(context)
        {


        }

        public async Task<Stream> ExportReport(DateTime? startDate, DateTime? endDate)
        {
            var stream = new MemoryStream();
            var xlPackage = new ExcelPackage(stream);
            var sheet = xlPackage.Workbook.Worksheets.Add($"گزارشات");
            sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            var orders = Table<Order>().Include(c => c.Details).ThenInclude(c => c.Product).Where(r => r.IsFinally);
            if (startDate != null)
            {
                orders = orders.Where(r => r.PaymentDate.Date >= startDate.Value.Date);
            }
            if (endDate != null)
            {
                orders = orders.Where(r => r.PaymentDate.Date <= endDate.Value.Date);
            }
            var values = await orders.OrderByDescending(d => d.PaymentDate).ToListAsync();

            sheet.Cells["A1"].Value = "شناسه سفارش";

            sheet.Cells["B1"].Value = "محصول";
            sheet.Cells["B1"].Style.WrapText = true;

            sheet.Cells["C1"].Value = "تعداد فروش";
            sheet.Cells["D1"].Value = "نوع محصول";
            sheet.Cells["E1"].Value = "تاریخ فروش";
            sheet.Cells["F1"].Value = "قیمت واحد";
            sheet.Cells["G1"].Value = "قیمت کل";

            var counter = 2;

            foreach (var item in values)
            {
                foreach (var orderDetail in item.Details.OrderByDescending(d => d.StackType))
                {
                    sheet.Cells[$"A{counter}"].Value = item.Id;
                    sheet.Cells[$"B{counter}"].Value = orderDetail.Product.ProductTitle;
                    sheet.Cells[$"C{counter}"].Value = orderDetail.Count;
                    var type = "";
                    switch (orderDetail.StackType)
                    {
                        case StackType.NimKilo:
                            type = "نیم کیلو";
                            break;
                        case StackType.YekKilo:
                            type = "یک کیلو";
                            break;
                        default:
                            type = orderDetail.GetWeight();
                            break;
                    };
                    sheet.Cells[$"D{counter}"].Value = type;
                    sheet.Cells[$"E{counter}"].Value = item.PaymentDate.ToPersianDateTime();
                    sheet.Cells[$"F{counter}"].Value = orderDetail.Price.TooMan();
                    sheet.Cells[$"G{counter}"].Value = orderDetail.TotalPrice;

                    counter += 1;
                }
            }
            var lastRecord = counter += 1;
            sheet.Cells[$"G{lastRecord}"].Value = values.Sum(s => s.Price).TooMan();
            sheet.Cells[$"G{lastRecord}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
            sheet.Cells[$"C{lastRecord}"].Value = values.Sum(s => s.ItemCount).ToString("#,0");
            sheet.Cells[$"C{lastRecord}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
            xlPackage.Save();
            stream.Position = 0;
            return stream;
        }
    }
}
