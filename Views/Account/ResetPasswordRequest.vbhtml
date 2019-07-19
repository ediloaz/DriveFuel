@Modeltype MVC4Base.LocalPasswordModel
@Imports MvcFlash.Core.Extensions

@Code
    ViewData("Title") = "Recuperar contraseña"
    Layout = "~/Views/Shared/_LayoutEmpty.vbhtml"
End Code

<div class="app signin usersession">
    <div class="session-wrapper">
        <div class="page-height-o row-equal align-middle">
            <div class="column">
                <div class="card bg-white no-border">
                    <div class="card-block">
                        
                            <div class="text-center m-b">
                                <img src="/Images/logonm.png" alt="Fuel Logo" class="brand-icon" style="width: 120px;">
                                <p class="m-t">Para terminar el proceso capture su nueva contraseña @Html.ValidationSummary(True)</p>
                                <p>@Html.Flash()</p>
                            </div>
                            @Using Html.BeginForm(Nothing, Nothing, New With {.ReturnUrl = ViewBag.ReturnUrl}, FormMethod.Post, Nothing)
                                @Html.AntiForgeryToken()
                                @<div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-addon"><i class="fa fa-asterisk"></i></div>
                                        @Html.PasswordFor(Function(m) m.NewPassword, New With {.class = "form-control", .placeholder = "Escriba su nueva contraseña"})
                                        @*@Html.ValidationMessageFor(Function(m) m.NewPassword)*@
                                    </div>
                                </div>
                                @<div class="form-group">
                                    <div class="input-group">
                                        <div class="input-group-addon"><i class="fa fa-asterisk"></i></div>
                                        @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control", .placeholder = "Confirme su nueva contraseña"})
                                        @*@Html.ValidationMessageFor(Function(m) m.ConfirmPassword)*@
                                    </div>
                                </div>
                                @<div class="row">
                                    <div class="col-xs-6">
                                        <div class="form-group text-right">
                                            <button class="btn btn-success text-uppercase" type="submit">Confirmar</button>
                                        </div>
                                    </div>
                                </div>
                            End Using
                    </div>
                    <a href="@Url.Action("Login", "Account")" class="bottom-link">Iniciar sesión</a>
                </div>
            </div>
        </div>
    </div>
</div>



@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
