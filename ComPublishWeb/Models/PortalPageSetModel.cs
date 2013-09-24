using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ComPublishWeb.Models
{
    public class PortalPageSetModel
    {
        [Display(Name = "用户帐号/手机号码")]
        public string UserAccount { get; set; }

        [Required]
        [Display(Name = "页面样式")]
        public string PageStyle { get; set; }

        [Display(Name = "页面显示频道")]
        public string Channels { get; set; }
    }

}