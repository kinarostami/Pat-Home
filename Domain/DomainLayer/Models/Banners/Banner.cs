using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Banners;

public class Banner : BaseEntity
{
    public string Url { get; set; }
    public string ImageName { get; set; }
    public BannerPositions Positions { get; set; }
    public bool IsActive { get; set; }
}
public enum BannerPositions
{
    بالای_اسلایدر_فروشگاه = 1,
    بند_انگشتی_زیر_اسلایدر_فروشگاه = 2,
    بنر_های_سمت_راست_فروشگاه = 3,
    بنر_وسط_صفحه_فروشگاه = 4,
    آخرین_بنر_پایین_صفحه_فروشگاه = 5
}
