using C2048.WebExtends.MVC;
using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C2048.Controllers
{
    public class HomeController : BaseController
    {
        [SkipLogin]
        public ActionResult Index()
        {
            if (LoginInfoModel != null && LoginInfoModel.UserInfo != null)
            {
                ViewBag.UserID = LoginInfoModel.UserInfo.UserID;
                ViewBag.MaxScore = new GameInfoBLL().GetMaxScore(LoginInfoModel.UserInfo);
            }
            return View();
        }

        /// <summary>
        /// 游戏结束后提交
        /// </summary>
        /// <param name="type">0重新开始/1提交并重新开始</param>
        /// <returns></returns>
        [SkipLogin]
        public ActionResult ScoreSubmit(int type)
        {
            var key1 = LoginInfoModel.UserInfo != null ? LoginInfoModel.UserInfo.UserID:null;
            var key2 = LoginInfoModel.ykCookie;
            var key = key1 ?? key2;

            var data = new GameInfoBLL().GetGameData(key);
            if (!data.IsOver)
            {
                return Json(new { Code = 2002, Msg = "游戏还未结束,再想想吧~" });
            }
            if (type == 1)
            {
                if (key1 != null)
                {
                    new GameInfoBLL().AddGameLog(LoginInfoModel.UserInfo, data.Score);
                }
                else
                {
                    return Json(new { Code = 2003, Msg = "游客朋友无法保存战绩哦~" });
                }
            }
            data = new GameInfo();
            new GameInfoBLL().RefNum(ref data);
            new GameInfoBLL().SetGameData(key,data);
            return Json(new { Code = 2000, Msg = "操作成功"});
        }

        /// <summary>
        /// 用户发起移动请求
        /// </summary>
        /// <param name="keyCode">操作类型（对应键盘编码）</param>
        /// <returns></returns>
        [HttpPost]
        [SkipLogin]
        public ActionResult UserMove(int keyCode)
        {
            var key = LoginInfoModel.UserInfo != null ? LoginInfoModel.UserInfo.UserID : LoginInfoModel.ykCookie;

            var data = new GameInfoBLL().GetGameData(key);
            switch (keyCode)
            {
                case 37:
                    new GameInfoBLL().MoveLeft(ref data);
                    new GameInfoBLL().RefNum(ref data);
                    break;
                case 38:
                    new GameInfoBLL().MoveUP(ref data);
                    new GameInfoBLL().RefNum(ref data);
                    break;
                case 39:
                    new GameInfoBLL().MoveRight(ref data);
                    new GameInfoBLL().RefNum(ref data);
                    break;
                case 40:
                    new GameInfoBLL().MoveDown(ref data);
                    new GameInfoBLL().RefNum(ref data);
                    break;
            }
            //执行完用户操作指令后保存游戏状态
            new GameInfoBLL().SetGameData(key, data);
            return Json(new {Data=data.Data,Code=2000,Score=data.Score});
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SkipLogin]
        public ActionResult GameOver()
        {
            var key = LoginInfoModel.UserInfo != null ? LoginInfoModel.UserInfo.UserID : LoginInfoModel.ykCookie;
            var data=new GameInfoBLL().GetGameData(key);
            data.IsOver = true;
            new GameInfoBLL().SetGameData(key,data);
            return Json(new { Code = 2000, Msg = "游戏结束" });
        }
    }
}