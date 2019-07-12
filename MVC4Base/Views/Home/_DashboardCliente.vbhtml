@ModelType MVC4Base.DashboardViewModel


<center class="m-t p-t">
    <h2> <b>Rutas</b></h2>
    <span>Estas son las rutas para ti</span>
</center>
@*Rutas*@
<div class="row m-t p-t">
    <div Class="row">
        <div Class="col-sm-12 col-md-12">
            <div class="table-responsive">
                <table class="table table-bordered datatable2 table-striped m-b-0">
                    <thead  class="text-white bold" style="background:#01A93A">
                        <tr>
                            <th>Cliente</th>
                            <th>Marca</th>
                            <th>Ruta</th>
                        </tr>
                    </thead>
                    @*<tfoot>
                            <tr>
                                <th>Cliente</th>
                                <th>Marca</th>
                                <th>Ruta</th>
                            </tr>
                        </tfoot>*@
                    <tbody>
                        @For Each item In Model.Rutas
                            Dim currentItem = item
                            @<tr>
                                <td>
                                    @Html.DisplayFor(Function(modelItem) currentItem.Cliente.NombreCliente)
                                </td>
                                <td>
                                    @Html.DisplayFor(Function(modelItem) currentItem.Producto.NombreProducto)
                                </td>
                                <td>
                                    <a href="@Url.Action("Checks", "Rutas", New With {.id = currentItem.idRuta})" class="btn btn-dark">@currentItem.Descripcion</a>
                                </td>
                            </tr>
                        Next
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<center class="m-t p-t">
    <h2> <b>Asistencia</b></h2>
    <span>Usuarios que han realizado su check-in</span>
</center>

@*Asistencias*@
<div class="row m-t p-t">
    <div Class="row">
        <div Class="col-sm-12 col-md-12">
            <div class="table-responsive">
                <table class="table table-bordered datatable table-striped m-b-0">
                    <thead class="text-white bold" style="background:#01A93A">
                        <tr>
                            <th style="width:45%">Promotor</th>
                            <th style="width:45%">Ruta</th>
                            <th style="width:10%">CheckIn</th>
                        </tr>
                    </thead>
                    @*<tfoot>
                            <tr>
                                <th>Cliente</th>
                                <th>Marca</th>
                                <th>Ruta</th>
                            </tr>
                        </tfoot>*@
                    <tbody>
                        @For Each item In Model.CheckIns
                            Dim currentItem = item
                            @<tr>
                                <td style="width:45%">
                                     @Html.DisplayFor(Function(modelItem) currentItem.Usuarios.Nombre)
                                 </td>
                                <td style="width:45%">
                                     @Html.DisplayFor(Function(modelItem) currentItem.RutaCheckpoint.Ruta.Descripcion)
                                 </td>
                                <td style="width:10%">
                                    <center>
                                        <i class="icon-check text-green"></i>
                                    </center>
                                </td>
                            </tr>
                        Next
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<center class="m-t p-t">
    <h2> <b>Ausencias</b></h2>
    <span>Usuarios que <b>no</b> han realizado su check-in</span>
</center>

@*Ausencias*@
<div class="row m-t p-t">
    <div Class="row">
        <div Class="col-sm-12 col-md-12">
            <div class="table-responsive">
                <table class="table table-bordered datatable table-striped m-b-0">
                    <thead  class="text-white bold" style="background:#FF3145">
                        <tr>
                            <th style="width:45%">Promotor</th>
                            <th style="width:45%">Ruta</th>
                            <th style="width:10%">CheckIn</th>
                        </tr>
                    </thead>
                    <tbody>
                        @For Each item In Model.Ausencias

                            Dim currentItem = item
                            @<tr>
                                <td style="width:45%">
                                    @Html.DisplayFor(Function(modelItem) currentItem.usuario)
                                </td>
                                <td style="width:45%">
                                    @Html.DisplayFor(Function(modelItem) currentItem.ruta)
                                </td>
                                <td style="width:10%">
                                    <center>
                                        <i class="glyphicon glyphicon-remove-circle text-red"></i>
                                    </center>
                                    
                                </td>
                            </tr>
                        Next
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<center class="m-t p-t">
</center>


<div Class="row m-t p-t">
    <div Class="row">
        @*Actividad Reciente*@
        <div Class="col-sm-12 col-md-7">
            <div Class="table-responsive">
                <Table Class="table m-b-0">
                    <tbody>
                        @For Each actividad In Model.ActividadReciente
                            @<tr class="row fa-hover">
                                <td style="text-align:center">
                                    @If actividad.Actividad = MVC4Base.EnumActividad.CheckIn Then
                                        @<i class="icon-pointer round-i" style="background-color: #4545e0"></i>
                                    ElseIf actividad.Actividad = MVC4Base.EnumActividad.CheckOut Then
                                        @<i class="icon-pointer round-i" style="background-color: #da10a5"></i>
                                    ElseIf actividad.Actividad = MVC4Base.EnumActividad.Upload Then
                                        @<i class="icon-cloud-upload round-i" style="background-color: #29aab7"></i>
                                    ElseIf actividad.Actividad = MVC4Base.EnumActividad.Capacitacion Then
                                        @<i class="icon-graduation round-i" style="background-color: #da10a5"></i>
                                    End If
                                </td>
                                <td>
                                    <span class="bold">@actividad.Persona </span>
                                    @actividad.Descripcion
                                </td>
                                <td style="text-align:right; font-size:12px">@actividad.Tiempo h</td>
                            </tr>
                        Next
                    </tbody>
                </Table>
            </div>
        </div>
        @*Notificacion masiva*@
        <div Class="col-sm-12 col-md-5 bg-white border-danger">
            <div Class="form-group">
                <div Class="col-sm-12 p-t m-t-md">
                    <Label Class="control-label" style="">Enviar notificación a usuarios</Label>
                </div>
                <div Class="col-sm-12 p-t">

                    <Button type="button" Class="btn btn-default" data-toggle="dropdown" aria-expanded="false" id="grupo">
                        <span id="textoGrupo"> Selecciona grupo de usuarios </span>
                        <span Class="caret"></span>
                    </Button>
                    <ul Class="dropdown-menu">
                        @For Each _grupo In Model.Grupos
                            @<li class="opcionesGrupo" data-item="@_grupo.Descripcion" data-idgrupo="@_grupo.idGrupo">
                                <a href="javascript:;">@_grupo.Descripcion</a>
                            </li>
                        Next
                    </ul>
                </div>
                <div class="col-sm-12 p-t">
                    <textarea class="form-control" rows="5" id="mensaje_notificacion"></textarea>
                </div>
                <div class="col-sm-12 p-t m-b-md">
                    <center>
                        <button class="btn btn-success" style="width:80%" type="button" id="notifica_envio">Enviar</button>
                    </center>
                </div>
            </div>
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