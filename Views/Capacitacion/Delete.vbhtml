@ModelType MVC4Base.Capacitacion

@Code
    ViewData("Title") = "Delete"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<fieldset>
    <legend>Capacitacion</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.idCapacitacion)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.idCapacitacion)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.NombreCapacitacion)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.NombreCapacitacion)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Descripcion)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Descripcion)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.FechaInicio)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.FechaInicio)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.FechaFin)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.FechaFin)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Activo)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Activo)
    </div>
</fieldset>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<p>
        <input type="submit" value="Delete" /> |
        @Html.ActionLink("Back to List", "Index")
    </p>
End Using
