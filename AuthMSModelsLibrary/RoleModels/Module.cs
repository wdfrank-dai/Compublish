using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UserGovern.Models
{
    public class Module
    {
        [Required]
        [DisplayName("ID")]
        public string ModuleID { get; set; }

        [Required]
        [DisplayName("模块父ID")]
        public string ModuleParentID { get; set; }

        [Required]
        [DisplayName("模块名称")]
        public string ModuleName { get; set; }

        [Required]
        [DisplayName("模块描述")]
        public string ModuleNotes { get; set; }

        [Required]
        [DisplayName("")]
        public string ModuleHierarchy { get; set; }

        [Required]
        [DisplayName("")]
        public string ModulePriority { get; set; }

        [Required]
        [DisplayName("")]
        public string RouteName { get; set; }

        [Required]
        [DisplayName("")]
        public string IsMenu { get; set; }

        [Required]
        [DisplayName("")]
        public string IsDelete { get; set; }

        [Required]
        [DisplayName("操作员ID")]
        public string OperatorID { get; set; }

        [Required]
        [DisplayName("操作员姓名")]
        public string OperatorName { get; set; }

        [Required]
        [DisplayName("操作时间")]
        public string OperateDate { get; set; }
    }
}