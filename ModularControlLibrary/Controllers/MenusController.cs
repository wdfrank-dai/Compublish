using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModularControl.Models;
using ModularControl.Repository;

namespace ModularControl.Controllers
{
    public class MenusController : Controller
    {
        //SystemID 是 。UserID 是 人员的EmpNO编号.
        public JsonResult GetModuleNodeChildren(string UserID, string SystemID)
        {
            
            //string dNo = Session["nowdNo"].ToString();
            if (UserID == null || SystemID == null)
                return null;
            string dNo = SystemID;
            //UserID = UserID.Substring(1);

            var menus = ModuleRepository.GetMenusByUserID(UserID);//获取当前登录的编号用户的所有模块信息

            //var ms = menus.Where(m => m.IsDelete == false);
            var ms = menus;

            List<MenuNodeModel> nodes = new List<MenuNodeModel>();
            if (ms != null)
            {
                var rs = ms.Where(c => c.ModuleParentID.ToString() == dNo).ToList();//ModuleParentID = 0是指根节点

                foreach (var d in rs)
                {

                    // create a new node
                    MenuNodeModel t = new MenuNodeModel();
                    t.attr = new NodeAttribute();
                    t.attr.id = d.ModuleID.ToString();
                    t.data = new nodedata();
                    t.data.title = d.ModuleName;
                    if (d.RouteName != null && d.RouteName != "")
                    {
                        t.data.icon = Url.Content("~/Scripts/plusIns/jsTree/themes/classic/TreeLeaf1.png");
                        t.data.attr = new data_attr();
                        t.data.attr.href = Url.Content("~/Menus/Dispatch?routeName=") + d.RouteName;
                        t.children = null;
                    }
                    else
                    {
                        t.data.icon = Url.Content("~/Scripts/plusIns/jsTree/themes/classic/TreeMenu1.png");
                        t.children = new List<MenuNodeModel>();
                        t.state = "closed";
                    }
                    nodes.Add(t);
                }

                return Json(nodes);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Dispatch(string routeName)
        {
            //char[] separators = { '.' };
            string[] road = routeName.Split('.');
            string areaName = road[0];
            string controllerName = road[1];
            string actionName = road[2];

            return RedirectToAction(actionName, controllerName, new { area = areaName });

            //return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IFrameContainer(string WebURL, string InformationText)
        {
            string UrlAuthority = Request.Url.GetLeftPart(UriPartial.Authority);
            ViewData["WebURL"] = UrlAuthority + WebURL;
            ViewData["InformationText"] = InformationText;
            return View();
        }
    }
}
