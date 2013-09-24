using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AuthModelManage.Models
{
    public class Module
    {
        [Required]
        [DisplayName("模块ID")]
        public int ModuleID { get; set; }

        [Required]
        [DisplayName("父模块ID")]
        public int ModuleParentID { get; set; }

        [Required]
        [DisplayName("模块名称")]
        public string ModuleName { get; set; }

        [Required]
        [DisplayName("模块功能描述")]
        public string ModuleNotes { get; set; }

        [Required]
        [DisplayName("模块层次")]
        public string ModuleHierarchy { get; set; }

        [Required]
        [DisplayName("模块优先级")]
        public int ModulePriority { get; set; }

        [Required]
        [DisplayName("路由名称")]
        public string RouteName { get; set; }

        [Required]
        [DisplayName("是否菜单")]
        public bool IsMenu { get; set; }

        [Required]
        [DisplayName("是否删除")]
        public bool IsDelete { get; set; }

        [Required]
        [DisplayName("操作人ID")]
        public int OperatorID { get; set; }

        [Required]
        [DisplayName("操作人姓名")]
        public string OperatorName { get; set; }

        [Required]
        [DisplayName("操作日期")]
        public System.DateTime OperateDate { get; set; }
    }
}