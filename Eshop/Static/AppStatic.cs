using System.Collections.Generic;
using DomainLayer.Models;
using DomainLayer.Models.Roles;

namespace Eshop.Static
{
    public static class AppStatic
    {
        public static SiteSetting SiteSettings { get; set; }
        public static List<RolePermission> RolePermissions { get; set; }
        public static PopUpModel PopUpModel { get; set; }

        //ZarinPal MerchantId
        public static string MerchantId => "4b581516-a39b-43c2-bb41-1f8730e60819";
    }
}