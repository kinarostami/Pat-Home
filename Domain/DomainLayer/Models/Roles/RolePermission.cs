
namespace DomainLayer.Models.Roles;

public class RolePermission:BaseEntity
{
    public long RoleId { get; set; }
    public Permissions PermissionId { get; set; }


    #region Relations
    public Role Role { get; set; }
    #endregion
}
public enum Permissions
{
    ادمین,
    مدیریت_نقش_ها,
    مدیریت_سفارشات,
    مدیریت_کاربران,
    مدیریت_محصولات,
    مدیریت_تیکت_ها,
    مدیریت_اسلایدر_ها,
    مدیریت_بنر_ها,
    مدیریت_مقالات,
    پشتیبان,
    کاربر,
    ویرایش_اطلاعات,
}