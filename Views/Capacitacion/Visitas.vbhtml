@ModelType IEnumerable(Of MVC4Base.CapacitacionVisitas)

@Code
    ViewData("Title") = "Actividad Reciente"
    ViewData("Descripcion") = "Visualiza la actividad reciente sobre las capacitaciones"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Capacitaciones", "Index", Nothing, New With {.class = "btn btn-info"})
        @Html.ActionLink("Inactividad", "Inactividad", Nothing, New With {.class = "btn btn-danger", .style = "float:right"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-condensed table-striped datatable m-b-0" id="tablaNoticias">
                <thead>
                    <tr>
                        <th>@Html.DisplayName("Id")</th>
                        <th>@Html.DisplayName("Fecha de Visita")</th>
                        <th>@Html.DisplayName("Usuario")</th>
                        <th>@Html.DisplayName("Capacitacion")</th>
                        <th>@Html.DisplayName("Tema")</th>
                        <th>@Html.DisplayName("Titulo del Archivo")</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr class="clickable-row" data-href="@Url.Action("ArchivoDetalle", "Capacitacion", New With {.id = item.idCapacitacionArchivos})">
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.idCapacitacionVisitas)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.FechaVisita)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.Usuarios.Nombre)</td>


                            <td>@Html.DisplayFor(Function(modelItem) currentItem.CapacitacionArchivos.CapacitacionTema.Capacitacion.NombreCapacitacion)</td>        
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.CapacitacionArchivos.CapacitacionTema.Titulo)</td>        
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.CapacitacionArchivos.Titulo)</td>
                            <td>@Html.ActionLink(" ", "ArchivoDetalle", "Capacitacion", New With {.id = currentItem.idCapacitacionArchivos}, New With {.class = "icon-book-open"})</td>
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

            $(".clickable-row").click(function () {
                window.location = $(this).data("href");
            });
        });

    </script>
End Section



