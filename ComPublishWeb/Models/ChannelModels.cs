using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComPublishWeb.Models
{
    public class ChannelModels
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
    }

    public class FirstPageChannelInfoModels
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public string ChannelFirstPage { get; set; }
    }

    public class ChannelSetModel
    {
        public int ChannelID { get; set; }
        public int ParentChannelID { get; set; }
        public string ChannelName { get; set; }
        public int ChannelType { get; set; }
    }
}