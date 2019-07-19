@ModelType IEnumerable(Of MVC4Base.Capacitacion)

@Code
    ViewData("Title") = "Capacitaciones"
    ViewData("Descripcion") = "Crea, visualiza y edita las capacitaciones que aparecerán en la app"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Nueva Capacitación", "Create", Nothing, New With {.class = "btn btn-success"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-condensed table-striped datatable m-b-0" id="tablaNoticias">
                <thead>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.idCapacitacion)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.NombreCapacitacion)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.FechaInicio)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.FechaFin)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Activo)</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.idCapacitacion)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.NombreCapacitacion)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.Descripcion)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.FechaInicio)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.FechaFin)</td>
                            <td>
                                <span class="label label-@currentItem.ActivoColor">
                                    @Html.DisplayFor(Function(modelItem) currentItem.ActivoTexto)
                                </span>
                            <td>
                                @Html.ActionLink(" ", "Edit", "Capacitacion", New With {.id = currentItem.idCapacitacion}, New With {.class = "icon-note"})
                                @*@Html.ActionLink(" ", "Details", "Details", New With {.id = currentItem.idCapacitacion, .class = "icon-note"})*@
                                @Html.ActionLink(" ", "Delete", "Capacitacion", New With {.id = currentItem.idCapacitacion}, New With {.class = "icon-trash"})
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



