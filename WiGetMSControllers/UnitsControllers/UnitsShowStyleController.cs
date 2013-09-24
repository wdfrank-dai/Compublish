using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models.Repository;
using WiGetMS.Models;

namespace WidgetAdmin.WiGet.Controllers
{
    public class UnitsShowStyleController : Controller
    {
        //
        // GET: /UnitsShowStyle/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUnitsShowStyle()
        {
            return PartialView("../UnitsShowStyle/UnitsShowStyle");
        }

        public ActionResult EditUnitsShowStyle(int confid)
        {
            return PartialView("../UnitsShowStyle/UnitsShowStyle", new UnitsShowStyleRepository().MeetingDetailInfo(confid));
        }

        //Telerik调用的方法
        [GridAction]
        public ActionResult UnitsShowStyleShow()
        {
            UnitsShowStyleRepository rep = new UnitsShowStyleRepository();
            return View(new GridModel(rep.GetAllUnitsShowStyle()));
        }

        //添加控件
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult AddUnitsShowStyle(string Dataid)
        //{
        //    //ApplicationUnitsShowStyle ads = new ApplicationUnitsShowStyle();
           
        //    string res = "控件添加成功";
        //    UnitsShowStyleRepository rep = new UnitsShowStyleRepository();
        //    try
        //    {
        //        if (TryUpdateModel(ads))
        //        {
        //            rep.Insert(ads);
        //        }
        //    }
        //    catch
        //    {
        //        res = "控件添加失败";
        //    }
        //    ViewData["ActionMessagesForAdd"] = res;
        //    return PartialView("../Shared/ShowActionMessage");
        //}

        //修改控件
        [GridAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditUnitsShowStyle(UnitsShowStyle unitsshow)
        {
            if (unitsshow.id == 0)
            {
                //UnitsShowStyle ads = new UnitsShowStyle();
                ApplicationUnitsShowStyle ads = new ApplicationUnitsShowStyle();
                string res = "控件添加成功";
                UnitsShowStyleRepository rep = new UnitsShowStyleRepository();
                try
                {
                    TryUpdateModel(ads);                    
                    rep.Insert(ads);                                
                }
                catch
                {
                    res = "控件添加失败";
                }
                ViewData["ActionMessagesForAdd"] = res;
                return PartialView("../Shared/ShowActionMessage");
            }
            else
            {
                new UnitsShowStyleRepository().GetEditUnitsShowStyle(unitsshow);
                ViewData["ActionMessagesForEdit"] = "修改成功";
                return PartialView("../Shared/ShowActionMessage");
            }
        }

        //删除控件
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult DeleteUnitsShowStyle(int unitsshowid)
        {
            string res = "数据元删除成功";
            UnitsShowStyleRepository rep = new UnitsShowStyleRepository();
            try
            {
                rep.Delete(unitsshowid);
                new UnitsShowStyleRepository().GetAllUnitsShowStyle(unitsshowid);
            }
            catch
            {
                res = "数据元删除失败";
            }
            ViewData["ActionMessagesForDel"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

    }
}
