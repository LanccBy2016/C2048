using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C2048.WebExtends.MVC
{
    public class LoginAuthFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验证权限（行为过滤器，action执行前会先执行这里）
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(SkipLoginAttribute), false) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipLoginAttribute), false))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[WebConfigOperation.CookieName];
                if (cookie == null)
                {
                    //var para = HttpContext.Current.Request.Url.Query;
                    //var url = "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName + para;
                    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", url }));
                    filterContext.Result = new JsonResult
                    {
                        Data = new { Code = 2001, Msg = "请登录后重试" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return;
                }
                else
                {

                    var CookieStr = cookie.Value;
                    var JsonStr = EncryptAndDecrypt.Decrypt(CookieStr);
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(JsonStr);
                    if (userInfo == null)
                    {
                        //var para = HttpContext.Current.Request.Url.Query;
                        //var url = "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName + para;
                        //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", url }));
                        filterContext.Result = new JsonResult
                        {
                            Data = new { Code = 2001, Msg = "请登录后重试" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        return;
                    }

                }


            }

        }
    }
    /// <summary>
    /// 用于标识可跳过登陆验证的接口
    /// </summary>
    public class SkipLoginAttribute : ActionFilterAttribute
    {

    }
}