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
    public class DataSourceController : Controller
    {
        //
        // GET: /Data/

        static int APPID = 0;
        static int APPID2 = 0;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddData()
        {
           // PageRepository re = new PageRepository();
            //ViewData["Unitsid"] = new SelectList(re.units(), "id", "unitname");
            return PartialView("../DataSource/AddData");
        }

        public ActionResult EditData(int confid)
        {
            PageRepository res = new PageRepository();
            var rep = new DatasourceRepository().GetViewDataForDataSource(confid);
            ViewData["Unitsid"] = new SelectList(res.units(), "id", "unitname", rep.unitname);
            return PartialView("../DataSource/EditData", new DatasourceRepository().MeetingDetailInfo(confid));
        }

        //Telerik调用的方法
        [GridAction]
        public ActionResult Datasource(int appid)
        {
            APPID = appid;
            APPID2 = appid;
            if (appid != 0)
                Session["nowappid"] = appid;
            else if (Session["nowappid"] != null)
                appid = int.Parse(Session["nowunitid"].ToString());
            DatasourceRepository rep = new DatasourceRepository();
            if (appid != 0)
                return View(new GridModel(rep.GetAllDataByAppId(appid)));
            return View(new GridModel(rep.GetAllData()));
        }

        //添加数据元
        [AcceptVerbs(HttpVerbs.Post)]
        public string AddDataForSave(string Dataid)
        {
            ApplicationDatasource ads = new ApplicationDatasource();
            ads.@operator = "杨佳";
            ads.applicationid = APPID;
            string res = "数据元添加成功";
            DatasourceRepository rep = new DatasourceRepository();
            try
            {
                if (TryUpdateModel(ads))
                {
                    rep.Insert(ads);
                }
            }
            catch
            {
                res = "数据元添加失败";
            }
            ViewData["ActionMessagesForAdd"] = res;
            return res;
        }

        //修改数据元
        [GridAction]
        [HttpPost]
        public ActionResult EditData(Datasource data)
        {
            data.appid = APPID2;
            new DatasourceRepository().GetEditData(data);
            ViewData["ActionMessagesForEdit"] = "修改成功";
            return PartialView("../Shared/ShowActionMessage");

        }

        //删除数据元
        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult DeleteData(int dataid)
        {
            string res = "数据元删除成功";
            DatasourceRepository rep = new DatasourceRepository();
            try
            {
                rep.Delete(dataid);
                new DatasourceRepository().GetAllDatasource(dataid);
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
