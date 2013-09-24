using System;
using System.Collections.Generic;
using System.Linq;
using UserGovern.Repository;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.Extensions;
using System.Threading;
using System.IO;
using System.Text;
using UserGovern.Models;

namespace AuthorityMgt.UserGovern.Controllers
{
    public class RoleHomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "欢迎使用 ASP.NET MVC!";

            return View();
        }


        public ActionResult GetRoleOthers()
        {
          return PartialView();
        }

        AuthUserRepository rep = new AuthUserRepository();

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult GetUser()
        {
            return View(new GridModel(rep.GetAllUserFromAuth()));
        }

        [GridAction]
        public ActionResult RoleUsersAjaxEvent(int? roleid)
        {
            RoleRepository sys = new RoleRepository();
            if (roleid == null)
                return View(new GridModel(rep.GetAllUserFromAuth()));

            IEnumerable<string> userinrole = null;
            if (roleid != null)
            {
                userinrole = sys.GetUserIDListInRole((int)roleid);
                var userlist = sys.GetAllAuthUserWithRole(userinrole.ToList());
                return View(new GridModel()
                {
                    Data = userlist
                });
            }
            return this.Content("Error");
        }

    }
}
