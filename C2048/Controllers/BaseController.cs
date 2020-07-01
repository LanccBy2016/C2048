using Common;
using C2048.WebExtends.MVC;
using BLL;
using Model;
using System.Web.Mvc;
using System;

namespace C2048.Controllers
{
    [LoginAuthFilter]
    [CustomErrorFilter]
    public class BaseController : Controller
    {
        /// <summary>
        /// 登录用户信息
        /// </summary>
        public LoginInfoModel LoginInfoModel
        {
            get
            {
                return GetLoginInfo();
            }
        }
        /// <summary>
        /// 获取全局用户信息
        /// </summary>
        /// <returns></returns>
        [SkipLogin]
        public LoginInfoModel GetLoginInfo()
        {
            var model = new LoginInfoModel();
            var loginCookie = CookiesHelper.GetCookie(WebConfigOperation.CookieName);//是否已存在登录的用户cookie
            if (loginCookie != null)
            {
                //2.获取用户信息
                model.UserInfo = new LoginBLL().GetUserInfo(loginCookie.Value);
                if (model.UserInfo == null)
                {
                    return model;
                }

            }
            var ykCookie = CookiesHelper.GetCookie(WebConfigOperation.YkCookieName);
            if (ykCookie == null)
            {
                var yk = EncryptAndDecrypt.Encrypt(DateTime.Now.ToString());
                CookiesHelper.AddCookie(WebConfigOperation.YkCookieName,yk);
                CookiesHelper.SetCookie(WebConfigOperation.YkCookieName, DateTime.Now.AddMonths(1));
                model.ykCookie = yk;
            }
            else
            {
                model.ykCookie = ykCookie.Value.ToString();
            }
            
            return model;
        }
    }
}