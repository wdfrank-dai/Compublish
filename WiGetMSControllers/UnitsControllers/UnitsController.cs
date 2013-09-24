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
        //
        // GET: /Units/

        public ActionResult Index()
        {
            var ad = new UnitsRepository();
            ViewData["datasourceid"] = new SelectList(ad.dsnamelist(), "id", "dsname");
            ViewData["showstyleid"] = new SelectList(ad.showstyle(), "id", "name");
            ViewData["NextPageid"] = new SelectList(ad.pagelist(), "id", "pagename");
            ViewData["NextPageShowTypeid"] = new SelectList(NextPageShowTypeList.GetNextPageShowTypeList(), "NextPageShowType", "NextPageShowType");
            Session["nowunitid"] = null;
            return View();
        }

        public ActionResult AddUnits()
        {
            var ad = new UnitsRepository();
            ViewData["datasourceid"] = new SelectList(ad.dsnamelist(), "id", "dsname");
            ViewData["showstyleid"] = new SelectList(ad.showstyle(), "id", "name");
            ViewData["NextPageid"] = new SelectList(ad.pagelist(), "id", "pagename");
            ViewData["NextPageShowTypeid"] = new SelectList(NextPageShowTypeList.GetNextPageShowTypeList(), "NextPageShowType", "NextPageShowType");
            return PartialView("AddUnits");
        }

        public ActionResult EditUnits1(int confid)
        {
            var rep = new UnitsRepository().GetViewDataForUnits(confid);
            var rep2 = new UnitsRepository().GetViewDatanextpage(confid);
            var rep3 = new UnitsRepository().GetViewDatanextpageshowtype(confid);
            var ad = new UnitsRepository();
            ViewData["datasourceid"] = new SelectList(ad.dsnamelist(), "id", "dsname", rep.datasourceid);
            ViewData["showstyleid"] = new SelectList(ad.showstyle(), "id", "name",rep.showstyleid);
            ViewData["NextPageid"] = new SelectList(ad.pagelist(), "id", "pagename", rep2.NextPage);
            ViewData["NextPageShowTypeid"] = new SelectList(NextPageShowTypeList.GetNextPageShowTypeList(), "NextPageShowType", "NextPageShowType", rep3.nextpageshowtype);
            return PartialView("EditUnits", new UnitsRepository().MeetingDetailInfo(confid));
        }

        //Telerik调用的方法
        [GridAction]
        public ActionResult UnitsShow()
        {
            UnitsRepository rep = new UnitsRepository();
            return View(new GridModel(rep.GetAllUnits()));
        }

        //添加控件
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddUnits(ApplicationUnits Dataid)
        {
            string nownextpage = Request.Form["NextPageid"].ToString();
            string nownextpageshowtype = Request.Form["NextPageShowTypeid"].ToString();
            if (nownextpage != "")
            {
                Dataid.NextPage = int.Parse(nownextpage);
            }
            if (nownextpageshowtype != "" && nownextpageshowtype != "未选择")
            {
                Dataid.NextPageShowType = nownextpageshowtype;
            }
            Dataid.NextPageShowType = nownextpageshowtype;
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
            if (Request.Form["NextPageid"] != "")
            {
                data.NextPage = int.Parse(Request.Form["NextPageid"]);
            }
            if (Request.Form["NextPageShowTypeid"] != "" && Request.Form["NextPageShowTypeid"] != "未选择")
            {
                data.nextpageshowtype = Request.Form["NextPageShowTypeid"];
            }
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

        public class NextPageShowTypeList
        {
            public string nextpageshowtype { get;set;}
            public static List<NextPageShowTypeList> GetNextPageShowTypeList()
            {
                return new List<NextPageShowTypeList>
                {
                    new NextPageShowTypeList { nextpageshowtype = "未选择"}, 
                    new NextPageShowTypeList { nextpageshowtype = "OnWindow" },
                    new NextPageShowTypeList { nextpageshowtype = "ShowDataDetail" },
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
