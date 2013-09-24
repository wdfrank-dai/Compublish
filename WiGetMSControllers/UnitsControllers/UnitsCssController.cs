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
    public class UnitsCssController : Controller
    {
        //
        // GET: /UnitsCss/

        static int UnitsID = 0; 

        public ActionResult UnitsCss()
        {
            return View();
        }


        public ActionResult UnitsCssEditWindow(int confid)
        {
            var rep = new UnitsRepository().GetViewDataForCss(confid);
            PageRepository res = new PageRepository();
            ViewData["Unitsid"] = new SelectList(res.units(), "id", "unitname", rep.unitname);
            UnitsCssRepository re = new UnitsCssRepository();
            if (confid == 0)
                return null;
            return PartialView("../Units/UnitsCss/UnitsCssEditWindow", re.GetUnitsCssInfo(confid));
        }


        public ActionResult UnitsCssInsertWindow(int Unitsid)
        {
            PageRepository re = new PageRepository();
            UnitsID = Unitsid;
            ViewData["Unitsid"] = new SelectList(re.units(), "id", "unitname");
            return PartialView("../Units/UnitsCss/UnitsCssAddWindow");
        }


        [HttpPost]
        public string Modify(ApplicationUnitsCss unitscss)   //点击保存按钮、重置调用。用来完成添加、修改的数据存储
        {
            string res = "操作成功";
            UnitsCssRepository systemdb = new UnitsCssRepository();

            if (unitscss.id != 0)   //修改
            {
                try
                {
                    if (systemdb.UpdateUnitsCssByAdmin(unitscss))
                        res = "操作成功";
                }
                catch
                {
                    res = "操作失败";
                }
            }
            else                 // 添加操作
            {
               // string ad = Request.Form["Units"];
                unitscss.unitsid = UnitsID;
                if (systemdb.InsertUnitsCssByAdmin(unitscss))
                    res = "操作成功";
            }
            ViewData["ActionMessagesForAdd"] = res;
            //return PartialView("../Shared/ShowActionMessage");
            return res;
        }


        [GridAction]
        public ActionResult SelectUnitsCssAjaxEditing(int unitsid, string cssname)
        {
            if (unitsid != 0)
                Session["nowunitid"] = unitsid;
            else if (Session["nowunitid"] != null)
                unitsid = int.Parse(Session["nowunitid"].ToString());
            
            UnitsCssRepository re = new UnitsCssRepository();
            if(unitsid == 0)
                if(cssname != null)
                   return View(new GridModel(re.GetAllCssByUnitsname(cssname)));

            return View(new GridModel(re.GetAllUnitsCss(unitsid)));
        }

        [GridAction]
        public ActionResult _DeleteUnitsCssAjaxEditing(int id)
        {
            UnitsCssRepository rep = new UnitsCssRepository();
            rep.DeleteUnitsCssByAdmin(id);
            return View(new GridModel(rep.GetAllUnitsCssForDel()));
        }


    }
}
