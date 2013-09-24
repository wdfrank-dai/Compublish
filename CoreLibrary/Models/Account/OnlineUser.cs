using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLibrary.Models.Account
{
    public class OnlineUser
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 最后一次活动时间
        /// </summary>
        public DateTime LastTime { get; set; }
        /// <summary>
        /// 最后一次活动地址
        /// </summary>
        public string LastActionUrl { get; set; }
        /// <summary>
        /// 登陆IP地址
        /// </summary>
        public string LoginIp { get; set; }
        /// <summary>
        /// 是否为游客
        /// </summary>
        public bool IsGuest { get; set; }
        /// <summary>
        /// 当前会话ID
        /// </summary>
        public string SessionID { get; set; }

    }
}
