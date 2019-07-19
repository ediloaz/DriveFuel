@ModelType MVC4Base.Cliente

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<fieldset>
    <legend>Cliente</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.idCliente)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.idCliente)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.NombreCliente)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.NombreCliente)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Logo)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Logo)
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
