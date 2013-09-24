    <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WiGetMS.Models.showApplication>" %>


<% using (Ajax.BeginForm("AddApps", "ApplicationManage", new AjaxOptions { HttpMethod = "POST", UpdateTargetId = "Message", OnSuccess = "AddWindow()" }))
   { %>
    <%: Html.ValidationSummary(true) %>
    <fieldset>
        <legend>Application</legend>

         <% Html.HiddenFor(m => m.id); %>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Appname) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Appname)%>
            <%: Html.ValidationMessageFor(model => model.Appname)%>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Note) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Note)%>
            <%: Html.ValidationMessageFor(model => model.Note)%>
        </div>


        <div class="editor-label">
            <%: Html.LabelFor(model => model.Logourl) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Logourl) %>
            <%: Html.ValidationMessageFor(model => model.Logourl) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Publishto) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Publishto) %>
            <%: Html.ValidationMessageFor(model => model.Publishto) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Tags) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Tags) %>
            <%: Html.ValidationMessageFor(model => model.Tags) %>
        </div>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
<% } %>
