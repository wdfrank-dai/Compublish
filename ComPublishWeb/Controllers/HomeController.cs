using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ComPublishWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string style)
        {
            ViewBag.Message = "��ӭ���������ۺϷ���ϵͳ.";
            if (style != null)
                ViewBag.NowPageStyle = style;
            var res = new WidgetDate.Repository.InitPageRepository().GetInitPageAppInfo();
            return View(res.Where(o => o.ShowType != 2));
        }



        public ActionResult About()
        {
            return View();
        }
        public ActionResult Channels()
        {
            ViewBag.Message = "��ӭ���������ۺϷ���ϵͳ.";

            return View(new WidgetDate.Repository.InitPageRepository().GetAuthedInitPageAppInfo(HttpContext.User.Identity.Name.Split(',')[0]).Where(o => o.ShowType != 2));
        }

        public ActionResult BookSearch()
        {
            return View();
        }

        public ActionResult ThirdAppEnter()
        {
            ViewBag.ThirdAppName = "�ҵ�ͼ���";
            return View();
        }

        public ActionResult ThirdAppEnterErr()
        {
            ViewBag.ThirdAppName = "�ҵ�ͼ���";
            return View();
        }

        public ActionResult _SideBar()
        {

            return View(new WidgetDate.Repository.InitPageRepository().GetInitPageAppInfo().Where(o => o.ShowType == 2));
        }
    }
}
