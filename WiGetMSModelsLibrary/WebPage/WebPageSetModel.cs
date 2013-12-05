using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WiGetMS.Models
{
    public class WebPageSetModel
    {
        [ScaffoldColumn(false)]
        public int pageno
        {
            get;
            set;
        }
        
        [Required]
        [DisplayName("页面名称")]
        public string pagename
        {
            get;
            set;
        }

        [Required]
        [DisplayName("显示平台")]
        public string pageshowscape
        {
            get;
            set;
        }
        [Required]
        [DisplayName("描叙")]
        public string descripte
        {
            get;
            set;
        }
        
        [Required]
        [DisplayName("是否起始页")]
        public bool iffrist
        {
            get;
            set;
        }
        [Required]
        [DisplayName("布局设置")]
        public string layoutstyle
        {
            get;
            set;
        }
 

    }
}
