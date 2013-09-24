using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;

namespace WiGetMS.Models
{
    public class showApplication
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("APP名称")]
        public string Appname { get; set; }

        [DisplayName("APP备注")]
        public string Note { get; set; }

        [DisplayName("优先级")]
        public int Priority { get; set; }

        [DisplayName("图片")]
        public string Logourl { get; set; }

        [DisplayName("发布平台")]
        public string Publishto { get; set; }

        public string publishtoMR { get; set; }//获取下拉菜单的默认值

        [DisplayName("是否发布")]
        public bool Ispublish { get; set; }

        [DisplayName("是否发布")]
        public string Ispublishstring { get; set; }

        [DisplayName("操作人员")]
        public string operators { get; set; }

        [DisplayName("最新修改时间")]
        public string Lastmodifytime { get; set; }

        [DisplayName("标签")]
        public string Tags { get; set; }

        [DisplayName("审核")]
        public string  Passed { get; set; }

        [DisplayName("显示样式")]
        public int  ShowType { get; set; }

        [DisplayName("显示样式")]
        public string  ShowTypes { get; set; }

        [DisplayName("登录显示")]
        public bool IfLoginShow { get; set; }

        [DisplayName("登录显示")]
        public string IfLoginShows { get; set; }
    }
}