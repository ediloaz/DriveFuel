Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Transactions
Imports System.Web.Routing
Imports DotNetOpenAuth.AspNet
Imports Microsoft.Web.WebPages.OAuth
Imports WebMatrix.WebData
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions

<Authorize()> _
Public Class AccountController
    Inherits System.Web.Mvc.Controller
    Private db As New BaseEntities

    '
    ' GET: /Account/Login

    <AllowAnonymous()> _
    Public Function Login(ByVal returnUrl As String) As ActionResult
        If WebSecurity.IsAuthenticated Then
            Return RedirectToAction("Index", "Home")
        End If
        ViewData("ReturnUrl") = returnUrl
        Return View()
    End Function

    '
    ' POST: /Account/Login

    <HttpPost()> _
    <AllowAnonymous()> _
    <ValidateAntiForgeryToken()> _
    Public Function Login(ByVal model As LoginModel, ByVal returnUrl As String) As ActionResult
        If Not VerificarIntentosFallidos(model) Then
            If ModelState.IsValid AndAlso WebSecurity.Login(model.UserName, model.Password, persistCookie:=model.RememberMe) Then
                SetCurrentUser(model.UserName)
                Return RedirectToLocal(returnUrl)
            Else
                If WebSecurity.IsConfirmed(model.UserName) Then
                    ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.")
                Else
                    Dim usuarioExiste As Boolean = (From u In db.Usuarios Where u.Correo = model.UserName).Any
                    If usuarioExiste Then
                        ModelState.AddModelError("", "El usuario no ha confirmado su cuenta.")
                    Else
                        ModelState.AddModelError("", "No se encontro cuenta con ese usuario.")
                    End If
                End If
            End If
        End If
        Return View(model)
    End Function

    '
    ' POST: /Account/LogOff

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function LogOff() As ActionResult
        WebSecurity.Logout()

        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' GET: /Account/Manage

    Public Function Manage(ByVal message As ManageMessageId?) As ActionResult
        Dim mensaje As String =
            If(message = ManageMessageId.ChangePasswordSuccess, "La contraseña se ha cambiado.", _
                If(message = ManageMessageId.SetPasswordSuccess, "Su contraseña se ha establecido.", _
                    If(message = ManageMessageId.RemoveLoginSuccess, "El inicio de sesión externo se ha quitado.", _
                        "")))
        If mensaje.Length > 0 Then
            Flash.Instance.Success("", mensaje)
            Return RedirectToLocal("")
        End If

        ViewData("HasLocalPassword") = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name))
        ViewData("ReturnUrl") = Url.Action("Manage")
        Return View()
    End Function

    '
    ' POST: /Account/Manage
    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function Manage(ByVal model As LocalPasswordModel) As ActionResult
        'Dim hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name))
        'ViewData("HasLocalPassword") = hasLocalAccount
        ViewData("ReturnUrl") = Url.Action("Manage")

        If model.NewPassword <> model.ConfirmPassword Then
            Flash.Instance.Warning("", "La nueva contraseña y la contraseña de confirmación no coinciden.")
            Return View(model)
        End If

        If model.NewPassword.Length < 6 Then
            Flash.Instance.Warning("", "El número de caracteres del nuevo password debe ser de al menos 6.")
            Return View(model)
        End If

        'If hasLocalAccount Then
        If ModelState.IsValid Then
            ' ChangePassword iniciará una excepción en lugar de devolver false en determinados escenarios de error.
            Dim changePasswordSucceeded As Boolean
            Try
                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword)
            Catch e As Exception
                changePasswordSucceeded = False
            End Try
            If changePasswordSucceeded Then
                Return RedirectToAction("Manage", New With {.Message = ManageMessageId.ChangePasswordSuccess})
            Else
                ModelState.AddModelError("", "La contraseña actual es incorrecta o la nueva contraseña no es válida.")
                Flash.Instance.Warning("", "La contraseña actual que escribió es incorrecta.")
            End If
        End If
        'Else
        ' '' El usuario no dispone de contraseña local, por lo que debe quitar todos los errores de validación generados por un
        ' '' campo OldPassword
        ''Dim state = ModelState("OldPassword")
        ''If sta3te IsNot Nothing Then
        ''    state.Errors.Clear()
        ''End If

        ''If ModelState.IsValid Then
        ''    Try
        ''        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword)
        ''        Return RedirectToAction("Manage", New With {.Message = ManageMessageId.SetPasswordSuccess})
        ''    Catch e As Exception
        ''        ModelState.AddModelError("", String.Format("No se puede crear una cuenta local. Es posible que ya exista una cuenta con el nombre ""{0}"".", User.Identity.Name))
        ''    End Try
        ''End If
        'End If
        ' Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
        Return View(model)
    End Function

    Public Function Edit() As ActionResult
        Dim usuario As Usuarios = db.Usuarios.Find(WebSecurity.CurrentUserId)
        If IsNothing(usuario) Then
            Return HttpNotFound()
        End If
        Dim model As New UsuarioInfoViewModel
        model.Nombre = usuario.Nombre
        model.Correo = usuario.Correo
        model.InformacionContacto = usuario.InformacionContacto

        Return View(model)
    End Function

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal model As UsuarioInfoViewModel) As ActionResult
        Dim usuario As Usuarios = db.Usuarios.Find(WebSecurity.CurrentUserId)

        If ModelState.IsValid Then
            If usuario.Correo.ToUpper <> model.Correo.ToUpper And WebSecurity.UserExists(model.Correo) Then
                'ModelState.AddModelError("", UsuariosController.ErrorCodeToString(MembershipCreateStatus.DuplicateUserName))
            Else
                usuario.Correo = model.Correo
            End If

            usuario.Nombre = model.Nombre
            usuario.Correo = model.Correo
            usuario.InformacionContacto = model.InformacionContacto
            db.SaveChanges()
            Flash.Instance.Success("", "Se ha actualizado correctamente su información.")
            Return RedirectToAction("UserStats")
        End If

        Return View(model)
    End Function

    Public Function Menu() As PartialViewResult
        Using db As New BaseEntities
            Return PartialView()
        End Using
    End Function

    Public Sub SetCurrentUser(Optional ByVal correo As String = Nothing)
        Using db As New BaseEntities
            Dim usuarioBuscado As Usuarios = (From u In db.Usuarios Where u.Correo = WebSecurity.CurrentUserName Or u.Correo = correo).SingleOrDefault
            If usuarioBuscado IsNot Nothing Then
                System.Web.HttpContext.Current.Session("Nombre") = usuarioBuscado.Nombre
            Else
                System.Web.HttpContext.Current.Session("Nombre") = Nothing
            End If
        End Using
    End Sub

    ' GET: /Account/Reset

    <AllowAnonymous()> _
    Public Function Reset() As ActionResult
        Return View()
    End Function

    ' GET: /Account/Reset/
    <HttpPost()> _
    <AllowAnonymous()> _
    Function Reset(ByVal model As LoginModel) As ActionResult
        Dim db As New BaseEntities
        Dim respuestaEnvio As RespuestaControlGeneral
        If Mail.IsValidEmail(model.UserName.Trim()) Then
            Dim usuarioBuscado As Usuarios = (From u In db.Usuarios Where u.Correo = model.UserName.Trim()).SingleOrDefault
            If usuarioBuscado IsNot Nothing Then
                If WebSecurity.IsConfirmed(usuarioBuscado.Correo) Then
                    respuestaEnvio = EnviarReseteoContrasenia(usuarioBuscado.Nombre, usuarioBuscado.Correo)
                    If respuestaEnvio.Tipo = EnumTipoRespuesta.Exito Then
                        Flash.Instance.Success("", "Se ha enviado un mensaje de correo a " + usuarioBuscado.Correo + " con un enlace para poder completar la solicitud.")
                        Return RedirectToAction("Index", "Home")
                    ElseIf respuestaEnvio.Tipo = EnumTipoRespuesta.Fracaso Then
                        Flash.Instance.Error("", respuestaEnvio.Mensaje)
                    End If
                Else
                    Flash.Instance.Error("", "La cuenta se encuentra inactiva y no podrá realizar el proceso, consulte con el administrador del sistema.")
                End If
            Else
                Flash.Instance.Warning("", "El correo " + model.UserName + " no se encuentra registrado como un usuario.")
            End If
        Else
            Flash.Instance.Info("", "No se ha escrito una dirección de correo con formato válido.")
        End If
        Return View()
    End Function


    Function EnviarReseteoContrasenia(ByVal nombreUser As String, ByVal UsuarioMail As String) As RespuestaControlGeneral
        Dim TokenReset As String = WebSecurity.GeneratePasswordResetToken(UsuarioMail, 1440)
        Dim linkToken As String = Url.Action("ResetPasswordValidate", "Account", New With {.id = TokenReset}, Me.Request.Url.Scheme)
        Dim webPage As String = (From w In db.Parametros Where w.Clave = "OFICIAL_WEB_PAGE").First.Valor

        Dim viewData As New ViewDataDictionary
        viewData.Add("nombreUser", nombreUser)
        viewData.Add("linkToken", linkToken)
        viewData.Add("webPage", webPage)

        Dim html As String = Util.RenderPartialViewToString(ControllerContext, "MailResetPassword", viewData)

        Return Mail.SendMail("Portal Fuel: Reestablecer contraseña.", {html}, {UsuarioMail}, {}, True)

    End Function

    <AllowAnonymous()> _
    Function ResetPasswordValidate(ByVal id As String) As ActionResult
        Dim db As New BaseEntities
        Dim member As webpages_Membership = (From w In db.webpages_Membership Where w.PasswordVerificationToken = id).SingleOrDefault

        If member IsNot Nothing Then
            ' La fecha de expiración se almacena en UTC por tanto la validación se hace con esa fecha          
            If member.PasswordVerificationTokenExpirationDate <= Date.UtcNow Then
                'La fecha de expiración del token ha caducado no permitir que el usuario resetee su contraseña
                Flash.Instance.Warning("", "El enlace ha caducado vuelva a solicitar el reseteo de su contraseña.")
                Return RedirectToAction("Reset", "Account")
            Else
                Return RedirectToAction("ResetPasswordRequest", "Account", New With {.id = id})
            End If
        Else
            Flash.Instance.Error("", "El enlace no es válido ya sea porque es erróneo o ya fue usado para realizar el proceso, verifique el enlace recibido e intente nuevamente.")
            Return RedirectToAction("Index", "Home")
        End If
    End Function

    ' GET: /Account/ResetPasswordRequest/

    <AllowAnonymous()> _
    Function ResetPasswordRequest(ByVal id As String) As ActionResult
        Return View()
    End Function


    ' GET: /Account/ResetPasswordRequest/id
    <AllowAnonymous()> _
    <HttpPost()> _
    Function ResetPasswordRequest(ByVal id As String, ByVal model As LocalPasswordModel) As ActionResult
        If model.NewPassword <> model.ConfirmPassword Then
            Flash.Instance.Warning("", "Las contraseñas no coindicen en ambas casillas por favor vuelva a intentarlo.")
            Return View()
        ElseIf model.NewPassword.Length < 6 Then
            Flash.Instance.Warning("", "La contraseña debe tener una longitud mínima de 6 caracteres.")
            Return View()
        Else
            If WebSecurity.ResetPassword(id, model.NewPassword) Then
                Flash.Instance.Success("", "Se ha cambiado la contraseña exitosamente, ahora puede ingresar usando su nueva contraseña.")
                Return RedirectToAction("Index", "Home")
            Else
                Flash.Instance.Error("", "El enlace no es válido por que es erróneo o el proceso ya ha sido realizado, verifique el enlace recibido e intente nuevamente.")
                Return View()
            End If
        End If

    End Function


    'Public Function UserStats() As ActionResult
    'End Function

    <AllowAnonymous()> _
    Function ActivateAccount(ByVal id As String) As ActionResult
        Dim db As New BaseEntities
        Dim diasVigenciaActivacion As Integer
        Dim fechaLimiteActivacion As DateTime
        Dim usuarioActivado As New Usuarios
        Dim paramDiasVigenciaActivacion As String

        usuarioActivado = (From s In db.Usuarios Join c In db.webpages_Membership On s.idUsuario Equals c.UserId Where c.ConfirmationToken = id Select s).FirstOrDefault
        If usuarioActivado IsNot Nothing Then
            paramDiasVigenciaActivacion = (From p In db.Parametros Where p.Clave = "DIAS_ACTIVACION_CUENTA" Select p.Valor).SingleOrDefault

            If Not Integer.TryParse(paramDiasVigenciaActivacion, diasVigenciaActivacion) Then
                diasVigenciaActivacion = 4
            End If
            fechaLimiteActivacion = WebSecurity.GetCreateDate(usuarioActivado.Correo).AddDays(diasVigenciaActivacion)

            If fechaLimiteActivacion < Date.Now Then
                Flash.Instance.Warning("", "El enlace ha expirado, consulte con su administrador del sistema.")
            Else
                If WebSecurity.ConfirmAccount(id) Then
                    usuarioActivado.CuentaActiva = True
                    db.SaveChanges()
                    Flash.Instance.Success("", String.Format("Se ha activado la cuenta {0} ahora puede ingresar con las credenciales que le fueron enviadas.", usuarioActivado.Correo))
                Else
                    Flash.Instance.Warning("", "El enlace no es válido, verifique y vuelva a intentar nuevamente.")
                End If
            End If
        End If

        Return RedirectToAction("Index", "Home")
    End Function

    Private Function VerificarIntentosFallidos(ByVal model As LoginModel) As Boolean
        Dim temporalmenteBloqueado As Boolean = False
        Dim intentos As Integer
        Dim minutos As Integer

        'Consultar los parametros de limite y número de intentos máximos configurados
        Dim paramIntentos As String = (From p In db.Parametros Where p.Clave = "MAX_NUM_INTENTOS" Select p.Valor).SingleOrDefault
        Dim paramMinutosEspera As String = (From p In db.Parametros Where p.Clave = "MINUTOS_ESPERA" Select p.Valor).SingleOrDefault

        If Integer.TryParse(paramIntentos, intentos) And Integer.TryParse(paramMinutosEspera, minutos) Then
            temporalmenteBloqueado = WebSecurity.IsAccountLockedOut(model.UserName, intentos, minutos * 60)
            If temporalmenteBloqueado Then
                Flash.Instance.Warning("", String.Format("Se ha alcanzado el número máximo de {0} intentos fallidos, vuelva a intentarlo en {1} minutos", intentos, minutos))
            End If
        End If
        Return temporalmenteBloqueado
    End Function

    'Public Function ValidateExternalLogin(ByVal user As String, ByVal password As String) As String
    '    Dim dbc As New BaseEntities
    '    Dim respuesta As String = ""

    '    If user = "" Or password = "" Then
    '        respuesta = "E|Faltan parámetros para invocar la función"
    '    Else

    '        If WebSecurity.Login(user, password, False) Then
    '            Dim usuarioBuscado As Usuarios
    '            Dim cuenta As webpages_Membership

    '            usuarioBuscado = (From u In dbc.Usuarios Where u.Correo = WebSecurity.CurrentUserName Or u.Correo = user).SingleOrDefault

    '            If usuarioBuscado IsNot Nothing Then

    '                cuenta = (From u In dbc.webpages_Membership Where u.UserId = usuarioBuscado.idUsuario).SingleOrDefault

    '                If usuarioBuscado.CuentaActiva And cuenta.IsConfirmed Then
    '                    respuesta = "S|" + usuarioBuscado.idUsuario.ToString
    '                Else
    '                    respuesta = "E|La cuenta del usuario esta desactivada o no ha sido confirmada"
    '                End If
    '            End If
    '            WebSecurity.Logout()
    '        Else
    '            respuesta = "E|El usuario o contraseña no son correctos verifique su información e intente nuevamente"
    '        End If

    '    End If
    '    Return respuesta
    'End Function

#Region "Aplicaciones auxiliares"
    Private Function RedirectToLocal(ByVal returnUrl As String) As ActionResult
        If Url.IsLocalUrl(returnUrl) Then
            Return Redirect(returnUrl)
        Else
            Return RedirectToAction("Index", "Home")
        End If
    End Function

    Public Enum ManageMessageId
        ChangePasswordSuccess
        SetPasswordSuccess
        RemoveLoginSuccess
    End Enum
#End Region

End Class
