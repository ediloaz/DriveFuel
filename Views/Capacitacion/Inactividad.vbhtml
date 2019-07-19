@ModelType IEnumerable(Of MVC4Base.InactividadViewModel)

@Code
    ViewData("Title") = "Inactividad en Capacitación"
    ViewData("Descripcion") = "¿Qué usuario no ha visualizado el contenido de las capacitaciones activas?"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Actividad Reciente", "Visitas", "Capacitacion", New With {.class = "btn btn-info"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-condensed table-striped datatable m-b-0" id="tablaNoticias">
                <thead>
                    <tr>
                        @*<th>@Html.DisplayNameFor(Function(model) model.idCapacitacion)</th>*@
                        <th>@Html.DisplayNameFor(Function(model) model.Capacitacion)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Tema)</th>
                        <th>@Html.DisplayNameFor(Function(model) model.Usuario)</th>
                    </tr>
                </thead>
                <tbody>
                    @Code
                        Dim l_idCapacitaciones = Model.Select(Function(x) x.idCapacitacion).Distinct().ToList()
                        Dim l_idTemas = Model.Select(Function(x) x.idTema).Distinct().ToList()
                    End Code

                    @For Each idCapacitacion In l_idCapacitaciones
                        Dim capacitacion = Model.Where(Function(x) x.idCapacitacion = idCapacitacion).Select(Function(x) x.Capacitacion).FirstOrDefault()
                        For Each idTema In l_idTemas
                            Dim tema = Model.Where(Function(x) x.idTema = idTema).Select(Function(x) x.Tema).FirstOrDefault()
                            Dim l_usuarios = Model.Where(Function(x) x.idCapacitacion = idCapacitacion And x.idTema = idTema).Select(Function(x) x.idUsuario).Distinct.ToList()
                            If l_usuarios.Any Then
                                 @<tr>
                                    @*<td>@idCapacitacion</td>*@
                                    <td>@capacitacion</td>
                                    <td>@tema</td>
                                    <td>
                                        @For Each idUsuario In l_usuarios
                                        Dim usuario = Model.Where(Function(x) x.idUsuario = idUsuario).Select(Function(x) x.Usuario).FirstOrDefault()
                                            @<span class="label label-danger" style="margin-right:2px">
                                                @Html.ActionLink(usuario, "InactividadUsuario", "Capacitacion", New With {.id = idUsuario}, Nothing)
                                            </span>
                                        Next
                                    </td>
                                </tr>
                            End If
                            
                    Next
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



