using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WiGetMS.Models
{
    public class Datasource
    {
         
        [DisplayName("ID")]
        public int id { get; set; }

         
        [DisplayName("数据源名")]
        public string dsname { get; set; }

         
        [DisplayName("单元")]
        public int unitsid { get; set; }

        [DisplayName("单元")]
        public string unitsname { get; set; }
         
        [DisplayName("数据源地址")]
        public string dsurl { get; set; }


        [DisplayName("数据源参数说明")]
        public string dsparams { get; set; }


        [DisplayName("数据源参数")]
        public string dataitems { get; set; }


        [DisplayName("是否有效")]
        public bool isefficetive { get; set; }

        [DisplayName("是否有效")]
        public string isefficetiveS { get; set; }

         
        [DisplayName("修改时间")]
        public string modifydate { get; set; }

         
        [DisplayName("操作员")]
        public string operators { get; set; }

         
        [DisplayName("添加")]
        public string Add { get; set; }

         
        [DisplayName("编辑")]
        public string Edit { get; set; }

         
        [DisplayName("删除")]
        public string Del { get; set; }
    }
}