@Code
    Layout = "~/Views/Shared/_LayoutMail.vbhtml"
    ViewBag.Title = "Confirmación de cuenta"
End Code

<p>@ViewData("nombreUser"), se ha dado de alta una cuenta con esta dirección de correo en el portal de recepción de facturas.</p><br/>
Para activar su cuenta, haga clic en el siguiente enlace: <br/>
<a href="@ViewData("linkVericationAccount")">@ViewData("linkVericationAccount")</a><br/><br/>
Si el enlace no funciona al hacer clic en él, puede copiarlo en la ventana del navegador o teclearlo directamente.<br/>
Una vez que haya activado su cuenta, podrá ingresar utilizando los siguientes datos: <br/><br/>
Usuario: <b>@ViewData("UsuarioMail")</b><br/>
Contraseña generada: <b>@ViewData("contrasenia")</b><br/><br/>
Es importante que active su cuenta antes de @ViewData("diasActivar") días, después de ese periodo el enlacé será inválido y tendrá que solicitar que le sea enviado un nuevo enlace.</b></p>