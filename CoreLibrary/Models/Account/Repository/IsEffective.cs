using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersMS.Models.Account;

namespace PersMS.Models.Repository
{
	public partial class AccountRepository
	{
		public bool isEffective(PersMS.Models.Account.Account account)
		{
			//NewUserDataContext udc = new NewUserDataContext();

			//string id = account.UserID;
			//string pwd = account.Password;
			//bool isEff = false;
			//try
			//{
			//    var user = udc.T_User.SingleOrDefault(u => u.UserID == id && u.Password == pwd);
			//    isEff = (user == null ? false : true);
			//    if (isEff)
			//    {
			//        Account = new PersMS.Models.Account.Account();
			//        Account.UserID = user.UserID;
			//        Account.UserName = user.UserName;
			//        //Account.UserType = user.UserType;
			//        Account.Password = user.Password;
			//        Account.EmployeeID =new AccountRepository().GetEmpIDByUserID(user.UserID);
			//        Account.EmployeeNO = user.UserID;

			//        Account.PostIDs = GetPostIDsByUserID(user.UserID).ToList();
			//        Account.Posts = GetPostsByUserID(user.UserID).ToList();
			//        Account.RoleIDs = GetRoleIDsByUserID(user.UserID).ToList();
			//        Account.Roles = GetRolesByUserID(user.UserID).ToList();
			//        Account.ModuleIDs = GetModuleIDsByUserID(user.UserID).ToList();
			//        Account.Modules = GetModulesByUserID(user.UserID).ToList();
			//        Account.ControlledDepIDs = GetControlledDepIDsByUserID(user.UserID).ToList();
			//        Account.ControlledDeps = GetControlledDepsByUserID(user.UserID).ToList();
			//    }

			//    return isEff;
			//}
			//catch (System.Exception ex)
			//{
			//    Error.Log(Error.DEFAULT, ex.ToString());
			//    return false;
			//}
			return false;
		}



	}
}