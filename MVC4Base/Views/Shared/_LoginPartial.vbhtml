<!--User dropdown-->
<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
<li id="dropdown-user" class="dropdown">
    <a href="#" data-toggle="dropdown" class="dropdown-toggle text-right">
        <div class="username hidden-xs">@User.Identity.Name</div>
    </a>

    <div class="dropdown-menu dropdown-menu-md dropdown-menu-right with-arrow panel-default">
        <!-- Dropdown footer -->
        <div class="pad-all text-right">
            <a href="@Url.Action("Manage", "Account")" class="btn btn-primary">
                <i class="fa fa-user-secret"></i> Cambiar Contraseña
            </a>
            @Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm"})
                @Html.AntiForgeryToken()
                @<a href="javascript:document.getElementById('logoutForm').submit()" class="btn btn-default">
                    <i class="fa fa-sign-out fa-fw"></i> Salir
                </a>
            End Using
        </div>
    </div>
</li>
<!--~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~-->
<!--End user dropdown-->