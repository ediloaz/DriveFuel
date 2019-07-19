@ModelType MVC4Base.webpages_Roles

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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
<p>
    @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |*@
    @Html.ActionLink("Back to List", "Index")
</p>
