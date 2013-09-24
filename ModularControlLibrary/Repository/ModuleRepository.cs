using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModularControl.Models;

namespace ModularControl.Repository
{
    public class ModuleRepository
    {
        public static IEnumerable<Module> GetMenusByUserID(string UserID)
        {
            //UserDataContext mdc = new UserDataContext();
            //var ar = new AccountRepository();
            
            //var modules = mdc.T_Module.Where(m => m.IsMenu == true&&m.IsDelete==false);
            var modules = GetModulesByUserID(UserID).Where(m => m.IsMenu == true);
            //var ms=modules.OrderBy(m => m.ModulePriority).ToList();
            return modules.OrderBy(m => m.ModulePriority);
        }

        //当前登录用户的所有角色的模块List
        public static IEnumerable<Module> GetModulesByUserID(string UserID)
        {
            try
            {
                UserDataContext udc = new UserDataContext();
                var roles = GetRolesByUserID(UserID);
                var moduleList = (from r in roles
                                  join rm in udc.RoleModule
                                         on r.RoleID equals rm.RoleID
                                  join m in udc.Module
                                         on rm.ModuleID equals m.ModuleID
                                  select m).Distinct();
                return moduleList;
            }
            catch (Exception)
            {

                return null;
            }
        }

        //读取当前登录人员的全部角色类型
        public static IEnumerable<Role> GetRolesByUserID(string UserID)
        {
            try
            {
                UserDataContext udc = new UserDataContext();
                var roleList = (from ur in udc.UserRole
                                where ur.UserID == UserID
                                join r in udc.Role
                                       on ur.RoleID equals r.RoleID
                                select r).ToList();   //当前登录人员的全部角色
                var defaultroles = (from m in udc.Role where m.RoleFunctionNotes == PersMSEnv.DefaultRole select m).ToList();//DefaultRole是默认角色，所有默认的角色
                for (int i = 0; i < defaultroles.Count(); i++)//将role表中的默认角色添加到当前登录用户的角色List中。
                    if (!roleList.Exists(r => defaultroles[i] == r))
                        roleList.Add(defaultroles[i]);
                return roleList;

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}