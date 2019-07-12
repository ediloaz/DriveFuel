@imports MVC4Base
@ModelType MVC4Base.Usuarios

@Code
    ViewData("Title") = "Nuevo Usuario"
    Dim roles As webpages_Roles() = ViewBag.rolesUser
End Code
<div id="">
    <div class="card bg-white">
        @Using Html.BeginForm()
        @Html.AntiForgeryToken()
        @Html.ValidationSummary(True)
            @<div class="card-header">
                <div class="panel-control hidden-xs">
                    <a href="@Url.Action("Index")" class="btn btn-default">Consulta</a>
                </div>
            </div>
            @<div class="card-block">
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Nombre, New With {.class = "control-label"})
                            @Html.TextBoxFor(Function(model) model.Nombre, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(model) model.Nombre)
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Correo, New With {.class = "control-label"})
                            @Html.TextBoxFor(Function(model) model.Correo, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(model) model.Correo)
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.InformacionContacto, New With {.class = "control-label"})
                            @Html.TextAreaFor(Function(model) model.InformacionContacto, New With {.class = "form-control"})
                            @Html.ValidationMessageFor(Function(model) model.InformacionContacto)
                        </div>
                    </div>
                </div>
                 <div class="row">
                     <div class="col-xs-12 col-sm-6">
                         <div class="form-group">
                             <label>Perfil</label>
                             <div class="row">
                                 @For Each rol As webpages_Roles In ViewBag.rolesList
                                 Dim activo As Boolean = Roles.Any(Function(r) r.RoleName = rol.RoleName)
                                     @<div class="col-xs-12 col-sm-6">
                                         <label class="form-checkbox form-icon form-text form-control @(If(activo, "active", ""))">
                                             <input type="checkbox" name="rolesSelected" value="@rol.RoleName" @(If(activo, "checked=checked", ""))>
                                             @rol.Description
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
                    <div class="col-xs-6 text-left">
                        <a href="@Url.Action("Index")" class="btn btn-default btn-labeled fa fa-list-alt visible-xs">Consulta</a>
                    </div>
                    <div class="col-xs-6 text-right">
                        <button class="btn btn-success" type="submit">Guardar</button>
                    </div>
                </div>
            </div>
        End Using
    </div>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
