@ModelType MVC4Base.webpages_Roles

@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>webpages_Roles</legend>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.RoleName)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.RoleName)
            @Html.ValidationMessageFor(Function(model) model.RoleName)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.Description)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.Description)
            @Html.ValidationMessageFor(Function(model) model.Description)
        </div>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
End Using

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
