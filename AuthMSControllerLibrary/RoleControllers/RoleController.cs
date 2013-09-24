using System.Web.Mvc;
using System.Web.Script.Serialization;
using UserGovern.Models;
using UserGovern.Repository;
using System.Text;
using System.Data.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Telerik.Web.Mvc;
using AuthMSModelsLibrary;

namespace AuthorityMgt.UserGovern.Controllers
{
    public class RoleController : Controller
    {
        //
        // GET: /Role/
        AuthMSLinqDataContext db = new AuthMSLinqDataContext();

        public ActionResult GetRole()
        {
            return View();
        }

        public ActionResult GetRoleOthers()
        {
            return PartialView();
        }

        public ActionResult AddRoleForRoleParentID(string confid)
        {
            roleusershowmodel role = new roleusershowmodel();
            int result = -1;
            if(int.TryParse(confid, out result))
            {
                role.RoleParentID = result;
                return PartialView("AddRole", role);
            }
            return PartialView("AddRole");
        }

        [GridAction]
        public ActionResult RoleUsersAjaxEvent(int? roleid)
        {
            AuthUserRepository rep = new AuthUserRepository();
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

        public ActionResult RoleModule()
        {
            return View();
        }

        public ActionResult UserRole()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _InsertAjaxEditing()
        {
            T_Role ad = new T_Role();
            RoleRepository rep = new RoleRepository();
            if (TryUpdateModel(ad))
            {
                rep.Insert(ad);
            }
            return PartialView("AddRole",new GridModel(rep.GetAllRole()));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string AddRole()
        {
            T_Role ads = new T_Role();
            Initialize(ads);
            string res = "角色添加成功";
            RoleRepository rep = new RoleRepository();
            try
            {
                if (TryUpdateModel(ads))
                {
                    int roleid = db.T_Role.OrderByDescending(o=>o.RoleID).FirstOrDefault().RoleID;//倒叙查找最大ID
                    ads.RoleID = roleid + 1;
                    rep.Insert(ads);
                }
            }
            catch
            {
                res = "角色添加失败";
            }
            ViewData["ActionMessagesForAdd"] = res;
            return res;
        }

        public void Initialize(T_Role ads)
        { 
              ads.OperatorID = 1;
              ads.OperateDate = DateTime.Now.Date;
              ads.OperatorName = "007";
        }

        public ActionResult EditRole(string confid)
        {
            RoleRepository res = new RoleRepository();
            return View(new RoleRepository().EditRole(confid));
        }

        //修改角色信息
        [AcceptVerbs(HttpVerbs.Post)]
        public string ConfInfoEditPost(int roleid)
        {
            var res = "修改成功";
            try
            {
                bool ischildren = true;
                var parentid = Request.Form["RoleParentID"];
                var rolename = Request.Form["RoleName"];
                var functionnote = Request.Form["RoleFunctionNotes"];
                if (Request.Form["IsChildrenRole"] == "flase")
                    ischildren = false;
                else
                    ischildren = true;

                //var rs = db.T_Role.FirstOrDefault(e => e.RoleID == roleid);
                T_Role role = db.T_Role.First(c => c.RoleID == roleid);

                role.RoleParentID = int.Parse(parentid);
                role.RoleName = rolename;
                role.IsChildrenRole = ischildren;
                role.RoleFunctionNotes = functionnote;

                db.SubmitChanges();
            }
            catch
            {
                res = "修改失败！";
            }
            ViewData["ActionMessagesForEdit"] = res;
            return res;

        }

        //删除角色
        //[AcceptVerbs(HttpVerbs.Post)]
        //[GridAction]
        public ActionResult DeleteRole(string roleid)
        {
            string res = "角色删除成功";
            RoleRepository rep = new RoleRepository();
            try
            {
                rep.Delete(roleid);
                new AuthUserRepository().GetAllUserRoleInfoFromAuth(roleid);
            }
            catch
            {
                res = "角色删除失败";
            }
            ViewData["ActionMessagesForDel"] = res;
            return PartialView("../Shared/ShowActionMessage");
        }

        //角色分配模块
        [GridAction]
        public string RoleAssignModule(string Roleid, IEnumerable<int> ModuleTree_checkedNodes)
        {
            // string node1 = RoleTree_checkedNodes[0].Value;
            // string node2 = ModuleTree_checkedNodes[0].Value;
            int roleid = 0;
            List<int> moduleidlist = new List<int>();
            IEnumerable<T_RoleModule> rolemodulelist = null;
            RoleRepository sysdc = new RoleRepository();
            if (Roleid != null && ModuleTree_checkedNodes != null)
            {
                try
                {

                    if (Int32.TryParse(Roleid, out roleid))
                    {
                        //foreach (var item in ModuleTree_checkedNodes)
                        //{
                        //    moduleidlist.Add(Convert.ToInt32(item.Value));
                        //}
                        if ((rolemodulelist = getroleModule(roleid, ModuleTree_checkedNodes)) != null)   //得到  要插入的T_rolemodule modle
                        {
                            if (sysdc.InsertRoleModuleByRoleid(rolemodulelist))
                            {
                                //  TempData["trueorfalse"] = "操作成功";
                                //  return RedirectToAction("AssignModule");
                                return "操作成功";
                            }
                            else
                            {
                                //TempData["trueorfalse"] = "操作失败";
                                //return RedirectToAction("AssignModule");
                                return "操作失败";
                            }
                        }
                    }
                    else //return RedirectToAction("AssignModule");
                        return "操作失败";
                }
                catch (Exception e) { TempData["trueorfalse"] = "操作失败"; } return ""; //return RedirectToAction("AssignModule"); }
            }
            return "操作失败";
        }

        private IEnumerable<T_RoleModule> getroleModule(int roleid, IEnumerable<int> Moduleid)
        {
            IList<T_RoleModule> rolimodule = new List<T_RoleModule>();

            try
            {
                foreach (int item in Moduleid)
                {
                    T_RoleModule a = new T_RoleModule();
                    a.ModuleID = item;

                    a.RoleID = roleid;
                    a.RightType = "rw";
                    a.OperateDate = System.DateTime.Now.Date;
                    a.OperatorName = "007";
                    a.OperatorID = 1237;
                    rolimodule.Add(a);

                }
                return rolimodule;
            }
            catch { return null; }
        }

        public JsonResult GetModuleIDByRoleID(string roleid)  //返回模块的分布图
        {
            ModuleRepository sysdc = new ModuleRepository();

            var allModule = ModuleLinqtoCum(sysdc.GetAllModule() as IEnumerable<T_Module>);


            IEnumerable<string> ModuleIdList = null;

            int a = 0;
            if (Int32.TryParse(roleid, out a))
            {
                ModuleIdList = IntTostr(sysdc.GetModuleIDByRoleID(a));
            }

            return Json(ModuleIdList);

        }

        private IEnumerable<Module> ModuleLinqtoCum(IEnumerable<T_Module> modulelist)
        {
            IList<Module> list = new List<Module>();
            foreach (var module in modulelist)
            {

                list.Add(ModuleLinqtoCum(module));

            }
            return list;

        }

        private Module ModuleLinqtoCum(T_Module model)
        {
            Module module = new Module();
            module.ModuleID = model.ModuleID.ToString();
            module.ModuleParentID = model.ModuleParentID.ToString();
            module.ModuleName = model.ModuleName;
            module.ModuleHierarchy = model.ModuleHierarchy;
            module.ModulePriority = model.ModulePriority.ToString();
            return module;

        }

        private IEnumerable<string> IntTostr(IEnumerable<int> list)
        {
            IList<string> a = new List<string>();
            foreach (int item in list)
                a.Add(item.ToString());
            return a as IEnumerable<string>;
        }

        //用户添加角色
        public JsonResult BatchAddRoleUser(int roleid, string[] check)
        {
            RoleRepository sys = new RoleRepository();
            var a = _buildUserRole(roleid, check);
            if (a == null)
                return Json("操作失败");
            else
            {
                if (sys.AddUserRole(a))
                {
                    return Json("操作成功");
                }
                else return Json("操作失败");
            }

        }

        private IEnumerable<T_UserRole> _buildUserRole(int? roleid, string[] check)
        {
            if (roleid == null || check == null)
                return null;
            else
            {
                int role = (int)roleid;
                IList<T_UserRole> userrole = new List<T_UserRole>();

                foreach (var itme in check)
                {
                    T_UserRole a = new T_UserRole();
                   // a.IsRole = true;
                    a.UserID = itme;
                    a.RoleID = role;
                    a.OperateDate = System.DateTime.Now;
                    a.OperatorID = 313;
                    a.OperatorName = "杨蕴坚";
                    userrole.Add(a);
                }
                return userrole;

            }
        }

        public JsonResult DelUserRole(int? roleid, string[] check)
        {
            RoleRepository sys = new RoleRepository();
            var a = _buildUserRole(roleid, check);
            if (a == null)

                return Json("操作失败");
            else
            {

                if (sys.DelUserRole(a))
                {
                    return Json("操作成功");
                }
                else return Json("操作失败");
            }
        }
    }
}
