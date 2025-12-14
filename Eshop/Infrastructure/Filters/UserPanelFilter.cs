using CoreLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Eshop.Infrastructure.Filters
{
    [Authorize]
    public class UserCompleted : ActionFilterAttribute
    {
        private readonly IAppContext _appContext;
        
        public UserCompleted(IAppContext appContext)
        {
            _appContext = appContext;
        }

        

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var user = _appContext.CurrentUser().Result;
            if (user != null)
            {
                var path = context.HttpContext.Request.Path;
                if (!user.IsCompleteProfile && !path.ToString().ToLower().EndsWith("/Profile/edit"))
                {
                    context.HttpContext.Response.Redirect("/Profile/Edit?completed=false");

                }
            }
            else
            {
                var path = context.HttpContext.Request.Path;
                context.HttpContext.Response.Redirect("/Auth/Login?returnUrl="+path);
            }

            base.OnResultExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
           
            base.OnActionExecuted(context);
        }
    }
}
