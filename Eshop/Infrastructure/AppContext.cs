using Common.Application.UserUtil;
using CoreLayer.Services;
using CoreLayer.Services.Users;
using DataLayer.Context;
using DomainLayer.Models;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Users;
using Eshop.Static;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Infrastructure
{
    public class AppContext : BaseService, IAppContext
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly IUserService _user;
        private readonly AppDbContext _dbContext;

        public AppContext(IHttpContextAccessor accessor, ITempDataDictionaryFactory tempDataDictionaryFactory, IUserService user, AppDbContext dbContext) : base(dbContext)
        {
            _accessor = accessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _user = user;
            _dbContext = dbContext;
        }


        public async Task AddOrEditPopUp(PopUpModel popUp)
        {
            if (popUp.Id == 0)
            {
                Insert(popUp);
                await Save();
                AppStatic.PopUpModel = popUp;
                return;
            }
            Update(popUp);
            await Save();
            AppStatic.PopUpModel = popUp;
        }

        public async Task<PopUpModel> GetPopUp()
        {
            if (AppStatic.PopUpModel == null)
            {
                var popUp = await _dbContext.PopUpModel.FirstOrDefaultAsync();
                AppStatic.PopUpModel = popUp;
                return popUp;
            }

            return AppStatic.PopUpModel;
        }

        public string SiteBaseUrl
        {
            get
            {
                try
                {
                    return $"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host}";
                }
                catch
                {
                    return "https://daftarjan.com/";
                }
            }
        }


        public List<RolePermission> PermissionModel
        {
            get
            {
                if (AppStatic.RolePermissions == null)
                {
                    AppStatic.RolePermissions = Task.Run(GetPermissionModel).Result;
                }
                return AppStatic.RolePermissions;
            }
        }

        public bool CheckPermission(Permissions permission)
        {
            if (!_accessor.HttpContext.User.Identity.IsAuthenticated) return false;
            var userRoles = _dbContext.UserRoles.Where(r => r.UserId == CurrentUserId).Select(r => r.RoleId).ToList();

            foreach (var item in userRoles)
            {
                if (PermissionModel.Where(p => p.PermissionId == permission).Any(p => p.RoleId == item))
                    return true;
            }
            return false;
        }

        public SiteSetting SiteSettings
        {
            get
            {
                if (AppStatic.SiteSettings == null)
                {
                    SetSiteSettings();
                }

                return AppStatic.SiteSettings;
            }
        }



        public async Task<User> CurrentUser()
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return await _user.GetSingleUser(CurrentUserId);
            }

            return null;
        }

        public bool IsError => _tempDataDictionaryFactory.GetTempData(_accessor.HttpContext)["Error"] != null;
        public bool IsSuccess => _tempDataDictionaryFactory.GetTempData(_accessor.HttpContext)["Success"] != null;
        public long CurrentUserId => (_accessor.HttpContext.User.Identity.IsAuthenticated) ? _accessor.HttpContext.User.GetUserId() : 0;

        public void SetSiteSettings()
        {
            AppStatic.SiteSettings = _dbContext.SiteSettings.FirstOrDefault();
        }

        public HttpRequest Request => _accessor.HttpContext.Request;



        #region Utilities
        private async Task<List<RolePermission>> GetPermissionModel()
        {
            if (AppStatic.RolePermissions != null)
            {
                var rolePermissions = AppStatic.RolePermissions;

                return rolePermissions;
            }
            else
            {
                var rolePermissions = await _dbContext.RolePermissions.ToListAsync();

                return rolePermissions;
            }

        }
        #endregion
    }
}