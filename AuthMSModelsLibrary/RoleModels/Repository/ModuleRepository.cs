using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using UserGovern.Models;
using AuthMSModelsLibrary;

namespace UserGovern.Repository
{
    public class ModuleRepository
    {
        public IQueryable<T_Module> GetAllModule()
        {
            AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext();

            return systemdc.T_Module.Where(o => o.IsDelete == false).OrderBy(o => o.ModulePriority);
        }

        public IEnumerable<int> GetModuleIDByRoleID(int roleid)
        {
            using (AuthMSLinqDataContext systemdc = new AuthMSLinqDataContext())
            {
                try
                {
                    return (from rolemodule in systemdc.T_RoleModule
                            where rolemodule.RoleID == roleid
                            select rolemodule.ModuleID).ToList();
                }
                catch (Exception)
                { return null; }
            }
        }

    }
}