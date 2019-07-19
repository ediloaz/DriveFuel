@ModelType MVC4Base.LocalPasswordModel
@Code
    ViewData("Title") = "Cambiar contraseña"
End Code

<div id="password-change-container" class="cls-container">
    <div class="cls-content">
        <div class="col-md-offset-4 col-md-4">
            <div class="card card-block bg-white">
                @Html.Partial("_ChangePasswordPartial")
            </div>
        </div>
    </div>
</div>
