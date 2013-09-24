using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WiGetMS.Models
{
    public class ShowUnitsCss
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [DisplayName("名称")]
        public string name { get; set; }

        [DisplayName("控件ID")]
        public int unitsid { get; set; }

        [DisplayName("控件名称")]
        public string unitname { get; set; }

        [DisplayName("CSS样式")]
        public string css { get; set; }

        public string res1 { get; set; }

        public string res2 { get; set; }

        [DisplayName("编辑")]
        public string edit { get; set; }
    }
}