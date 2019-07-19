@Modeltype MVC4Base.LoginModel
@Imports MvcFlash.Core.Extensions

@Code
    ViewData("Title") = "Recuperar contraseña"
    Layout = "~/Views/Shared/_LayoutEmpty.vbhtml"
End Code

<div class="pageload">
    <div class="pageload-inner">
        <div class="sk-rotating-plane"></div>
    </div>
</div>
<!-- /page loading spinner -->
<div class="app signin usersession">
    <div class="session-wrapper">
        <div class="page-height-o row-equal align-middle">
            <div class="column">
                <div class="card bg-white no-border">
                    <div class="card-block">
                        @Using Html.BeginForm("Reset", "Account", New With {.ReturnUrl = ViewBag.ReturnUrl}, FormMethod.Post, Nothing)
                            @Html.AntiForgeryToken()
                            @<div class="text-center m-b">
                                 <img src="/Images/logonm.png" alt="Fuel Logo" class="brand-icon" style="width: 120px;">
                                <p class="m-t">Escribe tu correo electrónico para recuperar la contraseña.@Html.ValidationSummary(True)</p>
                                <p>@Html.Flash()</p>
                            </div>
                            @<div class="row m-t">
                                    <label class="col-xs-2" style="text-align:center"><i class="icon-envelope"></i></label>
                                    <div class="col-xs-10">
                                        @Html.TextBoxFor(Function(m) m.UserName, New With {.class = "form-control input-lg", .placeholder = "Correo Electrónico...", .type = "email"})
                                    </div>   
                             </div>
                            @<button class="btn btn-success btn-block btn-lg m-b m-t" type="submit">Resetear Contraseña</button>
                        End Using
                    </div>
                    <a href="@Url.Action("Login", "Account")" class="bottom-link">Iniciar sesión</a>
                </div>
            </div>
        </div>
    </div>
</div>

@Section styles
   
    <style>
        i {
            display: inline-block;
            border-radius: 20px;
            box-shadow: 0px 0px 1px #888;
            padding: 5px 6px;
            border: 2px solid;
            margin-top: 7px;
        }
    </style>

End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
