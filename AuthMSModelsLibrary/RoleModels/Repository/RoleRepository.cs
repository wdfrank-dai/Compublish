using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UserGovern.Models;
using AuthMSModelsLibrary;

namespace UserGovern.Repository
{
    public class RoleRepository
    {
        AuthMSLinqDataContext db = new AuthMSLinqDataContext();

        public IEnumerable<T_Role> Insert(T_Role ad)
        {
            db.T_Role.InsertOnSubmit(ad);
            db.SubmitChanges();
            return null;
        }

        public IEnumerable<roleusershowmodel> GetAllRole()
        {
            var rep = new AuthMSLinqDataContext().T_Role.ToList();
            var re = from ad in rep
                     select new roleusershowmodel
            {
                RoleID = ad.RoleID,
                RoleParentID = ad.RoleParentID,
                RoleName = ad.RoleName,
                RoleFunctionNotes = ad.RoleFunctionNotes,
                IsChildrenRole = ad.IsChildrenRole,
                IsDelete = ad.IsDelete,
                OperateDate = Convert.ToDateTime(ad.OperateDate).ToString("yyyy-MM-dd HH:mm"),
                OperatorID = ad.OperatorID,
                OperatorName = ad.OperatorName,
            };
            return re;
        }

        public roleusershowmodel EditRole(string confid)
        {
            IList<roleusershowmodel> result1;
            var temp = (from m in new AuthMSLinqDataContext().T_Role
                        where m.RoleID.ToString() == confid
                        select new
                        {
                            RoleID = m.RoleID,
                            RoleParentID = m.RoleParentID,
                            RoleName = m.RoleName,
                            IsChildrenRole = m.IsChildrenRole,
                            RoleFunctionNotes = m.RoleFunctionNotes,
                        }).ToList();
            result1 = (from m in temp
                       select new roleusershowmodel
                       {
                           RoleID = m.RoleID,
                           RoleParentID = m.RoleParentID,
                           RoleName = m.RoleName,
                           IsChildrenRole = m.IsChildrenRole,
                           RoleFunctionNotes = m.RoleFunctionNotes,
                       }
                     ).ToList();

            return result1.FirstOrDefault();
        }


        //修改角色基本信息
        public void MeetingConfInfoEdit(roleusershowmodel roleid)
        {
            T_Role role = db.T_Role.First(c => c.RoleID == roleid.RoleID);

            role.RoleParentID = roleid.RoleParentID;
            role.RoleName = roleid.RoleName;
            role.IsChildrenRole = roleid.IsChildrenRole;
            role.RoleFunctionNotes = roleid.RoleFunctionNotes;

            db.SubmitChanges();
            return;
        }

        //删除角色
        public void Delete(string id)
        {
            var role = db.T_Role.Where(e => e.RoleID.ToString() == id).FirstOrDefault();
            if (role != null)
            {
                db.T_Role.DeleteOnSubmit(role);
                db.SubmitChanges();
                return;
            }
            return;
        }

        //角色分配模块
        public bool InsertRoleModuleByRoleid(IEnumerable<T_RoleModule> roleModulelist)   //如果以前存在 isdelete =true的 就不插入 直接将isdelete=false 而不重新插入 //这里有问题
        {
            AuthMSLinqDataContext sysdc = new AuthMSLinqDataContext();
            try
            {
                IEnumerable<T_RoleModule> listall = (from a in sysdc.T_RoleModule where a.RoleID == roleModulelist.ElementAt(0).RoleID select a).ToList();
                //  IEnumerable<T_RoleModule> list = listall.Where(o => o.IsDelete == false);
                // IEnumerable<T_RoleModule> listdeletetrue = listall.Where(o => o.IsDelete == true);

                //foreach (var item in list)
                //{
                //    item.IsDelete = true;
                //}
                sysdc.T_RoleModule.DeleteAllOnSubmit(listall);

                sysdc.T_RoleModule.InsertAllOnSubmit(roleModulelist);
                sysdc.SubmitChanges();
                return true;
            }
            catch (Exception) { return false; }

        }

        //用户添加角色
        public bool AddUserRole(IEnumerable<T_UserRole> userrole2)
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

        //批量删除用户角色
        public bool DelUserRole(IEnumerable<T_UserRole> userrole)
        {
            AuthMSLinqDataContext sysdc = new AuthMSLinqDataContext();
            int roleid = userrole.ElementAt(0).RoleID;
            IList<string> useridlist = new List<string>();
            foreach (var item in userrole)
                useridlist.Add(item.UserID);

            var UserlistInUseRole = sysdc.T_UserRole.Where(o => o.RoleID == roleid);
            var linquseridlist = UserlistInUseRole.Select(o => o.UserID);
            IList<string> idlist = new List<string>();

            foreach (var item in linquseridlist)
            {
                if (useridlist.Contains(item))
                    idlist.Add(item);
            }
            IList<T_UserRole> userrole2 = new List<T_UserRole>();
            foreach (var item in idlist)
            {
                T_UserRole a = sysdc.T_UserRole.Where(o => o.UserID == item).Where(o => o.RoleID == roleid).FirstOrDefault();
                userrole2.Add(a);
            }
            sysdc.T_UserRole.DeleteAllOnSubmit(userrole2);
            sysdc.SubmitChanges();
            return true;
        }

        //用户添加角色
        public IEnumerable<string> GetUserIDListInRole(int roleid)
        {
            AuthMSLinqDataContext sysdc = new AuthMSLinqDataContext();
            return from userrole in sysdc.T_UserRole
                   where userrole.RoleID == roleid
                   select userrole.UserID;
        }

        public IEnumerable<AuthUser> GetAllAuthUserWithRole(IEnumerable<string> roleid)
        {
            AuthMSLinqDataContext authUser = new AuthMSLinqDataContext();
            return from user in authUser.T_User
                   join dep in authUser.T_Department on user.DepNO equals dep.DepNO into user2
                   from sex in user2.DefaultIfEmpty()
                   join pid in authUser.DIC_PID on user.PID equals pid.pid_code into user3
                   from pid in user3.DefaultIfEmpty()
                   join dep2 in authUser.T_Department on user.DepSectionNO equals dep2.DepNO into user4
                   from dep2 in user4.DefaultIfEmpty()
                   where roleid.Contains(user.UserID) != false
                   select new AuthUser
                   {
                       UserID = user.UserID,
                       UserName = user.UserName,
                       AccountNo = user.AccountNo,
                       UserType = null,
                       CardNo = user.CardNo,
                       PID = pid.pid_name,
                       Email = user.Email,
                       Password = user.Password,
                       QQ = user.QQ,
                       Telephone = user.Telephone,
                       Flag = user.Flag,
                       IDCard = user.IDCard,
                       IDNo = user.IDNo,
                       DepNO = user.DepNO,
                       DepSectionNO = user.DepSectionNO,
                       DepSectionName = dep2.DepName,
                       InRole = roleid.Contains(user.UserID) ? true : false

                   }
                       ;
        }
    }
}