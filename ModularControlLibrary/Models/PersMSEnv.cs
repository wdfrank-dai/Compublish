using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class PersMSEnv
{

    ///// <summary>
    ///// 报表服务器
    ///// </summary>
    //public static string RSUrl
    //{
    //    get
    //    {
    //        return System.Configuration.ConfigurationManager.AppSettings["ReportWebServer"]+"?";
    //        //return "http://210.42.151.55:8079/Report.aspx?PersMS_ReportPath=/";
    //    }
    //}
    ///// <summary>
    ///// 系统CSS路径
    ///// </summary>
    //public static string DIR_CSS
    //{
    //    get
    //    {
    //        return DIR_SKIN + "Css/";
    //    }
    //}
    ///// <summary>
    ///// 系统图片路径
    ///// </summary>
    //public static string DIR_IMAGES
    //{
    //    get
    //    {
    //        return DIR_SKIN + "Images/";
    //    }
    //}
    ///// <summary>
    ///// 系统js脚本路径
    ///// </summary>
    //public static string DIR_SCRIPTS
    //{
    //    get
    //    {
    //        return DIR_SKIN + "Scripts/";
    //    }
    //}
    ///// <summary>
    ///// 系统皮肤路径
    ///// </summary>
    //public static string DIR_SKIN
    //{
    //    get
    //    {
    //        return DIR_SKINS+"Skin1/";
    //    }
    //}
    //public  static string DIR_RELA
    //{
    //    get
    //    {
    //        return "~/";
    //    }
    //     set
    //    {
			
    //    }
    //}
    //public static string DIR_WEB_ROOT
    //{
    //    get 
    //    {
    //        return "/";
    //    }
    //    set { }
    //}
    ///// <summary>
    ///// 系统扫描件路径
    ///// </summary>
    //public static string DIR_SCANNINGCOPY
    //{
    //    get
    //    {
    //        return DIR_DATA + "ScanningCopy" + "/";
    //    }
    //    set
    //    { }
    //}
    ///// <summary>
    ///// Excel模板路径
    ///// </summary>
    //public static string DIR_DOWNLOAD
    //{
    //    get
    //    {
    //        return DIR_DATA + "Downloads" + "/";
    //    }
    //    set
    //    { }
    //}
    ///// <summary>
    ///// 系统报表路径
    ///// </summary>
    //public static string DIR_REPORT
    //{
    //    get
    //    {
    //        return DIR_DATA + "ReportTemplate"+"/";
    //    }
    //    set { }
    //}
	
    ///// <summary>
    ///// 系统错误信息提示
    ///// </summary>
    //public static string PROM_OK 
    //{
    //    get
    //    {
    //        return "操作成功!";
    //    }
    //    set { }
    //}
    ///// <summary>
    ///// 系统正确信息提示
    ///// </summary>
    //public static string PROM_ERROR
    //{
    //    get
    //    {
    //        return "操作失败!";
    //    }
    //    set { }
    //}

    //public static string DIR_SYS_SCRIPTS
    //{
    //    get
    //    {
    //        return DIR_RELA+"Scripts/";
    //    }
    //    set { }
    //}
    //public static string DIR_SYS_PLUSINS
    //{
    //    get
    //    {
    //        return DIR_SYS_SCRIPTS+"plusIns/";
    //    }
    //    set { }
    //}
    //public static string DIR_SYS_IMAGES
    //{
    //    get
    //    {
    //        return DIR_SYS_CONTENT + "Images/";
    //    }
    //    set { }
    //}
    //public static string DIR_SYS_CONTENT
    //{
    //    get
    //    {
    //        return DIR_RELA + "Content/";
    //    }
    //    set { }
    //}


    ///// <summary>
    ///// 以下是私有的
    ///// </summary>

    //private static string DIR_SKINS
    //{
    //    get
    //    {
    //        return DIR_RELA + "Content/Skins/";
    //    }
    //    set
    //    {

    //    }
    //}
    //public static string DIR_DATA
    //{
    //    get
    //    {
    //        return DIR_RELA + "Pri_Data"+"/";
    //    }
    //    set
    //    { }
    //}
    public static string SuperAdminUser
    {
        get { return "超级管理员"; }
        set { }
    }

    public static string DefaultRole
    {
        get { return "默认角色"; }
        set { }
    }

}
