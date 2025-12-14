using System.Threading.Tasks;
using CoreLayer.Services.Orders;
using Quartz;

namespace Eshop.Infrastructure.Quartz.Jobs
{
    public class OrderJob: IJob
    {
        private readonly IOrderService _orderService;

        public OrderJob(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _orderService.ClearOrdersDiscount();
        }
    }
}