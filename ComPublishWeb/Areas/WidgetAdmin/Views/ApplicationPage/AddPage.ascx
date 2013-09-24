<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WiGetMS.Models.ShowPage>" %>


<% using (Ajax.BeginForm("PageAddSave", "ApplicationPage", new AjaxOptions { HttpMethod = "POST", UpdateTargetId = "Message", OnSuccess = "AddPageWindow()" }))
   { %>

    <%: Html.ValidationSummary(true) %>
   <table>
<%--       <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.appid) %>
        </td>
        <td class="PersMS-table-4cols-2">           
            <%=ViewData["Appid"] %>
        </td>
      </tr>--%>
      <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.pagename) %>：
        </td>
        <td class="PersMS-table-4cols-3">
            <%: Html.EditorFor(model => model.pagename) %>
            <%: Html.ValidationMessageFor(model => model.pagename) %>
        </td>
      </tr>
      <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.showtypename) %>：
        </td>
        <td class="PersMS-table-4cols-2">
            <%= Html.DropDownList("showtypeid2", null, "--请选择--", new { style = "width:180px" })%>
        </td>
      </tr>
      <%--<tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.nextpage) %>：
        </td>
        <td class="PersMS-table-4cols-2">
            <%= Html.DropDownList("pagelist2", null, "--请选择--", new { style = "width:180px" })%>
        </td>
      </tr>--%>
      <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.actionname) %>：
        </td>
        <td class="PersMS-table-4cols-3">
            <%: Html.EditorFor(model => model.actionname) %>
            <%: Html.ValidationMessageFor(model => model.actionname) %>
        </td>
      </tr>
      <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.actionunits) %>：
        </td>
        <td class="PersMS-table-4cols-2">
            <%= Html.DropDownList("Units2", ViewData["Units2"] as SelectList, new { multiple = "multiple", style = "z-index:999;width:130px" })%>
        </td>
      </tr>
      <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.isStart) %>：
        </td>
        <td class="PersMS-table-4cols-2">
            <%: Html.CheckBoxFor(model => model.isStart) %>
            <%: Html.ValidationMessageFor(model => model.isStart) %>
        </td>
      </tr>
        <tr>
        <td class="PersMS-table-4cols-1">
            <%: Html.LabelFor(model => model.note) %>：
        </td>
        <td class="PersMS-table-4cols-2">
            <%: Html.TextAreaFor(model => model.note, new { @style = "width:180px;height:90px" })%>
            <%: Html.ValidationMessageFor(model => model.note) %>
        </td>
      </tr>
  </table>
              <div class="PersMS-table-4cols-1" style="margin:0 auto;width:200px">
                 <button class="button" type="submit">
				<img src="../../../../Images/icons/tick1.png" alt="Save" />
				添加
				</button>
                <span class="text_button_padding"> </span>
				 
                 <button class="button" type="button" onclick="ClosePageWindow()">
				<img src="../../../../Images/icons/close.png" alt="Save" />
				取消
				</button>
        </div>


     <script src="<%: Url.Content("~/Scripts/jquery-ui-1.8.11.js") %>" type="text/javascript"></script>
     <script src="<%: Url.Content("~/Scripts/plusIns/multiselect/jquery.multiselect.js") %>" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
        
         $("#Units2").multiselect({
             selectedList: 3,
             minWidth:180,
             header: "请选择Css"
         });
//         $('#Units2.ui-multiselect').width(130);

</script>
<% } %>

