@ModelType MVC4Base.LocalPasswordModel

<p class="pad-btm">Cambio de contraseña</p>
@Using Html.BeginForm("Manage", "Account")
    @Html.AntiForgeryToken()
    @Html.ValidationSummary()

    @<div class="form-group">
        <div class="input-group">
            <div class="input-group-addon"><i class="fa fa-certificate"></i></div>
            @Html.PasswordFor(Function(m) m.OldPassword, New With {.class = "form-control", .placeholder = "Escriba su Contraseña Actual"})
            @*@Html.ValidationMessageFor(Function(m) m.OldPassword)*@
        </div>
    </div>

    @<div class="form-group">
        <div class="input-group">
            <div class="input-group-addon"><i class="fa fa-asterisk"></i></div>
            @Html.PasswordFor(Function(m) m.NewPassword, New With {.class = "form-control", .placeholder = "Escriba una nueva Contraseña"})
            @*@Html.ValidationMessageFor(Function(m) m.NewPassword)*@
        </div>
    </div>

    @<div class="form-group">
        <div class="input-group">
            <div class="input-group-addon"><i class="fa fa-asterisk"></i></div>
            @Html.PasswordFor(Function(m) m.ConfirmPassword, New With {.class = "form-control", .placeholder = "Confirme la nueva Contraseña"})
            @*@Html.ValidationMessageFor(Function(m) m.ConfirmPassword)*@
        </div>
    </div>
           
    @<div class="form-group text-right">
        <button class="btn btn-success text-uppercase" type="submit">Cambiar contraseña</button>
    </div>
        
        
    
End Using
