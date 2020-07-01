using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Common;
using Model;
using Newtonsoft.Json;

namespace C2048.WebExtends.API
{
    /// <summary>
    /// 登陆认证信息
    /// </summary>
    public class LoginAuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (
                actionContext.ActionDescriptor.GetCustomAttributes<SkipLoginAttribute>(false).Count == 0 &&
                 actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<SkipLoginAttribute>(false).Count == 0
                )
            {
                var CookieUserName = WebConfigOperation.CookieName;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieUserName];
                if (cookie == null)
                {
                    var data = new { Code = 2001, Msg = "用户未登陆", Success = false };
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                    };
                    return;
                }
                else
                {
                    var CookieEnStr = cookie.Value;
                    var JsonStr = EncryptAndDecrypt.Decrypt(CookieEnStr);

                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(JsonStr);
                    if (userInfo == null)
                    {
                        var data = new { Code = 2001, Msg = "用户未登陆", Success = false };
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                        };
                        return;
                    }
                }
            }
            //验证通过
            base.OnActionExecuting(actionContext);
        }
    }
    /// <summary>
    /// 用于标识可跳过登陆验证的接口
    /// </summary>
    public class SkipLoginAttribute : ActionFilterAttribute
    {

    }
}