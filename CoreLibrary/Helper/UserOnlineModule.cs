using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CoreLibrary.Models.Account;

namespace CoreLibrary.Helper
{
    public class UserOnlineModule : IHttpModule
    {
        #region IHttpModule 成员

        public static List<OnlineUser> OnlineList = null;
        private System.Timers.Timer updateTimer;
        //在线用户活动超时：分钟，默认10分钟
        private int timeOut = 10;
        //设置计时器触发周期：毫秒，默认1分钟
        private double timeInterval = 60000;

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            if (OnlineList == null)
                OnlineList = new List<OnlineUser>();

            updateTimer = new System.Timers.Timer();
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateTimer_Elapsed);
            updateTimer.Interval = timeInterval;
            updateTimer.Start();
        }

        void updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            updateTimer.Stop();
            if (OnlineList.Count > 0)
                OnlineList.RemoveAll(p => (DateTime.Now - p.LastTime).Minutes >= timeOut);
            updateTimer.Interval = timeInterval;
            updateTimer.Start();
        }

        public void Dispose()
        {

        }
        #endregion
    }
}
