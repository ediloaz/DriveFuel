@ModelType MVC4Base.Usuarios

@Code
    ViewData("Title") = "Activar/Desactivar Usuario"
    ViewData("Descripcion") = "Puede activar o desactivar el usuario"
End Code

<div>
    <div class="card bg-white">
        <div class="card-header">
            <div class="panel-control hidden-xs">
                <a href="@Url.Action("Index")" class="btn btn-default">Consulta</a>
            </div>
        </div>
        <div class="card-block">
            <div class="table">
                <div class="row">
                    <div class="col-xs-12 col-sm-6"><b>@Html.DisplayNameFor(Function(model) model.Nombre)</b></div>
                    <div class="col-xs-12 col-sm-6">@Html.DisplayFor(Function(model) model.Nombre)</div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-6"><b>@Html.DisplayNameFor(Function(model) model.Correo)</b></div>
                    <div class="col-xs-12 col-sm-6">@Html.DisplayFor(Function(model) model.Correo)</div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-6"><b>@Html.DisplayNameFor(Function(model) model.InformacionContacto)</b></div>
                    <div class="col-xs-12 col-sm-6">@Html.DisplayFor(Function(model) model.InformacionContacto)</div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-6"><b>@Html.DisplayNameFor(Function(model) model.CuentaActiva)</b></div>
                    <div class="col-xs-12 col-sm-6">@Html.DisplayFor(Function(model) model.CuentaActiva)</div>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <div class="row">
                <div class="col-xs-12 col-sm-6">
                    @If Not WebSecurity.IsConfirmed(Model.Correo) Then
                        @<a href="@Url.Action("ReEnviarTokenActivacion", New With {.id = Model.idUsuario})" class="btn btn-primary">Enviar token de confirmación</a>
                    End If
                </div>
                <div class="col-xs-5 visible-xs">
                    <a href="@Url.Action("Index")" class="btn btn-default btn-labeled fa fa-list-alt">Consulta</a>
                </div>
                @If WebSecurity.IsConfirmed(Model.Correo) Then
                @<div class="col-xs-7 col-sm-6 text-right">
                    @Using Html.BeginForm()
                    @Html.AntiForgeryToken()
                        @If Model.CuentaActiva Then
                    @<button type="submit" name="accion" value="desactivar" class="btn btn-warning">Desactivar</button>
                        Else
                    @<button type="submit" name="accion" value="activar" class="btn btn-success">Activar</button>
                        End If
                    End Using
                </div>
                End If
            </div>
        </div>
    </div>
</div>
