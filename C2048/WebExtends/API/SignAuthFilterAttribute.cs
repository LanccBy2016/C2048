using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using Common;
using Newtonsoft.Json;

namespace C2048.WebExtends.API
{

    /// <summary>
    /// WebAPI签名l验证信息
    /// </summary>
    public class SignAuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            if (actionContext.ActionDescriptor.GetCustomAttributes<SkipSignAttribute>(false).Count == 0
                && actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<SkipSignAttribute>(false).Count == 0

                ) //移动端启动签名认证
            {
                var requestHeader = HttpContext.Current.Request.Headers;
                //请求的签名
                var Sign = requestHeader.Get("Owin-Sign");

                //请求头信息
                var postValue = new SortedDictionary<string, string>();//排序字典，按照key排序
                var Uid = requestHeader.Get("Owin-Uid"); //登录用户的ID或机器ID
                var Ts = requestHeader.Get("Owin-Ts");//10位时间戳
                var Version = requestHeader.Get("Owin-Version");//10位时间戳
                postValue.Add("Owin-Uid", Uid);//API账户名称
                postValue.Add("Owin-Ts", Ts);//10位时间戳 
                postValue.Add("Owin-Rand", requestHeader.Get("Owin-Rand"));//随机数

                string APISignPublicKey = WebConfigOperation.APISignPublicKey;//客户端和手机端保持一致公钥

                string mysign = SignHelper.GetResponseMysign(postValue, APISignPublicKey);

                if (!Sign.Equals(mysign, StringComparison.InvariantCultureIgnoreCase))
                {
                    var data = new { Code = 1003, Msg = "签名认证错误", Success = false };
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.GetEncoding("UTF-8"), "application/json")
                    };
                    return;
                }
            }
            //验证通过
            base.OnActionExecuting(actionContext);
        }
        /// <summary>
        /// 验证请求头
        /// </summary>
        /// <param name="Sign"></param>
        /// <param name="Ts"></param>
        /// <param name="Uid"></param>
        /// <returns></returns>
        private string CheckRequestHeader(string Sign, string Ts, string Uid, string Vesion)
        {
            string Msg = "";
            //请求签名 |10位时间戳 |API账户名称|版本号
            if (string.IsNullOrWhiteSpace(Sign) || string.IsNullOrWhiteSpace(Ts) || string.IsNullOrWhiteSpace(Uid) || string.IsNullOrWhiteSpace(Vesion))
            {
                Msg = "认证信息格式错误";
            }
            return Msg;
        }


    }
    /// <summary>
    /// 用它标识的对象可以跳过签名
    /// </summary>
    public class SkipSignAttribute : Attribute
    {
    }
}