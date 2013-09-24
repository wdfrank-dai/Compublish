using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WiGetMS.Models
{
    public class ShowPage
    {
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [DisplayName("页面名称")]
        public string pagename { get; set; }

        [DisplayName("页面模板")]
        public int showtypeid { get; set; }

        [DisplayName("页面模板")]
        public string showtypename { get; set; }

        [DisplayName("下一个页面")]
        public int nextpage { get; set; }

        [DisplayName("下一页面")]
        public string nextpagename { get; set; }

        [DisplayName("单元描述")]
        public string actionname { get; set; }

        [DisplayName("页面描述")]
        public string note { get; set; }

        [DisplayName("第三方应用")]
        public int appid { get; set; }

        [DisplayName("第三方应用")]
        public string  appname { get; set; }


        [DisplayName("页面单元")]
        public string actionunits { get; set; }

        [DisplayName("页面单元")]
        public string actionunitsname { get; set; }


        [DisplayName("是否为初始模板")]
        public bool isStart { get; set; }

        [DisplayName("是否有效")]
        public bool isEffictive { get; set; }

        [DisplayName("是否为初始模板")]
        public string isStartS { get; set; }

        [DisplayName("是否有效")]
        public string isEffictiveS { get; set; }

        [DisplayName("最后修改时间")]
        public string lastmodifytime { get; set; }

        [DisplayName("编辑")]
        public string edit { get; set; }
    }
}