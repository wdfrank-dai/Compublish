using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace WiGetMS.Models
{
    public class UnitsShowStyle
    {

        [DisplayName("ID")]
        public int id { get; set; }

         
        [DisplayName("单元类型名称")]
        public string name { get; set; }


        [DisplayName("单元类型描述")]
        public string note { get; set; }


        [DisplayName("单元类型展示方式")]
        public string Add { get; set; }
    }
}