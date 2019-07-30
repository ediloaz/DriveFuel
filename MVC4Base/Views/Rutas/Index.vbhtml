@ModelType IEnumerable(Of MVC4Base.Ruta)

@Code
    ViewData("Title") = "Rutas"
    ViewData("Descripcion") = "Crea, visualiza y edita las diferentes rutas"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Nueva Ruta", "Create", Nothing, New With {.class = "btn btn-success"})
        @Html.ActionLink("Nueva Ruta Basada En", "Create", Nothing, New With {.class = "btn btn-info", .style = "float:right"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered datatable table-striped m-b-0">
                <thead>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th># Checkpoints</th>
                        <th></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th># Checkpoints</th>
                        <th></th>
                    </tr>
                </tfoot>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        Dim saltar = 0
                        If (ViewBag.RoleActual = "Cliente") Then
                            For Each grupo In item.Grupo
                                If (saltar = 1) Then
                                    GoTo Siguiente
                                End If
                                For Each usuario In grupo.Usuarios
                                    If (ViewBag.UsuarioActual.ToString.Contains(usuario.Correo)) Then
                                        saltar = 1
                                        @<tr>
                                            <td>
                                                @Html.DisplayFor(Function(modelItem) currentItem.Descripcion)
                                            </td>
                                            <td>@item.RutaCheckpoint.Count</td>
                                            <td>
                                                @Html.ActionLink("Checks", "Checks", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                                &nbsp;&nbsp;
                                                @Html.ActionLink("Formas", "RespuestasFormas", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                                &nbsp;&nbsp;
                                                @Html.ActionLink("Reporte", "Details", "Reportes", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                                &nbsp;&nbsp;
                                                @Html.ActionLink("Editar", "Edit", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                                &nbsp;&nbsp;
                                                @Html.ActionLink("Borrar", "Delete", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-danger"})
                                            </td>
                                        </tr>
                                    End If
                                Next
                            Next
Siguiente:
                        Else
                            @<tr>
                                <td>
                                    @Html.DisplayFor(Function(modelItem) currentItem.Descripcion)
                                </td>
                                <td>@item.RutaCheckpoint.Count</td>
                                <td>
                                    @Html.ActionLink("Checks", "Checks", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                    &nbsp;&nbsp;
                                    @Html.ActionLink("Formas", "RespuestasFormas", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                    &nbsp;&nbsp;
                                    @Html.ActionLink("Reporte", "Details", "Reportes", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                    &nbsp;&nbsp;
                                    @Html.ActionLink("Editar", "Edit", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
                                    &nbsp;&nbsp;
                                    @Html.ActionLink("Borrar", "Delete", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-danger"})
                                </td>
                            </tr>
                        End If

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
                            $('.datatable').dataTable();
                        });
                    </script>
                End Section
