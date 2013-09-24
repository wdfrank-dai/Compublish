using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UserGovern.Models;
using AuthMSModelsLibrary;

namespace UserGovern.Repository
{
    public class AuthUserRepository : IAuthUserRepository
    {
        AuthMSLinqDataContext db = new AuthMSLinqDataContext();

        public void Dispose()
        { 
        }

        public IEnumerable<AuthUser> GetAllUserFromAuth()
        {
            try
            {                   
                var users = (from user in db.T_User
                             select new AuthUser
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
            catch
            {
                return null;
            }
        }

        public IEnumerable<roleusershowmodel> GetAllUserRoleInfoFromAuth(string roleid)
        {
            var result = (from user in db.T_Role
                          select new roleusershowmodel
                          {
                              RoleID = user.RoleID,
                              RoleName = user.RoleName,
                              RoleParentID = user.RoleParentID,
                              IsChildrenRole = user.IsChildrenRole,
                              RoleFunctionNotes = user.RoleFunctionNotes,
                              OperatorName = user.OperatorName,
                          });
            return result;
        }

        public IEnumerable<AuthUser> GetAllUserFromAuthByDepNO(string depno)
        {
            var result = (from user in db.T_User select new AuthUser{
                 UserID = user.UserID,
                 UserName = user.UserName,
                 UserType = (from utype in db.DIC_PID where utype.pid_code == user.PID select utype.pid_name).FirstOrDefault(),
                 Email = user.Email,
                 Telephone = user.Telephone,
                 QQ = user.QQ,
                 depName = (from dep in db.T_Department where dep.DepNO == depno select dep.DepName).FirstOrDefault()
                }).Where(o => o.DepNO == depno).ToList();
            return result;
        }

        public IEnumerable<AuthUser> GetAllUserFromAuthByType(string typeid)
        {
            var result = (from user in db.T_User
                          join type in db.DIC_PID on user.PID equals type.pid_code
                          select new AuthUser
                          {
                              PID = type.pid_name,
                          });
            return result;
        }

        public IEnumerable<AuthUser> GetAllUserFromAuthByRole(int roleid)
        { 
            var result = (from user in db.T_UserRole
                          join role in db.T_Role on user.RoleID equals role.RoleID into user1
                          from role in user1.DefaultIfEmpty()
                          join username in db.T_User on user.OperatorName equals username.UserName
                          select new AuthUser
                          {
                              UserID = role.RoleName,
                              UserName = username.UserName,                          
                          });
            return result;
        }
    }
}