using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WiGetMSModelsLibrary;
using WiGetMS.Models.Repository;
using WiGetMS.Models;

namespace WidgetAdmin.WiGetMS.Controllers
{
    public class UnitsController : Controller
    {
        static int APPID = 0;
        static int APPID2 = 0;

        public ActionResult Index()
        {
            var ad = new UnitsRepository();
            ViewData["datasourceid"] = new SelectList(ad.dsnamelist(), "id", "dsname");
            ViewData["showstyleid"] = new SelectList(ad.showstyle(), "id", "name");
            ViewData["NextUnitid"] = new SelectList(ad.unitlist(), "id", "unitname");
            ViewData["NextUnitShowTypeid"] = new SelectList(NextUnitShowTypeList.GetNextUnitShowTypeList(), "NextUnitShowType", "NextUnitShowType");
            ViewData["AuthErrorAlertUnit"] = new SelectList(ad.unitlist(), "id", "unitname");
            Session["nowunitid"] = null;
            return View();
        }

        public ActionResult AddUnits()
        {   
            var ad = new UnitsRepository();
            ViewData["datasourceid"] = new SelectList(ad.dsnamelist(), "id", "dsname");
            ViewData["showstyleid"] = new SelectList(ad.showstyle(), "id", "name");
            ViewData["NextUnitid"] = new SelectList(ad.unitlist(), "id", "unitname");
            ViewData["NextUnitShowTypeid"] = new SelectList(NextUnitShowTypeList.GetNextUnitShowTypeList(), "NextUnitShowType", "NextUnitShowType");
            ViewData["AuthErrorAlertUnit"] = new SelectList(ad.unitlist(), "id", "unitname");
            return PartialView("AddUnits");
        }

        public ActionResult EditUnits1(int confid)
        {
            var rep = new UnitsRepository().GetViewDataForUnits(confid);
            var rep2 = new UnitsRepository().GetViewDatanextpage(confid);
            var rep3 = new UnitsRepository().GetViewDatanextpageshowtype(confid);
            var rep4 = new UnitsRepository().GetViewDataErrorUnit(confid);
            var ad = new UnitsRepository();
            ViewData["datasourceid"] = new SelectList(ad.dsnamelist(), "id", "dsname", rep.datasourceid);
            ViewData["showstyleid"] = new SelectList(ad.showstyle(), "id", "name",rep.showstyleid);
            ViewData["NextUnitid"] = new SelectList(ad.unitlist(), "id", "unitname", rep2.NextUnit);
            ViewData["NextUnitShowTypeid"] = new SelectList(NextUnitShowTypeList.GetNextUnitShowTypeList(), "NextUnitShowType", "NextUnitShowType", rep3.nextunitshowtype);
            ViewData["AuthErrorAlertUnit"] = new SelectList(ad.unitlist(), "id", "unitname",rep4.AuthErrorAlertUnit);
            return PartialView("EditUnits", new UnitsRepository().MeetingDetailInfo(confid));
        }

        //Telerik调用的方法
        [GridAction]
        public ActionResult UnitsShow(int appid)
        {
            APPID = appid;
            APPID2 = appid;
            if (appid != 0)
                Session["nowappid"] = appid;
            else if (Session["nowappid"] != null)
                appid = int.Parse(Session["nowunitid"].ToString());
            UnitsRepository rep = new UnitsRepository();
            if (appid != 0)
                return View(new GridModel(rep.GetAllUnitsByAppid(appid)));
            return View(new GridModel(rep.GetAllUnits()));
          
        }

        [GridAction]
        public ActionResult UnitsShow2()
        {
            UnitsRepository rep = new UnitsRepository();
            return View(new GridModel(rep.GetAllUnits()));
        }

        //[GridAction]
        //public ActionResult SelectUnitsAjaxEditing(int appid)
        //{
        //    if (appid != 0)
        //        Session["nowappid"] = appid;
        //    else if (Session["nowappid"] != null)
        //        appid = int.Parse(Session["nowunitid"].ToString());

        //    UnitsRepository re = new UnitsRepository();
        //    if (appid != 0)
        //        return View(new GridModel(re.GetAllUnitsByAppid(appid)));
        //    return View(new GridModel(re.GetAllUnits()));
        //}


