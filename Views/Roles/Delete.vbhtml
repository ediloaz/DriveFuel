@ModelType MVC4Base.webpages_Roles

@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<fieldset>
    <legend>webpages_Roles</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.RoleId)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.RoleId)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.RoleName)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.RoleName)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Description)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Description)
    </div>
</fieldset>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<p>
        <input type="submit" value="Delete" /> |
        @Html.ActionLink("Back to List", "Index")
    </p>
End Using
