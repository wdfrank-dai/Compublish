using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WiGetMSModelsLibrary;

namespace WiGetMS.Models
{
    public class WebPageUnitsModel
    {
        [ScaffoldColumn(false)]
        public int id
        {
            get;
            set;
        }

        //[Required]
        [DisplayName("页面名称")]
        [ScaffoldColumn(false)]
        public int pageid
        {
            get;
            set;
        }
        [UIHint("DropList")]
        [Required]
        [DisplayName("页面控件")]
        public string pageunitid
        {
            get;
            set;
        }
        [Required]
        [DisplayName("是否登录显示")]
        public bool ifloginshow
        {
            get;
            set;
        }

        [DisplayName("显示容器")]
        public string showcontainer
        {
            get;
            set;
        }
       //0占一半的小窗口，1占整行的大窗口，2，侧边栏
        [Required]
        [DisplayName("widget类型")]
        public string widgettype
        {
            get;
            set;
        }

        [Required]
        [DisplayName("widget标题")]
        public string showtitle
        {
            get;
            set;
        }

        [Required]
        [DataType("Integer")]
        [DisplayName("显示顺序")]
        public int showorder
        {
            get;
            set;
        }

        [DisplayName("CSS名称")]
        public string specialCssFile
        {
            get;
            set;
        }
  
    }
}
