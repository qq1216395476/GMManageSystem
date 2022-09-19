using GMManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GMManageSystem.Common
{
    /// <summary>
    /// 登录验证，权限验证，action过滤
    /// </summary>
    public class LoginFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断是否跳过授权过滤器
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            bool isAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            bool IsAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
            if (!IsAuthenticated)
            {
                if (isAjax)
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new EmptyResult();
                    filterContext.HttpContext.Response.ContentType = "application/json;charset=UTF-8";
                    filterContext.HttpContext.Response.Write(new Result { code = 1, msg = "您还未登录，请先登录！" });
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                }
                return;
            }
        }

    }
}