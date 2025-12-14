using System;
using System.Threading.Tasks;
using CoreLayer.Services.Newsletters;
using Quartz;

namespace Eshop.Infrastructure.Quartz.Jobs
{
    public class NewsletterJob:IJob
    {
        private readonly INewsletterService _newsletterService;

        public NewsletterJob(INewsletterService newsletterService)
        {
            _newsletterService = newsletterService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _newsletterService.SendNewsletters();
            }
            catch
            {
              //Ignor
            }
        }
    }
}