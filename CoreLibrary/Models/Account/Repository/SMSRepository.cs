using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersMS.Models.Account;

namespace PersMS.Models.Repository
{
    public partial class SMSRepository
    {
        public void SendSMSToPerson(int personid, string smscontent,string comefrom)
        {
            NewUserDataContext udb = new NewUserDataContext();
            T_SMSInfo sm = new T_SMSInfo();
            var person = udb.T_User.Where(m => m.AccountNo == personid).FirstOrDefault();
            if (person.Telephone != null)
            {
                if (person.Telephone.Trim().Length == 11)
                {
                    sm.PID = personid;
                    sm.TargetPhone = person.Telephone.Trim();
                    sm.TContent = smscontent;
                    sm.SubMitTime = DateTime.Now;
                    sm.ComeFrom = comefrom;
                    udb.T_SMSInfo.InsertOnSubmit(sm);
                    udb.SubmitChanges();
                    udb.Dispose();
                }
            }            
            return;
        }
    }
}
