using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModularControl.Controllers
{
    public class ModelTreeController : Controller
    {

        public ActionResult Index(string UserID, int SystemID)
        {
            ViewBag.UserID = UserID;
            ViewBag.SystemID = SystemID;
           
            //return this.PartialView("/ModularControl/Views/ModelTree/Index.aspx");
            return PartialView("ModelTreeView");
        }

    }
}
