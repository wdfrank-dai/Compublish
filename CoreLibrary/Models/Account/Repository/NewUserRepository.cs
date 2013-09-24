using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreLibrary.Models;
using CoreLibrary.Models.Account;

namespace CoreLibrary.Models.Repository
{
	public partial class NewUserRepository
	{
		public T_User GetUser(string UserID, string Password)
		{
			NewUserDataDataContext nudc = new NewUserDataDataContext();
			return nudc.T_User.FirstOrDefault(u=>(u.UserID==UserID || u.Telephone.Trim() == UserID) && u.Password==Password);
		}
		public T_User GetUserByUserID(string UserID)
		{
			NewUserDataDataContext nudc = new NewUserDataDataContext();
			return nudc.T_User.FirstOrDefault(u=>u.UserID==UserID);
		}

        //更新T_User表IDCard-identity,IDNo-IDENTITYID,CardNo-cardid,UserId-sno,AccountNo-account,telephone-phone
        public string CompXzxUser(string identity, string identityid, string phone, string sno, int account, string cardid)
        {
            NewUserDataDataContext nudc = new NewUserDataDataContext();
            try
            {
                T_User user = nudc.T_User.Where(m => m.IDCard == identity).FirstOrDefault();

                if (user != null)
                {
                    if (user.IDNo != identityid || user.Telephone != phone || user.CardNo != long.Parse(cardid) || user.AccountNo != account)
                    {
                        if (user.AccountNo != account && account > 0)
                        {
                            T_User clearuser = nudc.T_User.Where(m => m.AccountNo == account).FirstOrDefault();
                            if (clearuser != null)
                            {
                                clearuser.AccountNo = -1;
                                nudc.SubmitChanges();
                            }
                        }
                        user.IDNo = identityid;
                        user.Telephone = phone;
                        user.CardNo = long.Parse(cardid);
                        user.AccountNo = account;
                        nudc.SubmitChanges();

                        return "(" + identity + "," + phone + "," + cardid + "," + account.ToString() + ") Success:the User info has updated";
                    }
                    nudc.Dispose();
                    return "Success:the User info Not Change";
                }
                nudc.Dispose();
                return "Success:No the User info";
            }
            catch (Exception e)
            {
                nudc.Dispose();
                return "Error"+e.Message.Substring(0,100);
            }
        }

        public string GetNeedAsyUser()
        {
            NewUserDataDataContext nudc = new NewUserDataDataContext();
            string result = "";
            var t = nudc.T_User.Where(m => m.AccountNo == 0).ToList();
            for (int i = 0; i < t.Count(); i++)
            {
                result = result + t[i].IDCard;
                if (i < t.Count())
                    result = result + ",";
            }
            return result;
        }
        
	}
}