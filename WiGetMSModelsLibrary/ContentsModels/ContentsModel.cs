using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WiGetMS.Models
{
    public class ContentsModel
    {
        [Required]
        [DisplayName("内容ID")]
        public int ID { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("内容")]
        public string T_content { get; set; }

        [DisplayName("优先级")]
        public int Priority { get; set; }

        [DisplayName("是否热点")]
        public bool Hotspot { get; set; }

        [DisplayName("是否热点")]
        public string HotspotS { get; set; }

        [DisplayName("是否置顶")]
        public bool Stick { get; set; }

        [DisplayName("是否置顶")]
        public string StickS { get; set; }

        [DisplayName("审核")]
        public string Passed { get; set; }

        [DisplayName("标签")]
        public string Tags { get; set; }

        [DisplayName("外部链接")]
        public string Sorlink { get; set; }

        [DisplayName("作者")]
        public string Creator { get; set; }

        [DisplayName("创作时间")]
        public string Createtime { get; set; }

        [DisplayName("操作人")]
        public string Operator { get; set; }

        [DisplayName("修改时间")]
        public string Lastmodifytime { get; set; }

        [DisplayName("审核人")]
        public string Verifier { get; set; }

        [DisplayName("审核时间")]
        public string Verifytime { get; set; }

        [DisplayName("内容简介")]
        public string Summary { get; set; }

        [DisplayName("审核意见")]
        public string Suggestion { get; set; }

        [DisplayName("操 作")]
        public string Edit { get; set; }
        
    }
}