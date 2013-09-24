using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using AuthUserDataLibrary.Repository;
using AuthUserDataLibrary.Models;
using AuthMSModelsLibrary;
using AuthUser2.Models;
using System.Web.Mvc;

namespace AuthUserDataLibrary.Repository
{
    public class AuthUserRepository : IAuthUserRepository
    {

        public IEnumerable<AuthUserM> GetAllUserFromAuth()//显示所有用户信息
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var users = (from user in db.T_User
                         select new AuthUserM
                         {
                             UserID = user.UserID,
                             UserName = user.UserName,
                             depName = (from dep in db.T_Department where dep.DepNO == user.DepNO select dep.DepName).FirstOrDefault(),
                             UserType = (from utype in db.DIC_PID where utype.pid_code == user.PID select utype.pid_name).FirstOrDefault(),
                             Email = user.Email,
                             Telephone = user.Telephone,
                             QQ = user.QQ,
                         });
            return users;
        }

        public void GetRoleTree(string dhid, RoleTreeModel node)//生成角色树
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            if (node.children == null)
            {
                node.children = new List<RoleTreeModel>();
            }
            var dp = db.T_Role.Where(c => c.RoleID.ToString() == dhid).FirstOrDefault();
            var hid = dp.RoleID;
            foreach (var p in db.T_Role.Where(e => e.RoleID != hid).OrderBy(e => e.RoleName).ToList())
            {
                RoleTreeModel t = new RoleTreeModel();
                t.attr = new JsTreeAttribute();
                t.attr.id = p.RoleID.ToString();
                t.attr.ifPerson = "Y"; //表明是人员节点
                t.data = p.RoleName;
                t.state = "close";
                t.children = null;
                node.children.Add(t);
            }
        }

        public bool AddUserRole(IEnumerable<T_UserRole> userrole2)//添加用户角色
        {
            AuthMSLinqDataContext sysdc = new AuthMSLinqDataContext();
            int roleid = userrole2.ElementAt(0).RoleID;
            IList<T_UserRole> userrole = userrole2 as List<T_UserRole>;
            IList<T_UserRole> toadd = new List<T_UserRole>();
            var UserIDlistInUseRole = sysdc.T_UserRole.Where(o => o.RoleID == roleid).Select(o => o.UserID);
            foreach (var item in userrole)
            {
                if (UserIDlistInUseRole.Contains(item.UserID) == false)
                {
                    toadd.Add(item);
                }

            }
            userrole2 = userrole2 as IEnumerable<T_UserRole>;
            sysdc.T_UserRole.InsertAllOnSubmit(toadd);
            sysdc.SubmitChanges();
            return true;
        }

        public IEnumerable<AuthUserM> GetUserFromAuthByUserIDandPassword(string userid, string password)
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var rs = (from m in db.T_User.Where(u => u.UserID == userid && u.Password == password)
                      select new AuthUserM
                      {
                          UserID = m.UserID,
                          UserName = m.UserName,
                          Email = m.Email,
                          Telephone = m.Telephone,
                          QQ = m.QQ,
                      });
            return rs;
        }

        public string UpdateUserInfo(AuthUserEditModel userinfo)
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var res = db.T_User.Where(u => u.UserID == userinfo.OldUserID).FirstOrDefault();
            if (userinfo.OldUserID != userinfo.NewUserID)
                res.UserID = userinfo.NewUserID;
            res.PID = userinfo.PID;
            res.QQ = userinfo.QQ;
            res.Email = userinfo.Email;
            res.Telephone = userinfo.Telephone;
            db.SubmitChanges();
            return "UpdateSuccess!";
        }

        public string ChangeAuthUserPassword(string userid, string newpassword)
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var res = db.T_User.Where(u => u.UserID == userid).FirstOrDefault();
            res.Password = newpassword;
            db.SubmitChanges();
            return "PassWord Change Success!";
        }

        public bool RestPassword(string[] userid, string newpassword)//批量修改用户密码
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            if (userid == null)
                return false;
            IEnumerable<string> useridlist = userid;
            var userlist = from user in db.T_User
                           where useridlist.Contains(user.UserID)
                           select user;

            foreach (var item in userlist)
                item.Password = newpassword;
            db.SubmitChanges();
            return true;

        }

        public IEnumerable<showroleInfo> PersonTreeAll2(int roleID)//角色信息显示
        {   
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();

            var u = db.T_UserRole.Where(ur => ur.RoleID == roleID).FirstOrDefault();
            var u2 = db.T_User.Where(user => user.UserID == u.UserID).FirstOrDefault();

            var result =
           (from rs_event in db.T_User.Where(user2 => user2.UserID == u2.UserID)
            select new showroleInfo
            {
                UserID = rs_event.UserID,
                RoleName = (from ro in db.T_Role where ro.RoleID == roleID select ro.RoleName).FirstOrDefault(),
                UserName = rs_event.UserName,
                CardNo = rs_event.CardNo.ToString(),
                depName = (from de in db.T_Department where de.DepNO == u2.DepNO select de.DepName).FirstOrDefault()
            });
            return result;
        }

        public AuthUserEditModel GetAuthUserByID(string userID)
        {
            IList<AuthUserEditModel> result1;
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var res = (from user in db.T_User
                       where user.UserID == userID
                       select new
                       {
                           OldUserID = userID,
                           NewUserID = userID,
                           PID = user.PID,
                           QQ = user.QQ,
                           Email = user.Email,
                           Telephone = user.Telephone
                       }).ToList();
            result1 = (from u in res
                       select new AuthUserEditModel
                       {
                           OldUserID = u.OldUserID,
                           NewUserID = u.NewUserID,
                           PID = u.PID,
                           QQ = u.QQ,
                           Telephone = u.Telephone,
                           Email = u.Email
                       }).ToList();
            return result1.FirstOrDefault();
        }

        public IEnumerable<SelectListItem> GetPid()//获得角色ID
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();
            var a = (from deppp in db.DIC_PID
                     select new SelectListItem
                     {
                         Value = deppp.pid_code,
                         Text = deppp.pid_name

                     }).ToList();
            IList<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem() { Value = null, Text = "" });
            foreach (var item in a)
            {
                result.Add(item);
            }

            return result;

        }

        public bool UpdateAuthUser(string userid, AuthUserEditModel ued)//编辑 更新用户信息
        {
            AuthMSLinqDataContext db = new AuthMSLinqDataContext();

            var user = db.T_User.FirstOrDefault(u => u.UserID == userid);
            if (user != null)
            {
                if (user.UserID != ued.NewUserID)
                {
                    T_User newuser = new T_User();
                    newuser.UserID = ued.NewUserID;
                    newuser.PID = ued.PID;
                    newuser.QQ = ued.QQ;
                    newuser.Email = ued.Email;
                    newuser.Telephone = ued.Telephone;
                    newuser.AccountNo = user.AccountNo;
                    newuser.CardNo = user.CardNo;
                    newuser.DepNO = user.DepNO;
                    newuser.DepSectionNO = user.DepSectionNO;
                    newuser.Flag = user.Flag;
                    newuser.IDCard = user.IDCard;
                    newuser.IDNo = user.IDNo;
                    newuser.Password = user.Password;
                    newuser.SexNo = user.SexNo;
                    newuser.UserName = user.UserName;
                    db.T_User.DeleteOnSubmit(user);
                    db.T_User.InsertOnSubmit(newuser);
                    db.SubmitChanges();
                }
                else
                {
                    user.PID = ued.PID;
                    user.QQ = ued.QQ;
                    user.Email = ued.Email;
                    user.Telephone = ued.Telephone;
                    db.SubmitChanges();
                }
            }
            else
                return false;
            return true;

        }

        public void Dispose(){}


    }
}
