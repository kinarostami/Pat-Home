using CoreLayer.Services;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLayer.Utilities
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IAppContext _appContext;
        private readonly Permissions _permission;

        public PermissionCheckerAttribute(Permissions permission)
        {
            _permission = permission;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _appContext = (IAppContext)context.HttpContext.RequestServices.GetService(typeof(IAppContext));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!_appContext.CheckPermission(_permission))
                {
                    context.Result = new RedirectResult("/");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Auth/Login?returnUrl=" + context.HttpContext.Request.Path);
            }
        }

    }
}
