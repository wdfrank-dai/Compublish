using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UserGovern.Models
{
    public class roleusershowmodel
    {
        [DisplayName("ID")]
        [Required]
        public int RoleID { get; set; }

        [DisplayName("ParentID")]
        [Required]
        public int RoleParentID { get; set; }

        [DisplayName("角色姓名")]
        [Required]
        public string RoleName { get; set; }

        [DisplayName("角色说明")]
        [Required]
        public string RoleFunctionNotes { get; set; }

        [DisplayName("是否为子节点")]
        [Required]
        public bool IsChildrenRole { get; set; }

        [DisplayName("")]
        [Required]
        public bool IsDelete { get; set; }

        [DisplayName("")]
        [Required]
        public int OperatorID { get; set; }

        [DisplayName("")]
        [Required]
        public string OperatorName { get; set; }

        [DisplayName("")]
        [Required]
        public string OperateDate { get; set; }

        [DisplayName("")]
        [Required]
        public string depName { get; set; }
    }
}