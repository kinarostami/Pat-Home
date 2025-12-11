using DomainLayer.Models;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context;

public class DBInitializer 
{
    public static void Initializer(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Roles.Any())
            // DB has been seeded
            return;

        using (var transaction = context.Database.BeginTransaction())
        {
            //Add Role
            var adminRole = new Role
            {
                RoleTitle = "ادمین"
            };
            var userRole = new Role
            {
                RoleTitle = "کاربر"
            };

            context.Roles.AddRange(new[]
            {
                adminRole,
                userRole
            });
            context.SaveChanges();

            var rolePermission = new List<RolePermission>()
            {
                new RolePermission()
                {
                    PermissionId = Permissions.ادمین,
                    RoleId = adminRole.Id
                },
                new RolePermission()
                    {
                        PermissionId = Permissions.مدیریت_کاربران,
                        RoleId = adminRole.Id
                    },
                    new RolePermission()
                    {
                        PermissionId = Permissions.مدیریت_اسلایدر_ها,
                        RoleId = adminRole.Id
                    }, new RolePermission()
                    {
                        PermissionId = Permissions.مدیریت_محصولات,
                        RoleId = adminRole.Id
                    },
                    new RolePermission()
                    {
                        PermissionId = Permissions.پشتیبان,
                        RoleId = adminRole.Id
                    },
                    new RolePermission()
                    {
                        PermissionId = Permissions.مدیریت_تیکت_ها,
                        RoleId = adminRole.Id
                    },
                    new RolePermission()
                    {
                        PermissionId = Permissions.مدیریت_نقش_ها,
                        RoleId = adminRole.Id
                    },
                    new RolePermission()
                    {
                        PermissionId = Permissions.مدیریت_مقالات,
                        RoleId = adminRole.Id
                    },
                    new RolePermission()
                    {
                    PermissionId = Permissions.کاربر,
                    RoleId = userRole.Id
                    },
                    new RolePermission()
                    {
                        PermissionId = Permissions.ویرایش_اطلاعات,
                        RoleId = userRole.Id
                    }
            };
            context.RolePermissions.AddRange(rolePermission);
            context.SaveChanges();
            //Add User
            User user1 = new User
            {
                Password = "25-D5-5A-D2-83-AA-40-0A-F4-64-C7-6D-71-3C-07-AD",//password=12345678
                IsActive = true,
                Name = "هاجر",
                Family = "عسل",
                Email = "daftarjan@gmail.com",
                PhoneNumber = "09170000000",
                SecondActiveCode = "45347"
            };
            context.Users.Add(user1);
            context.SaveChanges();

            //Set Role
            context.UserRoles.Add(new UserRole
            {
                RoleId = adminRole.Id,
                UserId = user1.Id
            });
            context.SaveChanges();


            var siteRules = new SiteRules()
            {
                LastModify = DateTime.Now,
            };
            context.SiteRules.Add(siteRules);
            context.SaveChanges();

            var siteSetting = new SiteSetting()
            {
                Address = " ",
                EmailPassword = " ",
                BaseSiteUrl = "https://hajarasali.com/",
                Email = "info@hajarasali.com",
                EmailSmtpPort = 25,
                EmailSmtpServer = "25",
                EnglishSitName = "DaftarJan",
                LinkDin = null,
                MagDescription = " ",
                MagTitle = " ",
                PersianSitName = "هاجرعسلی",
                PhoneNumber = "09170000000",
                ShopDescription = " ",
                ShopTitle = " ",
                TelePhone = "07142640000",
                Telegram = null,
                Twitter = null,
            };
            context.SiteSettings.Add(siteSetting);
            context.SaveChanges();
            transaction.Commit();
        }
    }
}