        //添加控件
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUnits(ApplicationUnits Dataid)
        {
            string nownextunit = Request.Form["NextUnitid"].ToString();
            string nownextunitshowtype = Request.Form["NextUnitShowTypeid"].ToString();
            string nowerrorunitid = Request.Form["AuthErrorAlertUnit"].ToString();
            if (nownextunit != "")
            {
                Dataid.NextUnit = int.Parse(nownextunit);
            }
            if (nownextunitshowtype != "" && nownextunitshowtype != "未选择")
            {
                Dataid.NextUnitShowType = nownextunitshowtype;
            }
            if (nowerrorunitid != "")
            {
                Dataid.AuthErrorAlertUnit = int.Parse(nowerrorunitid);
            }
            Dataid.applicationid = APPID;
            Dataid.NextUnitShowType = nownextunitshowtype;
            //ApplicationUnits ads = new ApplicationUnits();
            string res;
            UnitsRepository rep = new UnitsRepository();
            var ad = new UnitsRepository();
            if (rep.Insert(Dataid))
                {
                    res = "控件添加成功";
                }
            else
            {
                res = "控件添加失败";
            }
            //ViewData["datasourceid"] = new SelectList(ad.dsname(), "id", "dsname");
            ViewData["ActionMessagesForAdd"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

        //修改控件
        //[GridAction]
        [HttpPost]
        public ActionResult EditUnits(Units data)
        {
            var ad = new UnitsRepository();
            if (Request.Form["datasourceid"] != "")
            {
                data.datasourceid = int.Parse(Request.Form["datasourceid"]);
            }
            if (Request.Form["showstyleid"] != "")
            {
                data.showstyleid = int.Parse(Request.Form["showstyleid"]);
            }
            if (Request.Form["NextUnitid"] != "")
            {
                data.NextUnit = int.Parse(Request.Form["NextUnitid"]);
            }
            if (Request.Form["NextUnitShowTypeid"] != "" && Request.Form["NextUnitShowTypeid"] != "未选择")
            {
                data.nextunitshowtype = Request.Form["NextUnitShowTypeid"];
            }
            if (Request.Form["AuthErrorAlertUnit"] != "" && Request.Form["AuthErrorAlertUnit"] != "未选择")
            {
                data.AuthErrorAlertUnit = int.Parse(Request.Form["AuthErrorAlertUnit"]);
            }
            data.appid = APPID2;
            new UnitsRepository().GetEditUnits(data);
            ViewData["ActionMessagesForEdit"] = "修改成功";
            return PartialView("../Shared/ShowActionMessage");

        }

        //删除控件
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult DeleteUnits(int unitsid)
        {
            string res = "数据元删除成功";
            UnitsRepository rep = new UnitsRepository();
            UnitsShowStyleRepository rest = new UnitsShowStyleRepository();
            UnitsCssRepository rec = new UnitsCssRepository();
            PageRepository pr = new PageRepository(); 
        
            try
            {
                rep.Delete(unitsid);                   //删除Units
                //rest.Delete(unitsid);                //删除UnitsShowStyle
                rec.DeleteUnitsCssByAdminForUnits(unitsid);    //删除UnitsCss
                pr.DeletePageByUnits(unitsid);         //清空Page中的actionunits
                new UnitsRepository().GetAllUnits(unitsid);
            }
            catch
            {
                res = "数据元删除失败";
            }
            ViewData["ActionMessagesForDel"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

        public class NextUnitShowTypeList
        {
            public string nextunitshowtype { get; set; }
            public static List<NextUnitShowTypeList> GetNextUnitShowTypeList()
            {
                return new List<NextUnitShowTypeList>
                {
                    new NextUnitShowTypeList { nextunitshowtype = "未选择"}, 
                    new NextUnitShowTypeList { nextunitshowtype = "OnWindow" },
                    new NextUnitShowTypeList { nextunitshowtype = "ShowDataDetail" },
                };
            }

        }


      
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUnitsCss(string checkedRecords)
        {
            ApplicationUnitsCss ads = new ApplicationUnitsCss();
            string res = "控件添加成功";
            UnitsRepository rep = new UnitsRepository();
            var ad = new UnitsRepository();
            try
            {
                if (TryUpdateModel(checkedRecords))
                {
                    rep.InsertCss(checkedRecords);
                }
            }
            catch
            {
                res = "控件添加失败";
            }
            //ViewData["datasourceid"] = new SelectList(ad.dsname(), "id", "dsname");
            ViewData["ActionMessagesForAdd"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

    }
}
