using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AuthUser2.Models
{
    public class Role
    {
        private string roleFunctionNotes = "";

        [ScaffoldColumn(false)]
        public int RoleID { get; set; }

        [Display(Name = "父角色ID")]
        public int RoleParentID { get; set; }

        [Required]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }

        [Display(Name = "是否叶子角色")]
        public bool IsChildrenRole { get; set; }

        [Display(Name = "角色描述")]
        public string RoleFunctionNotes { get { return this.roleFunctionNotes; } set { this.roleFunctionNotes = value; } }

        [Display(Name = "用户状态")]
        public bool IsDelete { get; set; }

        [ScaffoldColumn(false)]
        public int OperatorID { get; set; }

        [Display(Name = "操作人姓名")]
        public string OperatorName { get; set; }

        [Display(Name = "操作日期")]
        public DateTime OperateDate { get; set; }
    }
}