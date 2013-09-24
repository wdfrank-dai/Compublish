<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site1.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    GetAllApp
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>GetAllApp</h2>

<div>
<% Html.Telerik().Grid<WiGetMS.Models.showApplication>()
          .Name("UsersGrid")
          .DataKeys(keys =>
           {
              keys.Add(c => c.id);
           })
          .Pageable(paging =>
                  paging.PageSize(15)
                        .Style(GridPagerStyles.NextPreviousAndNumeric)
                        .Position(GridPagerPosition.Bottom)
              )
          .ToolBar(commands => commands.Template("<input type ='button' value='添 加' class='t-button' onclick = Add() />"))
          .DataBinding(dataBinding =>
              {
                  dataBinding.Ajax()
                   .Select("GetAllApp", "ApplicationManage");
              }) 
          .Columns(columns =>   
          {
              columns.Bound(c => c.id).Hidden(true);    
              columns.Bound(c => c.Appname);
              columns.Bound(c => c.Note);
              columns.Bound(c => c.Logourl);
              columns.Bound(c => c.Lastmodifytime);
              columns.Bound(c => c.Publishto);
              columns.Bound(c => c.Tags);
              columns.Bound(c => c.Appname)
                 .ClientTemplate("<input type='button' value='编辑' class='t-button' onclick='classeditwindow(<#= id#>)' />   <input type='button' value='删除' class='t-button' onclick='deleteapps(<#= id#>)' />").Title("操作");
          })
          .Editable(editing => editing.Mode(GridEditMode.PopUp))
          .Pageable()
          .Scrollable()
          .Filterable()
          .Resizable(resizing => resizing.Columns(true))
          .Reorderable(reorder => reorder.Columns(true))          
          .Footer(true)
          .Render();
%>
</div>


<div >
    <%  Html.Telerik().Window()
            .Name("modification-Window")
            .Title("修改分类组信息")
            .Content(() => {})
            .Width(580)
            .Height(400)
            .Draggable(true)
            .Modal(true)
            .Visible(false)
            .Render();
    %>
    </div>
  
<div>
    <%  Html.Telerik().Window()
            .Name("AddAppsWindow")
            .Title("添加组信息")
            .Content(() => {})
            .Width(580)
            .Height(400)
            .Draggable(true)
            .Modal(true)
            .Resizable(resizing => resizing
                .Enabled(true)
                .MinHeight(400)
                .MinWidth(700)
                .MaxHeight(400)
                .MaxWidth(700)
            )
            .Visible(false)
            .Render();
    %>




</div>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">

<script src="<%: Url.Content("~/Scripts/jquery.unobtrusive-ajax.min.js") %>" type="text/javascript"></script>
<script type="text/javascript">

    function classeditwindow(id) {
        var window = $('#modification-Window').data('tWindow');
        window.ajaxRequest("/ApplicationManage/EditApps?cid=" + id);
        window.center().open()
    }

    function deleteapps(id) {
        $.ajax({
            type: "post",
            url: '<%=Url.Action("DeleteApps","ApplicationManage")%>',
            cache: false,
            data: "id=" + id,
            //data: {dataid : id },
            datatype: "html",
            success: function (ms) {
                var res = ms;
                if (res != "")
                    alert(res);
                else
                    alert("删除失败");
                var grid = $('#UsersGrid').data('tGrid'); //重新加载
                grid.rebind();
                //autodisapear();
            }
        })
    }

    function GetResult() {

        alert("修改成功！");
        $('#modification-Window').data('tWindow').close();
        var grid = $('#UsersGrid').data('tGrid'); //重新加载
        grid.rebind();
        autodisapear();
    }

    function Add() {
        var window = $('#AddAppsWindow').data('tWindow');
        window.ajaxRequest("/ApplicationManage/AddApps");
        window.center().open();
    }

    function AddWindow() {    
        alert("添加成功")
        $('#AddAppsWindow').data('tWindow').close();
        var grid = $('#UsersGrid').data('tGrid'); //重新加载
        grid.rebind();
        autodisapear();
    }
          
</script>
</asp:Content>
