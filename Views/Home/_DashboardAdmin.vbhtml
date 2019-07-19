@ModelType MVC4Base.DashboardViewModel



<div Class="row">
    @*Actividad Reciente*@
    <div Class="col-md-7">
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
    @*Mensajes, Eventos, Fotos*@
    <div Class="col-md-5">
        <a href="/Noticias?estado=activas">
            <div class="card" style="background-color:#76990f; color:white">
                <div class="row">
                    <div class="col-sm-4 p-t p-b text-center">
                        <i class="icon-globe" style="font-size:8em"></i>
                    </div>
                    <div class="col-sm-3 p-t p-b text-right">
                        <span style="font-size:5em">@Model.Noticias</span>
                    </div>
                    <div class="col-sm-5 p-t p-b text-left" style="font-size:28px">
                        <div class="row" style="margin-bottom: -10px;margin-top: 15px">NOTICIAS</div>
                        <div class="row" style="font-size:20px">ACTIVAS</div>
                    </div>
                </div>
            </div>
        </a>
        <a href="/Capacitacion/">
            <div class="card" style="background-color: #477628; color: white">
                <div class="row">
                    <div class="col-sm-4 p-t p-b text-center">
                        <i class="icon-graduation" style="font-size:7em"></i>
                    </div>
                    <div class="col-sm-3 p-t p-b text-right">
                        <span style="font-size:5em">@Model.Capacitaciones</span>
                    </div>
                    <div class="col-sm-5 p-t p-b text-left">
                        <div class="row" style="margin-bottom: -10px; margin-top: 15px; font-size:28px">CURSOS</div>
                        <div class="row" style="font-size:20px">ACTIVOS</div>
                    </div>
                </div>
            </div>
        </a>
        <div id="img_display" style="height: 233px"></div>
    </div>
</div>

@*Mensaje Masivo*@
<div Class="row m-t p-t bg-bluegrey">
    <div Class="form-group">
        <div Class="col-sm-12">
            <Label Class="col-sm-2 control-label" style="color:white; text-align:right">Enviar mensaje para todos los promotores del grupo</Label>
            <Button type="button" Class="btn btn-success" data-toggle="dropdown" aria-expanded="false" id="grupo">
                <span id="textoGrupo"> Grupo</span>
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
        <div class="col-sm-10 col-sm-offset-2">
            <div class="input-group m-b">
                <input type="text" class="form-control br0" id="mensaje_notificacion">
                <span class="input-group-btn">
                    <button class="btn btn-primary" type="button" id="notifica_envio">Enviar</button>
                </span>
            </div>
        </div>
    </div>
</div>