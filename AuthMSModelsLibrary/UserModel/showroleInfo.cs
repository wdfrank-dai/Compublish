using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AuthUser2.Models
{
    public class showroleInfo
    {
        [DisplayName("ID")]
        public string UserID { get; set; }

        [DisplayName("角色")]
        public string RoleName { get; set; }

        [DisplayName("账号")]
        public string CardNo { get; set; }

        [DisplayName("姓名")]
        public string UserName { get; set; }

        [DisplayName("部门")]
        public string depName { get; set; }
    }
}