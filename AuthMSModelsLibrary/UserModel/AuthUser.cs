using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AuthUserDataLibrary.Models
{
    public class AuthUserM
    {

        [Required]
        [DisplayName("用户账号")]
        public string UserID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 1)]
        [DisplayName("用户编号")]

        public string EmployeeNO { get; set; }

        [DisplayName("用户姓名")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("密码")]
        [Required]
        public string Password { get; set; }
        [DisplayName("用户类型")]
        [Required]
        public string UserType { get; set; }
        [DisplayName("QQ")]
        [Required]
        public string QQ { get; set; }
        //[DisplayName("")]                 
        [Required]
        [DisplayName("Email")]

        public string Email { get; set; }
        [DisplayName("联系电话")]
        [Required]
        public string Telephone { get; set; }

        [DisplayName("用户类型")]
        public string PID { get; set; }
        public string IDNo { get; set; }
        [Required]
        public string DepNO { get; set; }
        public string depName { get; set; }
        [Required]
        public string DepSectionNO { get; set; }
        public string DepSectionName { get; set; }
        public string Sex { get; set; }
        public long CardNo { get; set; }
        public long AccountNo { get; set; }
        public string IDCard { get; set; }
        public string Flag { get; set; }
        public bool InRole { get; set; }
    }
}