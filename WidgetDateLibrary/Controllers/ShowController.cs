using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WidgetDateView.Controllers
{
    public class ShowController : Controller
    {
        public ActionResult ShowDataDetail(int pageid, string theparams)
        {
            ViewBag.pageid = pageid;
            ViewBag.theparams = theparams;
            return View();
        }

        public ActionResult OnWindow(int pageid, string theparams)
        {
            ViewBag.pageid = pageid;
            ViewBag.theparams = theparams;
            return View();
        }
    }
}
