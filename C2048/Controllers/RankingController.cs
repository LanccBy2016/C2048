using C2048.WebExtends.MVC;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace C2048.Controllers
{
    [SkipLogin]
    public class RankingController : Controller
    {
        // GET: Ranking
        public ActionResult Index()
        {
            ViewBag.RankingList = new GameInfoBLL().GetRankingList();
            return View();
        }
    }
}