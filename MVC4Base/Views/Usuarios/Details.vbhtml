@imports MVC4Base
@ModelType MVC4Base.Usuarios

@Code
    ViewData("Title") = "Detalles Usuario"
End Code

<div>
    <div class="card bg-white">
        <div class="card-header">
            <div class="panel-control hidden-xs">
                <a href="@Url.Action("Index")" class="btn btn-default">Consulta</a>
            </div>
        </div>
        <div class="panel-body">
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
                    <div class="col-xs-12 col-sm-6"><b>@Html.DisplayName("Roles")</b></div>
                    <div class="col-xs-12 col-sm-6">
                        <ul>
                            @For Each rol As webpages_Roles In Model.webpages_Roles
                                @<li>@rol.Description </li>
                            Next
                        </ul>
                    </div>
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
                    <div class="col-xs-6 text-left">
                        <a href="@Url.Action("Index")" class="btn btn-default visible-xs">Consulta</a>
                    </div>
                    <div class="col-xs-6 text-right">
                        <a href="@Url.Action("Edit", New With {.id = Model.idUsuario})" class="btn btn-primary">Editar</a>
                    </div>
                </div>
        </div>
    </div>
</div>