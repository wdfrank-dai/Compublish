using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComPublishWeb.Models;

namespace ComPublishWeb.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChannelSet()
        {
            List<ChannelModels> sre = new List<ChannelModels> { 
                    new ChannelModels {ChannelID = 1,ChannelName="部门通告"},
                    new ChannelModels {ChannelID = 2,ChannelName="教学信息"},
                    new ChannelModels {ChannelID = 3,ChannelName="事务信息"},
                    new ChannelModels {ChannelID = 4,ChannelName="科研信息"},
                    new ChannelModels {ChannelID = 4,ChannelName="图书资源信息"},
                    new ChannelModels {ChannelID = 5,ChannelName="书刊查询"},                    
            };

            ViewData["Channels"] = new SelectList(sre, "ChannelID", "ChannelName");
            return View();
        }

        public ActionResult ContentSet()
        {
            List<ChannelModels> sre = new List<ChannelModels> { 
                    new ChannelModels {ChannelID = 1,ChannelName="部门通告"},
                    new ChannelModels {ChannelID = 2,ChannelName="教学信息"},
                    new ChannelModels {ChannelID = 3,ChannelName="事务信息"},
                    new ChannelModels {ChannelID = 4,ChannelName="科研信息"},
                    new ChannelModels {ChannelID = 4,ChannelName="图书资源信息"},
                    new ChannelModels {ChannelID = 5,ChannelName="书刊查询"},                    
            };

            ViewData["Channels"] = new SelectList(sre, "ChannelID", "ChannelName");
            return View();
        }



        public ActionResult AdminIndex()
        {
            //ViewBag.UserID = "0309024";
            //ViewBag.SystemID = 0;
            string UserID = HttpContext.User.Identity.Name.Split(',')[0];
            string SystemID = "677";
            return View(new ModularControl.Models.AttendMenus().GetAttendMenu(UserID, SystemID));
        }

    }
}
