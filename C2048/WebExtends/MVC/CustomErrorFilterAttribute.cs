using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C2048.WebExtends.MVC
{
    public class CustomErrorFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {

            //如果异常未处理
            if (!filterContext.ExceptionHandled)
            {
                //记录异常
                Exception ex = filterContext.Exception;
                string path = filterContext.HttpContext.Request.Url == null ? "" : filterContext.HttpContext.Request.Url.ToString();
                //获得url请求里的controller和action：
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();
                filterContext.Controller.ViewData["BackUrl"] = "/" + controllerName + "/" + actionName;
                #region 记录Bug信息
                //处理全局错误日志
                #endregion
                HandleErrorInfo modelError = new HandleErrorInfo(ex, controllerName, actionName);
                filterContext.Result = new ViewResult
                {
                    ViewName = this.View,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(modelError),
                };
                filterContext.ExceptionHandled = true;
            }
            base.OnException(filterContext);
        }
    }

}