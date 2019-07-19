@ModelType IEnumerable(Of MVC4Base.Producto)
@Code
    ViewData("Title") = "Producto"
    ViewData("Descripcion") = "Crea, visualiza y edita productos"
End Code


<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Nuevo Cliente", "Create", Nothing, New With {.class = "btn btn-success"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-condensed table-striped datatable m-b-0" id="tablaNoticias">
                <thead>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.idProducto)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.NombreProducto)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Clave)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Cliente.NombreCliente)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Activo)</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.idProducto)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.NombreProducto)</td>
                             <td>@Html.DisplayFor(Function(modelItem) currentItem.Clave)</td>
                             <td>@Html.DisplayFor(Function(modelItem) currentItem.Cliente.NombreCliente)</td>
                            <td>
                                <span class="label label-@currentItem.ActivoColor">
                                    @Html.DisplayFor(Function(modelItem) currentItem.ActivoTexto)
                                </span>
                            <td>
                                @Html.ActionLink(" ", "Edit", "Productos", New With {.id = currentItem.idProducto}, New With {.class = "icon-note"})
                                @*@Html.ActionLink(" ", "Details", "Details", New With {.id = currentItem.idCapacitacion, .class = "icon-note"})*@
                                @Html.ActionLink(" ", "Delete", "Productos", New With {.id = currentItem.idProducto}, New With {.class = "icon-trash"})
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>

@Section Styles
    @Styles.Render("~/Content/datatable")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/datatable")
    <script>
        $(document).ready(function () {
            $("#tablaNoticias").DataTable({ responsive: true, "language": DataTables.languaje.es, "order": [[0, "desc"]] });
        });
    </script>
End Section



