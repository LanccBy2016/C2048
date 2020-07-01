using C2048.WebExtends.MVC;
using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace C2048.Controllers
{
    [SkipLogin]
    public class LoginController : BaseController
    {

        public ActionResult Login(string uid, string pwd)
        {
            //登陆信息
            var info = new LoginBLL().LoginIn(uid, pwd);

            //登陆前的游客游戏
            var ykdata = CacheExts<GameInfo>.GetValue(LoginInfoModel.ykCookie);
            if (ykdata == null)
            {
                ykdata = new GameInfo();
                new GameInfoBLL().RefNum(ref ykdata);
            }


            if (info != null)
            {
                var data= CacheExts<GameInfo>.GetValue(info.UserID);
                if (data==null||data.Score < ykdata.Score)
                {
                    //如果此用户不存在游戏记录，或用户的游戏分数低于游客状态下的得分，则用游客的游戏状态覆盖用户的游戏状态
                    CacheExts<GameInfo>.SetValue(info.UserID, ykdata,noSlidingExpiration:false);
                }
                return Json(new { Code = 2000, Msg = "登陆成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Code = 2001, Msg = "登陆失败" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}