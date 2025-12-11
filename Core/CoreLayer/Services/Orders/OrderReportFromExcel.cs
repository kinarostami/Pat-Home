using DataLayer.Context;
using System;
using System.Threading.Tasks;

namespace CoreLayer.Services.Orders
{
    public class OrderReportFromExcel : BaseService
    {
        public OrderReportFromExcel(AppDbContext context) : base(context)
        {


        }

        public async Task ExportReport(DateTime? startDate, DateTime? endDate)
        {
   
            //var sheet = workbook.CreateWorkSheet($"گزارشات");

            //var orders = Table<Order>().Where(r => r.IsFinally);
            //if (startDate != null)
            //{
            //    orders = orders.Where(r => r.PaymentDate.Date >= startDate.Value.Date);
            //}
            //if (endDate != null)
            //{
            //    orders = orders.Where(r => r.PaymentDate.Date <= endDate.Value.Date);
            //}
            //var values = await orders.OrderByDescending(d => d.PaymentDate).ToListAsync();

            //sheet["A1"].Value = "شناسه سفارش";
            //sheet["B1"].Value = "محصول";
            //sheet["C1"].Value = "تعداد فروش";
            //sheet["D1"].Value = "نوع محصول";
            //sheet["E1"].Value = "تاریخ فروش";
            //sheet["F1"].Value = "قیمت واحد";
            //sheet["G1"].Value = "قیمت کل";
            ////sheet["G1"].Value = "شناسه سفارش";
            ////sheet["H1"].Value = "شناسه سفارش";
            ////sheet["I1"].Value = "شناسه سفارش";
            ////sheet["J1"].Value = "شناسه سفارش";
            //var counter = 2;

            //foreach (var item in values)
            //{
            //    foreach (var orderDetail in item.Details.OrderByDescending(d => d.StackType))
            //    {
            //        sheet[$"A{counter}"].Value = item.Id;
            //        sheet[$"B{counter}"].Value = orderDetail.Product.ProductTitle;
            //        sheet[$"C{counter}"].Value = orderDetail.Count;
            //        sheet[$"D{counter}"].Value = orderDetail.StackType==StackType.YekKilo?"یک کیلو ای":"نیم کیلو ای";
            //        sheet[$"E{counter}"].Value = item.PaymentDate.ToPersianDateTime();
            //        sheet[$"F{counter}"].Value = orderDetail.Price.TooMan();
            //        sheet[$"G{counter}"].Value = orderDetail.TotalPrice.TooMan();

            //        counter += 1;
            //    }
            //}
            //var lastRecord = counter += 1; 
            //sheet[$"G{lastRecord}"].Value = sheet[$"G2:G{lastRecord}"].Sum();
            //sheet[$"F{lastRecord}"].Value = sheet[$"F2:F{lastRecord}"].Sum();
       
        }
    }
}
