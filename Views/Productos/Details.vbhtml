@ModelType MVC4Base.Producto

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<fieldset>
    <legend>Producto</legend>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.idProducto)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.idProducto)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Cliente.NombreCliente)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Cliente.NombreCliente)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.Clave)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.Clave)
    </div>

    <div class="display-label">
        @Html.DisplayNameFor(Function(model) model.NombreProducto)
    </div>
    <div class="display-field">
        @Html.DisplayFor(Function(model) model.NombreProducto)
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
