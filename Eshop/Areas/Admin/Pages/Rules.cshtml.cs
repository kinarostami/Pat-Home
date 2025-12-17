using CoreLayer.Services.SiteRules;
using CoreLayer.Utilities;
using DomainLayer.Models;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages
{
    [PermissionChecker(Permissions.ادمین)]
    public class RulesModel : PageUtil
    {
        private readonly ISiteRuleService _rule;

        public RulesModel(ISiteRuleService rule)
        {
            _rule = rule;
        }
        [BindProperty]
        public SiteRules SiteRules { get; set; }
        public async Task OnGet()
        {
            SiteRules = await _rule.GetRules();
        }

        public async Task<IActionResult> OnPost()
        {
            return await TryCatch(async () =>
            {
                await _rule.AddOrEdit(SiteRules);

            }, successReturn: "/Admin", errorReturn: "/Admin");

        }
    }
}
