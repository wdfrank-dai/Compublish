using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthUserDataLibrary.Repository;
using AuthUserDataLibrary.Models;
using AuthMSModelsLibrary;
using Telerik.Web.Mvc;
using System.Threading;
using System.IO;
using System.Text;
using AuthUser2.Models;


namespace AuthorityMgt.AuthUser2.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            string userid = "0001";
            string password = "000000";

            var rep = new AuthUserRepository();
            return View(new GridModel(rep.GetUserFromAuthByUserIDandPassword(userid, password)));
        }

        public ActionResult Index2()
        {

            return View();
        }

        public ActionResult EditUser(string UserID)
        {
            AuthUserRepository rep = new AuthUserRepository();
            if (UserID != null)
            {
                AuthUserEditModel c = new AuthUserEditModel();
                c.OldUserID = UserID;
                c.NewUserID = UserID;
                var ui = rep.GetAuthUserByID(UserID);
                c.QQ = ui.QQ;
                c.Telephone = ui.Telephone;
                c.PID = ui.PID;
                c.Email = ui.Email;

                var sl = rep.GetPid();
                SelectList sele = new SelectList(sl, "Value", "Text", ui.PID);
                ViewData["PID"] = sele;
                ViewBag.pid = sele;
                return PartialView(c);
               
            }

            else return PartialView();
        }//编辑窗口显示
  
        [HttpPost]
        public string EditSave(FormCollection form)//编辑用户之后保存
        {
            string olduserid = form.Get("OldUserID");
            string newuserid = form.Get("NewUserID");
            AuthUserRepository rep = new AuthUserRepository();
            AuthUserEditModel user = new AuthUserEditModel();
            TryUpdateModel(user);
            if (user.QQ == null) user.QQ = "";
            if (user.Telephone == null) user.Telephone = "";
            if (user.Email == null) user.Email = "";



            var flag = rep.UpdateAuthUser(olduserid, user);
            if (flag)
                return "操作成功";
            else return "操作失败";
        }

        public JsonResult BatchAddRoleUser(int? roleid, string[] check)//批量添加用户角色
        {
            AuthUserRepository rep = new AuthUserRepository();
            var a = _buildUserRole(roleid, check);
            if (a == null)
                return Json("操作失败");
            else
            {

                if (rep.AddUserRole(a))
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
                    a.UserID = itme;
                    a.RoleID = role;
                    a.OperateDate = System.DateTime.Now;
                    a.OperatorID = 0;
                    a.OperatorName = itme;
                    userrole.Add(a);
                }
                return userrole;

            }
        }

        [HttpPost]
        public ActionResult GetRoleTreeDataAll(int did)//没用到地点值，显示全部人员，只是保留
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            string dbid = "1";
            var d = db.T_Role.FirstOrDefault(c => c.RoleID.ToString() == dbid);
            if (d != null)
            {
                RoleTreeModel rootNode = new RoleTreeModel();
                rootNode.attr = new JsTreeAttribute();
                rootNode.data = d.RoleName;
                rootNode.attr.id = d.RoleID.ToString();
                rootNode.state = "open"; //根节点设定为初始打开状态
                new AuthUserRepository().GetRoleTree(dbid, rootNode);
                return Json(rootNode);
            }
            else
            {
                return null;
            }
        }

      

        public string PawChange(string[] checkedRecords, string paw)
        {
            AuthUserRepository rep = new AuthUserRepository();
            if (rep.RestPassword(checkedRecords, paw))
                return "操作成功";
            else return "操作失败";
        }

        [GridAction]
        public ActionResult GetAllUser()//获得所有用户
        {
            AuthUserRepository rep = new AuthUserRepository();
            return View(new GridModel(rep.GetAllUserFromAuth()));
        }

        [GridAction]
        public ActionResult GetUserFromAuthByUserIDandPassword()
        {
            string userid = "0001";
            string password = "000000";

            var rep = new AuthUserRepository();
            return Json(rep.GetUserFromAuthByUserIDandPassword(userid, password));
        }

        public ActionResult UserEditWindow(string uid)
        {
            return PartialView();
        }

       }
}
