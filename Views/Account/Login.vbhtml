@ModelType MVC4Base.LoginModel
@Imports MvcFlash.Core.Extensions

@Code
    Layout = "~/Views/Shared/_LayoutEmpty.vbhtml"
    ViewBag.Title = "Iniciar sesión"
End Code

  <!-- page loading spinner -->
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
                <div class="card" style="border: none;box-shadow: none;margin-top:4em">
                    <div class="card-block">
                        @Using Html.BeginForm("Login", "Account", New With {.ReturnUrl = ViewBag.ReturnUrl}, FormMethod.Post, Nothing)
                            @Html.AntiForgeryToken()
                            @<div class="text-center m-b">
                                @*<h4 class="text-uppercase">FUEL</h4>*@
                                <img src="/Images/logonm.png" alt="Fuel Logo" class="brand-icon" style="width: 120px;">
                                @*<p>Inicio de sesión @Html.ValidationSummary(True)</p>*@
                                <p>@Html.Flash()</p>
                            </div>
                            @<div >
                                <div class="row m-t-lg">
                                    <label class="col-xs-2" style="text-align:center"><i class="icon-user"></i></label>
                                    <div class="col-xs-10">
                                        @Html.TextBoxFor(Function(m) m.UserName, New With {.class = "form-control col-xs-10", .placeholder = "Usuario...", .type = "email"})
                                    </div>   
                                </div>
                                 <div class="row m-t-md">
                                     <label class="col-xs-2" style="text-align:center"><i class="icon-lock"></i></label>
                                     <div class="col-xs-10">
                                         @Html.PasswordFor(Function(m) m.Password, New With {.class = "form-control col-xs-10", .placeholder = "Password..."})
                                     </div>
                                 </div>
                                                   
                            </div>
                            @<div class="cs-checkbox" style="margin:1em 0 1em 0; float:right">
                                <input type="checkbox" name="RememberMe" id="r1"> 
                                 <label for="r1">Recordar Cuenta</label>
                            </div>
                            @<button class="btn btn-success btn-block btn-lg m-b" type="submit">Iniciar sesión</button>
                        End Using
                    </div>
                    <div class="row" style="margin-top:-2em">
                        <a href="@Url.Action("Reset", "Account")" class="bottom-link" style="margin: 0 auto; display:block; text-align: center; position:relative">¿Olvidaste tu contraseña?</a>
                    </div>                   
                </div>
            </div>
        </div>
    </div>
</div>


@Section styles
    @Styles.Render("~/Content/login ")
    
    <style>
        i {
            display: inline-block;
            border-radius: 20px;
            box-shadow: 0px 0px 1px #888;
            padding: 5px 6px;
            border: 2px solid;
          
        }
    </style>
    
End Section

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
