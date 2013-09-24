using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreLibary.Models.Account;

namespace CoreLibary.Models.Repository
{
	public partial class AccountRepository
	{
		//public T_User GetUserByUserID(string UserID)
		//{
		//    UserDataContext pdc = new UserDataContext();
		//    try
		//    {
		//        var user = pdc.T_User.FirstOrDefault(p => p.UserID == UserID);
		//        return user;
		//    }
		//    catch (Exception)
		//    {
		//        return null;
		//    }
		//}
		public int GetEmpIDByUserID(string UserID)
		{
			try
			{
				UserDataContext pdc = new UserDataContext();
				var empid =pdc.T_EmpInfo.FirstOrDefault(p => p.EmpNO == UserID).EmpID;
				return empid;
			}
			catch (Exception)
			{
				return 0;
			}


		}

        public IEnumerable<int> GetRoleIDsByUserID(string UserID)
        {
            try
            {
                UserDataContext udc = new UserDataContext();
                //var posts = GetPostsByUserID(UserID);
                //var roleList = (from p in posts
                //                join pr in udc.T_PostRole
                //                        on p.PostID equals pr.PostID
                //                join r in udc.T_Role
                //                        on pr.RoleID equals r.RoleID
                //                select r.RoleName).Distinct();
                var roleIDList = (from ur in udc.T_UserRole
                                  where ur.UserID == UserID
                                  join r in udc.T_Role
                                         on ur.RoleID equals r.RoleID
                                  select r.RoleID).ToList();
                var defaultroleid = (from m in udc.T_Role where m.RoleFunctionNotes == PersMSEnv.DefaultRole select m.RoleID).ToList();
                for (int i = 0; i < defaultroleid.Count(); i++)
                    if (!roleIDList.Exists(r => defaultroleid[i] == r))
                        roleIDList.Add(defaultroleid[i]);
                return roleIDList;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<string> GetRoleNamesByUserID(string UserID)
        {
            try
            {
                UserDataContext udc = new UserDataContext();
                //var posts = GetPostsByUserID(UserID);
                //var roleList = (from p in posts
                //                join pr in udc.T_PostRole
                //                        on p.PostID equals pr.PostID
                //                join r in udc.T_Role
                //                        on pr.RoleID equals r.RoleID
                //                select r.RoleName).Distinct();
                var rolenameList = (from ur in udc.T_UserRole
                                    where ur.UserID == UserID
                                    join r in udc.T_Role
                                           on ur.RoleID equals r.RoleID
                                    select r.RoleName).ToList();
                var defaultrolenames = (from m in udc.T_Role where m.RoleFunctionNotes == PersMSEnv.DefaultRole select m.RoleName).ToList();
                for (int i = 0; i < defaultrolenames.Count(); i++)
                    if (!rolenameList.Exists(r => defaultrolenames[i] == r))
                        rolenameList.Add(defaultrolenames[i]);
                return rolenameList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<T_Role> GetRolesByUserID(string UserID)
        {
            try
            {
                UserDataContext udc = new UserDataContext();
                //var posts = GetPostsByUserID(UserID);
                //var roleList = (from p in posts
                //                join pr in udc.T_PostRole
                //                        on p.PostID equals pr.PostID
                //                join r in udc.T_Role
                //                        on pr.RoleID equals r.RoleID
                //                select r.RoleName).Distinct();
                var roleList = (from ur in udc.T_UserRole
                                where ur.UserID == UserID
                                join r in udc.T_Role
                                       on ur.RoleID equals r.RoleID
                                select r).ToList();
                var defaultroles = (from m in udc.T_Role where m.RoleFunctionNotes == PersMSEnv.DefaultRole select m).ToList();
                for (int i = 0; i < defaultroles.Count(); i++)
                    if (!roleList.Exists(r => defaultroles[i] == r))
                        roleList.Add(defaultroles[i]);
                return roleList;

            }
            catch (Exception)
            {
                return null;
            }
        }

		public IEnumerable<int> GetControlledDepIDsByUserID(string UserID)
		{
			try
			{
				UserDataContext udc = new UserDataContext();
				var depIDList = (from p in udc.T_PostUser
								join d in udc.T_PostDepartment
									   on p.PostID equals d.PostID
								where p.UserID == UserID
								select d.ControlledDepID).Distinct();
				return depIDList;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public IEnumerable<T_Department> GetControlledDepsByUserID(string UserID)
		{
			try
			{
				UserDataContext udc = new UserDataContext();
				var posts = GetPostsByUserID(UserID);
				var depIDList = (from p in posts
								 join pd in udc.T_PostDepartment
										on p.PostID equals pd.PostID
								 join d in udc.T_Department
										on pd.ControlledDepID equals d.DepID
								 select d).Distinct();
				return depIDList;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public IEnumerable<int> GetModuleIDsByUserID(string UserID)
		{
			try
			{
				UserDataContext udc = new UserDataContext();
				var moduleIDList = (from p in udc.T_PostUser
								   join r in udc.T_PostRole
										  on p.PostID equals r.PostID
								   join m in udc.T_RoleModule
										  on r.RoleID equals m.RoleID
								   where p.UserID == UserID
								   select m.ModuleID).Distinct();
				return moduleIDList;
			}
			catch (Exception)
			{

				return null;
			}
		}
		public IEnumerable<T_Module> GetModulesByUserID(string UserID)
		{
			try
			{
				UserDataContext udc = new UserDataContext();
				var roles = GetRolesByUserID(UserID);
				var moduleList = (from r in roles 
									join rm in udc.T_RoleModule
										   on r.RoleID equals rm.RoleID
									join m in udc.T_Module
										   on rm.ModuleID equals m.ModuleID
									select m).Distinct();
				return moduleList;
			}
			catch (Exception)
			{

				return null;
			}
		}
		public IEnumerable<int> GetPostIDsByUserID(string userID)
		{
			UserDataContext udc = new UserDataContext();
			try
			{
				var postIDs = udc.T_PostUser.Where(p => p.UserID == userID).Select(pt => pt.PostID);
				return postIDs;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public IEnumerable<T_Post> GetPostsByUserID(string userID)
		{
			UserDataContext udc = new UserDataContext();
			try
			{
				var posts = (from pu in udc.T_PostUser
							  where pu.UserID == userID
							  join p in udc.T_Post
									 on pu.PostID equals p.PostID
							  select p).Distinct();
				return posts;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public bool isLegitimate(string UserID,int SystemID)
		{
			try
			{
				var modules = GetSystemsByUserID(UserID);
				if (modules == null)
					return false;
				var result = modules.FirstOrDefault(m => m.ModuleID == SystemID) == null ? false : true;
				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public IEnumerable<T_Module> GetSystemsByUserID(string UserID)
		{
			try
			{
				var modules = GetModulesByUserID(UserID);
				var result = modules.Where(m=>m.ModuleParentID==MenuSystemID.IntegrationID);
				return result;
			}
			catch (Exception e)
			{
				Error.Log(Error.DEFAULT,e.ToString());
				return null;
			}
			

		}

	}
}