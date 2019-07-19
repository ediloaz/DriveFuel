@ModelType MVC4Base.Grupo

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<fieldset>
    <legend>Grupo</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.idGrupo)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.idGrupo)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Descripcion)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Descripcion)
    </div>
</fieldset>
<p>
    @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |*@
    @Html.ActionLink("Back to List", "Index")
</p>
