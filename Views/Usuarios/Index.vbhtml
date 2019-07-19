@ModelType IEnumerable(Of MVC4Base.Usuarios)

@Code
    ViewData("Title") = "Usuarios"
    ViewData("Descripcion") = "Crea, visualiza y edita los usuarios de la aplicación"
End Code

<div id="">
    <div class="card bg-white">
        <div class="card-header">
            @*<a id="btnNuevoUsuario" href="@Url.Action("Create")" class="btn btn-success btn-labeled fa fa-plus">Nuevo</a>*@
            @Html.ActionLink("Nuevo Usuario", "Create", Nothing, New With {.class = "btn btn-success"})
        </div>
        <div class="card-block">
            <table id="tablaDatos" class="table table-striped table-bordered display dt-responsive nowrap" cellspacing="0" width="100%">
                <thead>
                    <tr>
                        <th class="all">
                            @Html.DisplayNameFor(Function(model) model.Nombre)
                        </th>
                        <th class="min-tablet-l">
                            @Html.DisplayNameFor(Function(model) model.Correo)
                        </th>
                        <th class="min-desktop">
                            @Html.DisplayNameFor(Function(model) model.InformacionContacto)
                        </th>
                        <th class="min-tablet">
                            @Html.DisplayNameFor(Function(model) model.CuentaActiva)
                        </th>
                        <th class="min-desktop" data-class="table-actions"></th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr>
                            <td>
                                @Html.DisplayFor(Function(modelItem) currentItem.Nombre)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) currentItem.Correo)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) currentItem.InformacionContacto)
                            </td>
                            <td>
                                @Html.DisplayFor(Function(modelItem) currentItem.CuentaActiva)
                            </td>
                            <td>
                                @If Not currentItem.CuentaActiva Then
                                    @<span class="glyphicon glyphicon-user" title="Cuenta inactiva"></span>
                                End If
                                <a href="@Url.Action("Details", New With {.id = currentItem.idUsuario})" class="icon-action btn-xl add-tooltip" data-toggle="tooltip" data-original-title="Detalles"><i class="fa fa-search text-primary"></i></a>
                                <a href="@Url.Action("Edit", New With {.id = currentItem.idUsuario})" class="icon-action btn-xl add-tooltip" data-toggle="tooltip" data-original-title="Editar"><i class="fa fa-edit text-primary"></i></a>
                                <a href="@Url.Action("Edit", "UsuariosAcceso", New With {.id = currentItem.idUsuario})" class="icon-action btn-xl add-tooltip" data-toggle="tooltip" data-original-title="Editar Permisos"><i class="fa fa-key text-primary"></i></a>
                                @If Not currentItem.CuentaActiva Then
                                    @<a href="@Url.Action("Delete", New With {.id = currentItem.idUsuario})" class="icon-action btn-xl add-tooltip" data-toggle="tooltip" data-original-title="Desactivar"><i class="fa fa-ban text-danger"></i></a>
                                Else
                                    @<a href="@Url.Action("Delete", New With {.id = currentItem.idUsuario})" class="icon-action btn-xl add-tooltip" data-toggle="tooltip" data-original-title="Activar"><i class="fa fa-check-circle text-success"></i></a>
                                End If
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>

        </div>
    </div>
</div>

@Section styles
    @Styles.Render("~/Content/datatable")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/datatable")

    <script type="text/javascript">
        $(document).ready(function() {
            $("#tablaDatos").DataTable({
                responsive: true
            });
        });
    </script>
End Section