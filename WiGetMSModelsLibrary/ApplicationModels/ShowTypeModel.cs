using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WiGetMS.Models
{
    public class ShowTypeModel
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [DisplayName("名称")]
        public string name { get; set; }
        
        [DisplayName("分割标记")]
        public string FillMark { get; set; }

        [DisplayName("单元数量")]
        public int FillCount { get; set; }

        [DisplayName("注释")]
        public string remark { get; set; }

        [DisplayName("内容")]
        public string viewdate { get; set; }

        [DisplayName("编辑")]
        public string edit { get; set; }
    }
}
