@imports MVC4Base
@ModelType MVC4Base.webpages_Roles

@Code
    ViewData("Title") = "Edit Rol"
    Dim roles As webpages_Roles() = ViewBag.roles
    Dim acciones As webpages_Acciones() = ViewBag.acciones
End Code

<div id="page-content">
    <div class="panel panel-bordered-success">
        @Using Html.BeginForm()
            @Html.AntiForgeryToken()
            @Html.ValidationSummary(True)
            @<div class="panel-heading">
                <div class="panel-control hidden-xs">
                    <a href="@Url.Action("Index")" class="btn btn-default btn-labeled fa fa-list-alt">Consulta</a>
                </div>
                <h3 class="panel-title">@ViewData("Title")</h3>
            </div>
            @<div class="panel-body">
                <div class="row">
                    <div class="col-xs-2">
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.RoleId, New With {.class = "control-label"})
                            @Html.TextBoxFor(Function(model) model.RoleId, New With {.class = "form-control", .readonly = ""})
                            @Html.ValidationMessageFor(Function(model) model.RoleId)
                        </div>
                    </div>

                    <div class="col-xs-4">
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.RoleName, New With {.class = "control-label"})
                            @Html.TextBoxFor(Function(model) model.RoleName, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(model) model.RoleName)
                        </div>
                    </div>

                    <div class="col-xs-6">
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Description, New With {.class = "control-label"})
                            @Html.TextBoxFor(Function(model) model.Description, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(model) model.Description)
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <label>Admin roles</label>
                            <div class="row">
                                @For Each rol As webpages_Roles In roles
                                Dim activo As Boolean = Model.webpages_RolesSub.Any(Function(r) r.RoleId = rol.RoleId)
                                    @<div class="col-xs-12 col-sm-6">
                                        <label class="form-checkbox form-icon form-text form-control @(If(activo, "active", ""))">
                                            <input type="checkbox" name="roles" value="@rol.RoleId" @(If(activo, "checked=checked", ""))>
                                            @rol.Description
                                        </label>
                                    </div>
                                Next
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <label>Acciones</label>
                            <div class="row">
                                @For Each accion As webpages_Acciones In acciones
                                Dim activo As Boolean = Model.webpages_Acciones.Any(Function(r) r.idAccion = accion.idAccion)
                                    @<div class="col-xs-12 col-sm-6">
                                        <label class="form-checkbox form-icon form-text form-control">
                                            <input type="checkbox" name="acciones" value="@accion.idAccion" @(If(activo, "checked=checked", ""))>
                                            @accion.Nombre
                                        </label>
                                    </div>
                                Next
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            @<div class="panel-footer">
                <div class="row">
                    <div class="col-xs-6 text-left visible-xs">
                        <a href="@Url.Action("Index")" class="btn btn-default btn-labeled fa fa-list-alt">Consulta</a>
                    </div>
                    <div class="col-xs-6 col-sm-12 text-right">
                        <button class="btn btn-success btn-labeled fa fa-save" type="submit">Guardar</button>
                    </div>
                </div>
            </div>
        End Using
    </div>
</div>

@Section Scripts
@Scripts.Render("~/bundles/jqueryval")
End Section
