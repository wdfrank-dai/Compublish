<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WiGetMS.Models.showApplication>" %>



<% using (Ajax.BeginForm("SaveAppEdit", "ApplicationManage", new { id = Model.id }, new AjaxOptions { HttpMethod = "POST", UpdateTargetId = "Message", OnSuccess = "GetResult()" }))
       { %>

    <fieldset>
        <legend>App编辑</legend>
         <div id="Message" style="color:Red"></div>
        
            <% Html.HiddenFor(m => m.id); %>
             
        <div class="editor-field">
             APP：<%: Html.EditorFor(m=>m.Appname) %>
             <%: Html.ValidationMessageFor(model => model.Appname)%>
        </div>

        <div class="editor-field">
            备注：<%: Html.EditorFor(m => m.Note) %>
            <%: Html.ValidationMessageFor(m => m.Note)%>
        </div>
       
        <div class="editor-field">
             图片：<%: Html.EditorFor(m => m.Logourl) %>
             <%: Html.ValidationMessageFor(m => m.Logourl)%>
        </div>

      
        <div class="editor-field">
            发布平台：<%: Html.EditorFor(m => m.Publishto) %>
            <%: Html.ValidationMessageFor(m => m.Publishto)%>
        </div>

       
        <div class="editor-field">
           标签：<%: Html.EditorFor(m => m.Tags) %>
            <%: Html.ValidationMessageFor(m => m.Tags)%>
        </div>

         <p>
                  <input type="submit" value="保 存" />
         </p>
    </fieldset>

<% } %>