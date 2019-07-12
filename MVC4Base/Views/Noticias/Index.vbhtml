@ModelType IEnumerable(Of MVC4Base.Noticias)

@Code
    ViewData("Title") = "Noticias"
    ViewData("Descripcion") = "Crea, visualiza y edita las noticias que aparecerán en el newsfeed de la app"
End Code


<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Nueva Noticia", "Create", Nothing, New With {.class = "btn btn-success"})

    </div>
    <div class="card-block">

    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-condensed table-striped datatable m-b-0" id="tablaNoticias">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Titulo)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Mensaje)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.FechaInicio)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.FechaFin)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Activa)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.idTipoNoticia)</th>
                        <th></th>
                    </tr>
                </thead>

                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.idNoticia)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.Titulo)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.Mensaje)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.FechaInicio)</td>
                             <td>@Html.DisplayFor(Function(modelItem) currentItem.FechaFin)</td>

                            <td>
                                @If currentItem.Activa = True Then
                                    @<span class="label label-success">Activa</span>
                                Else
                                    @<span class="label label-danger">Inactiva</span>
                                End If
                            </td>
                            <td>
                                <span class="label label-@currentItem.TipoNoticiaColor">
                                    @Html.DisplayFor(Function(modelItem) currentItem.TipoNoticia.Descripcion)
                                </span>
                            </td>
                            <td>
                                @Html.ActionLink(" ", "Edit", "Noticias", New With {.id = currentItem.idNoticia}, New With {.class = "icon-note"})

                                @Html.ActionLink(" ", "Delete", "Noticias", New With {.id = currentItem.idNoticia}, New With {.class = "icon-trash"})
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



