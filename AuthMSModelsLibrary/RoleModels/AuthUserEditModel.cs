using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UserGovern.Models
{
    public class AuthUserEditModel
    {
        [DisplayName("用户原账号")]
        public string OldUserID { get; set; }

        [DisplayName("用户新账号")]
        public string NewUserID { get; set; }

        [DisplayName("用户类型")]
        public string PID { get; set; }
        [DisplayName("QQ")]
        public string QQ { get; set; }
                
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("联系电话")]
        public string Telephone { get; set; }
    }
}