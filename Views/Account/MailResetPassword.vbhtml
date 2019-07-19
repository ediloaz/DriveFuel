@Code
    Layout = "~/Views/Shared/_LayoutMail.vbhtml"
    ViewBag.Title = "Recuperación de contraseña"
End Code

@ViewData("nombreUser"), hemos recibido una petición para restablecer la contraseña de su cuenta.
<br/>
Para realizar esta acción haga clic en el siguiente enlace:
<br/><br/>
<a href="@ViewData("linkToken")">@ViewData("linkToken")</a>
<br/><br/>
Si el enlace no funciona al hacer clic en él, puede copiarlo en la ventana de su navegador o teclearlo directamente.
<br/><br/>Si usted no realizó esta petición puede ignorar este correo.