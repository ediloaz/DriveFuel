@ModelType MVC4Base.Capacitacion

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

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
<p>
    @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.PrimaryKey}) |*@
    @Html.ActionLink("Back to List", "Index")
</p>
