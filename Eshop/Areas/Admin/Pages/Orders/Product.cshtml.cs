using Common.Application.DateUtil;
using CoreLayer.DTOs.Profile;
using CoreLayer.Services.Orders;
using CoreLayer.Utilities;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Pages.Orders;

[PermissionChecker(Permissions.ادمین)]
public class ProductModel : PageUtil
{
    private readonly IOrderService _orderService;
    private readonly OrderReportToExcel _reportToExcel;
    public ProductModel(IOrderService orderService, OrderReportToExcel reportToExcel)
    {
        _orderService = orderService;
        _reportToExcel = reportToExcel;
    }

    public UserOrdersFilter OrdersModel { get; set; }

    public async Task OnGet(int pageId = 1, long orderId = 0, long userId = 0, OrderStatus status = 0, string phone = "", string startDate = null, string endDate = null)
    {
        DateTime? stDate = startDate.ToMiladi();
        DateTime? enDate = endDate.ToMiladi();
        if (startDate == null)
            stDate = null;

        if (endDate == null)
            enDate = null;
        OrdersModel = await _orderService.GetOrders(pageId, 14, userId, orderId, status, phone, stDate, enDate);
    }

    public async Task<IActionResult> OnPostReport(string startDate, string endDate)
    {
        var stDate = startDate.ToMiladi();
        var enDate = endDate.ToMiladi();
        var stream = await _reportToExcel.ExportReport(stDate, enDate);

        //download file
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"report.xlsx");
    }

}
