using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Newtonsoft.Json;

namespace C2048.WebExtends.API
{
    /// <summary>
    /// WebAPI自定义错误过滤器属性
    /// </summary>
    public class CustomErrorFilterAttribute : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {

            //HttpContextBase context = (HttpContextBase)actionExecutedContext.Request.Properties["MS_HttpContext"];//获取传统context
            //HttpRequestBase request = context.Request;//定义传统request对象 
            //记录异常
            Exception ex = actionExecutedContext.Exception;
            //WebApi上下文
            var requestCont = actionExecutedContext.ActionContext.RequestContext;
            var RequestT = actionExecutedContext.Request.Headers.GetValues("Owin-RequestT");

            #region 异常信息时 记录信息
            //全局API错误处理
            #endregion
            #region 全局错误处理
            var data = new { Code = 5001, Msg = "很抱歉，程序发生异常", Errors = ex.Message, Success = false };
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
            };
            #endregion
            base.OnException(actionExecutedContext);
        }


    }

}