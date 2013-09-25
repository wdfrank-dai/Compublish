using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace WiGetMS.Models
{
    public class Units
    {

        [DisplayName("ID")]
        public int id { get; set; }


        [DisplayName("单元名称")]
        public string unitname { get; set; }


        [DisplayName("数据源id")]
        public int datasourceid { get; set; }

        [DisplayName("数据获取参数")]
        public string theparams { get; set; }

        [DisplayName("数据源名称")]
        public string dsname { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("单元类型")]
        public int utype { get; set; }


        [DisplayName("单元内容")]
        public string ucontent { get; set; }

         
        [DisplayName("单元显示类型")]
        public int showstyleid { get; set; }

        [DisplayName("单元显示类型")]
        public string showstylename { get; set; }

        [DisplayName("下个页面")]
        public int NextPage { get; set; }

        [DisplayName("下个页面打开方式")]
        public string nextpageshowtype { get; set; }

         
        [DisplayName("是否有效")]
        public bool iseffictive { get; set; }

        [DisplayName("是否有效")]
        public string iseffictiveS { get; set; }

         
        [DisplayName("最后修改时间")]
        public string lastmodifytime { get; set; }

        [DisplayName("Css文件")]
        public string Cssname { get; set; }
         
         
        [DisplayName("res2")]
        public string res2 { get; set; }
           
        [DisplayName("修改")]
        public string Edit { get; set; }

         
        [DisplayName("删除")]
        public string Del { get; set; }

    }
}