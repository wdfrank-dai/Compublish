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
    public class WebPageController : Controller
    {
        static int str = 0;
        public ActionResult Index()
        {            
            ViewData["pageunitidlist"] = new SelectList(new WebPageRepository().pageunitidDownList(), "id", "unitname");
            return View();
        }

        [GridAction]
        public ActionResult SelectWebPageSet()
        {
            return View(new GridModel(new WebPageRepository().WebPageSetAll()));
        }

        
        [GridAction]
        public ActionResult InsertWebPageSet()
        {
            WebPageSetModel eventss = new WebPageSetModel();

            if (TryUpdateModel(eventss))
            {
                new WebPageRepository().WebPageSetInsert(eventss);

            }

            return View(new GridModel(new WebPageRepository().WebPageSetAll()));
        }
        
        [GridAction]
        public ActionResult SaveWebPageSet(int id)
        {
            WebPageSetModel eventss = new WebPageSetModel
            {
                pageno = id
            };

            if (TryUpdateModel(eventss))
            {
                new WebPageRepository().WebPageSetUpdate(eventss);


            }

            return View(new GridModel(new WebPageRepository().WebPageSetAll()));
        }
        
        [GridAction]
        public ActionResult DeleteWebPageSet(int id)
        {

            new WebPageRepository().WebPageSetDelete(id);
            return View(new GridModel(new WebPageRepository().WebPageSetAll()));
        }

        //---------------------------------------

        [GridAction]
        public ActionResult SelectWebPageUnits(string name)
        {
            var sess = new WebPageRepository().GetWebPageSetByStr(name);
            if (sess == null)
            {
                str = 0;
                return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
            }
            str = sess.pageno;
            return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult InsertWebPageUnits()
        {
            WebPageUnitsModel eventss = new WebPageUnitsModel();
            if (str == 0)
            {
                return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
            }
            if (TryUpdateModel(eventss))
            {
                new WebPageRepository().WebPageUnitsInsert(eventss, str);

            }

            return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
        }

        [GridAction]
        public ActionResult SaveWebPageUnits(int id)
        {
            if (str == 0)
            {
                return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
            }
            WebPageUnitsModel eventss = new WebPageUnitsModel
            {
                id = id,
            };

            if (TryUpdateModel(eventss))
            {
                new WebPageRepository().WebPageUnitsUpdate(eventss);


            }

            return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
        }

        [GridAction]
        public ActionResult DeleteWebPageUnits(int id)
        {
            if (str == 0)
            {
                return View(new GridModel(new WebPageRepository().WebPageUnitsAll(0)));
            }
            new WebPageRepository().WebPageSetDelete(id);
            return View(new GridModel(new WebPageRepository().WebPageUnitsAll(str)));
        }

    }
}
