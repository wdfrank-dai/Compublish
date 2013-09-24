using System;
using System.IO;
using System.Web;
using System.Data;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using System.Data.Linq;
using System.Threading;
using UserGovern.Models;
using System.Web.SessionState;
using System.Collections;
using UserGovern.Repository;
using System.Data.SqlClient;
using System.Collections.Generic;
using Telerik.Web.Mvc.Extensions;
using System.Web.Script.Serialization;
using AuthMSModelsLibrary;

namespace AuthorityMgt.UserGovern.Controllers
{
    public class TreeController : Controller
    {
        //
        // GET: /Tree/
        AuthMSLinqDataContext db = new AuthMSLinqDataContext();

        public ActionResult RoleTree()
        {
            return PartialView();
        }

        public ActionResult ModuleTree()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetRoleTreeDataAll()//没用到地点值，显示全部人员，只是保留
        {
            string dbid = "0";
            var d = db.T_Role.Where(c => c.RoleParentID.ToString() == dbid).ToList();
            List<RoleTreeModel> nodes = new List<RoleTreeModel>();
            foreach (var a in d)
            {
                RoleTreeModel rootNode = new RoleTreeModel();
                rootNode.attr = new JsTreeAttribute();
                rootNode.data = a.RoleName;
                rootNode.attr.id = a.RoleID;
                rootNode.state = "open"; //根节点设定为初始打开状态
                new TreeRepository().GetRoleTree(a.RoleID.ToString(), rootNode);
                nodes.Add(rootNode);
            }
            return Json(nodes);
        }

        public JsonResult GetRoleNodeChildren(int SystemID)
        {
            //if (SystemID == 0)
            //    return null;
            int dNo = SystemID;
            TreeRepository re = new TreeRepository();
            var menus = re.GetAllRole();
            var ms = menus;
            List<ModuleTreeModel> nodes = new List<ModuleTreeModel>();
            if (ms != null)
            {
                var rs = ms.Where(c => c.RoleParentID == dNo).ToList();

                foreach (var d in rs)
                {

                    // create a new node
                    ModuleTreeModel t = new ModuleTreeModel();
                    t.attr = new JsTreeAttribute1();
                    t.attr.id = d.RoleID;
                    t.data = new nodedata();
                    t.data.title = d.RoleName;
                    if (d.IsChildrenRole != false)
                    {
                        // t.data.icon = Url.Content("~/Scripts/plusIns/jsTree/themes/classic/TreeLeaf1.png");
                        t.data.attr = new data_attr();
                        t.data.attr.href = Url.Content("~/Menus/Dispatch?routeName=") + d.IsChildrenRole;
                        t.children = null;
                    }
                    else
                    {
                        // t.data.icon = Url.Content("~/Scripts/plusIns/jsTree/themes/classic/TreeMenu1.png");
                        t.children = new List<ModuleTreeModel>();
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

        //模块树
        public JsonResult GetModuleNodeChildren(int SystemID)
        {
            int dNo = 0;
            if (SystemID != 0)
                dNo = SystemID;
            TreeRepository re = new TreeRepository();
            var menus = re.GetAllModule();
            var ms = menus;
            List<ModuleTreeModel> nodes = new List<ModuleTreeModel>();
            if (ms != null)
            {
                var rs = ms.Where(c => c.ModuleParentID == dNo).ToList();
                foreach (var d in rs)
                {
                    ModuleTreeModel t = new ModuleTreeModel();
                    t.attr = new JsTreeAttribute1();
                    t.attr.id = d.ModuleID;
                    t.data = new nodedata();
                    t.data.title = d.ModuleName;
                    if (d.RouteName != null && d.RouteName != "")
                    {
                        // t.data.icon = Url.Content("~/Scripts/plusIns/jsTree/themes/classic/TreeLeaf1.png");
                        t.data.attr = new data_attr();
                        t.data.attr.href = Url.Content("~/Menus/Dispatch?routeName=") + d.RouteName;
                        t.children = null;
                    }
                    else
                    {
                        // t.data.icon = Url.Content("~/Scripts/plusIns/jsTree/themes/classic/TreeMenu1.png");
                        t.children = new List<ModuleTreeModel>();
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

        [GridAction]
        public ActionResult DisplayCheckedRole(string[] checkedRecords)
        {
            try
            {
                Session["PersonTreeIDCrew"] = checkedRecords;
                Session["PersonTreeIDCrew1"] = checkedRecords[0];
                return PartialView("../Role/GetRoleOthers", TreeRepository.RoleTreeAll(checkedRecords));
            }
            catch {
                return null;
            }
        }

        [GridAction]
        public ActionResult AjaxLoadingUserSequence(int? roleid)
        {
            roleid = int.Parse(Session["PersonTreeIDCrew1"].ToString());
            TreeRepository rep = new TreeRepository();
            RoleRepository sys = new RoleRepository();
            IEnumerable<string> userinrole = null;
            if (roleid != null)
            {
                userinrole = sys.GetUserIDListInRole((int)roleid);
                var userlist = rep.UserTreeAll(userinrole.ToList());
                return View(new GridModel()
                {
                    Data = userlist
                });
            }
            return this.Content("Error");
        }

    }
}
