Imports System.Data.Entity
Imports WebMatrix.WebData
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions

'<CustomAuthorize({"AdminUsuarios"})>
Public Class UsuariosController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Usuarios/

    Function Index() As ActionResult
        Return View(db.Usuarios.ToList())
    End Function

    '
    ' GET: /Usuarios/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim usuarios As Usuarios = db.Usuarios.Find(id)
        If IsNothing(usuarios) Then
            Return HttpNotFound()
        End If
        Return View(usuarios)
    End Function

    '
    ' GET: /Usuarios/Create

    Function Create() As ActionResult
        ViewBag.rolesList = obtenerRoles()
        ViewBag.rolesUser = CType({}, webpages_Roles())
        Return View()
    End Function

    '
    ' POST: /Usuarios/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal usuario As Usuarios, ByVal rolesSelected As String()) As ActionResult
        Dim datosValidos As Boolean = True

        If ModelState.IsValid Then
            If rolesSelected Is Nothing OrElse rolesSelected.Count = 0 Then
                Flash.Instance.Error("", "Debe seleccionar por lo menos un perfil")
                rolesSelected = {}
                datosValidos = False
            End If
            If datosValidos Then
                Dim passIni As String = Web.Security.Membership.GeneratePassword(8, 0)
                Dim verificationToken As String = WebSecurity.CreateUserAndAccount(usuario.Correo, passIni, New With {
                                                     .Nombre = usuario.Nombre, _
                                                     .InformacionContacto = usuario.InformacionContacto, _
                                                     .CuentaActiva = 0}, True)
                Roles.AddUserToRoles(usuario.Correo, rolesSelected)
                Dim respuestaEnvioCorreo As RespuestaControlGeneral = EnviarConfirmacion(usuario.Nombre, usuario.Correo, passIni, verificationToken)
                If respuestaEnvioCorreo.Tipo = EnumTipoRespuesta.Exito Then
                    Flash.Instance.Success("", "Se ha enviado un correo al usuario {0} con un enlace para activar la cuenta y las credenciales para ingresar al portal.".StringFormat(usuario.Nombre))
                    Return Redirect(Url.Action("Index"))
                ElseIf respuestaEnvioCorreo.Tipo = EnumTipoRespuesta.Fracaso Then
                    Flash.Instance.Error("", "Error al enviar correo, usar función para enviar correo de confirmación: " + respuestaEnvioCorreo.Mensaje)

                    Return Redirect(Url.Action("Edit", New With {.id = WebSecurity.GetUserId(usuario.Correo)}))
                End If
            End If
        End If

        Return View(usuario)
    End Function

    '
    ' GET: /Usuarios/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim usuario As Usuarios = db.Usuarios.Find(id)
        Dim rolesDisponibles As webpages_Roles() = obtenerRoles()

        If IsNothing(usuario) Then
            Return HttpNotFound()
        End If

        ViewBag.rolesList = rolesDisponibles
        ViewBag.rolesUser = usuario.webpages_Roles.ToArray
        ViewBag.idUsuario = id

        Return View(usuario)
    End Function

    '
    ' POST: /Usuarios/Edit/5

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal id As Integer, ByVal model As Usuarios, ByVal rolesSelected As String()) As ActionResult
        Dim usuario As Usuarios = db.Usuarios.Find(id)
        Dim datosValidos As Boolean = True
        Dim actualizado As Integer

        Dim rolesDisponibles As webpages_Roles() = obtenerRoles()
        ViewBag.rolesList = rolesDisponibles

        If ModelState.IsValid Then
            If rolesSelected Is Nothing OrElse rolesSelected.Count = 0 Then
                Flash.Instance.Error("Debe seleccionar por lo menos un perfil.")
                rolesSelected = {}
                datosValidos = False
            End If
            If usuario.Correo.ToUpper <> model.Correo.ToUpper And WebSecurity.UserExists(model.Correo) Then
                ModelState.AddModelError("", ErrorCodeToString(MembershipCreateStatus.DuplicateUserName))
                datosValidos = False
            End If

            If datosValidos Then
                If usuario.webpages_Roles.Count > 0 Then
                    Roles.RemoveUserFromRoles(usuario.Correo, usuario.webpages_Roles.Select(Function(r) r.RoleName).ToArray)
                End If

                Roles.AddUserToRoles(usuario.Correo, rolesSelected)
                Try
                    usuario.Nombre = model.Nombre
                    usuario.Correo = model.Correo
                    usuario.InformacionContacto = model.InformacionContacto
                    actualizado = db.SaveChanges()
                Catch ex As Exception
                    Flash.Instance.Warning("No se pudo actualizar la información del usuario {0} , intente nuevamente.".StringFormat(model.Nombre))
                End Try

                If actualizado > 0 Then
                    Flash.Instance.Success("Se ha actualizado la información del usuario {0}.".StringFormat(model.Nombre))
                End If

                If WebSecurity.CurrentUserId = usuario.idUsuario Then
                    WebSecurity.Logout()
                    Return RedirectToAction("Index", "Home")
                Else
                    Return RedirectToAction("Index")
                End If
            End If
        End If

        Return View(usuario)
    End Function

    '
    ' GET: /Usuarios/Delete/5

    Function Delete(ByVal id As Integer) As ActionResult
        Dim usuario As Usuarios = db.Usuarios.Find(id)
        If IsNothing(usuario) Then
            Return HttpNotFound()
        End If

        Return View(usuario)
    End Function

    '
    ' POST: /Usuarios/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer, ByVal accion As String) As ActionResult
        Dim usuario As Usuarios = db.Usuarios.Find(id)
        If usuario Is Nothing Then Return HttpNotFound()

        usuario.CuentaActiva = (accion = "activar")
        db.SaveChanges()
        Flash.Instance.Success("", String.Format("Cuenta {0} exitosamente", If(usuario.CuentaActiva, "Activada", "Desactivada")))

        Return RedirectToAction("Details", New With {.id = id})
    End Function

    Function ReEnviarTokenActivacion(ByVal id As Integer) As ActionResult
        Dim usuarioSend As Usuarios = db.Usuarios.Find(id)
        If IsNothing(usuarioSend) Then
            Return HttpNotFound()
        End If

        Dim respuestaEnvioCorreo As RespuestaControlGeneral
        Dim memberShipUpdate = (From mem In db.webpages_Membership Where mem.UserId = usuarioSend.idUsuario).FirstOrDefault

        If memberShipUpdate IsNot Nothing Then
            Try
                If Not memberShipUpdate.IsConfirmed Then
                    'Generar una nueva contraseña                                        
                    Dim nuevoPassword As String = Guid.NewGuid().ToString("N").Substring(0, 8)

                    'Crear un nuevo token  de forma artificial para activar la cuenta de usuario
                    Dim newToken As String = Guid.NewGuid().ToString("N").Substring(0, 23) 'WebSecurity.GeneratePasswordResetToken(correoUser)

                    'El token de reseteo de contraseña lo colocamos tambien como token de activación de cuenta
                    memberShipUpdate.ConfirmationToken = newToken
                    memberShipUpdate.CreateDate = DateTime.Now
                    memberShipUpdate.PasswordVerificationToken = newToken
                    memberShipUpdate.PasswordVerificationTokenExpirationDate = DateTime.Now.AddDays(1)
                    db.SaveChanges()

                    'Resetear password en la base de datos con el método nativo 
                    Dim actualizado = WebSecurity.ResetPassword(newToken, nuevoPassword)
                    If actualizado Then
                        respuestaEnvioCorreo = EnviarConfirmacion(usuarioSend.Nombre, usuarioSend.Correo, nuevoPassword, newToken)

                        If respuestaEnvioCorreo.Tipo = EnumTipoRespuesta.Exito Then
                            Flash.Instance.Success("", "Se ha reenviado un correo al usuario {0} con un enlace para activar la cuenta y las credenciales para ingresar al portal.".StringFormat(usuarioSend.Nombre))
                            Return Redirect(Url.Action("Details", New With {.id = usuarioSend.idUsuario}))
                        ElseIf respuestaEnvioCorreo.Tipo = EnumTipoRespuesta.Fracaso Then
                            Flash.Instance.Error("", "Error al enviar correo, intentar de nuevo :" + respuestaEnvioCorreo.Mensaje)
                        End If
                    Else
                        Flash.Instance.Error("", "Error al crear nueva contraseña para el usuario.")
                    End If
                Else
                    If Not usuarioSend.CuentaActiva Then
                        Flash.Instance.Error("", "El usuario ya confirmo su cuenta pero ha sido desactivada por algún administrador.")
                    Else
                        Flash.Instance.Error("", "El usuario ya confirmo.")
                    End If
                End If
            Catch ex As MembershipCreateUserException
                ModelState.AddModelError("", ErrorCodeToString(ex.StatusCode))
            End Try
        End If
        Return Redirect(Url.Action("Edit", New With {.id = usuarioSend.idUsuario}))
    End Function

#Region "Funciones Auxiliares"
    Function EnviarConfirmacion(ByVal nombreUser As String, ByVal UsuarioMail As String, ByVal contrasenia As String, ByVal verificationToken As String) As RespuestaControlGeneral
        Dim cuerpo As New List(Of String)
        Dim archivos As New List(Of String)
        Dim linkVericationAccount As String = "http://" + Request.Url.Authority + "/Account/ActivateAccount/" + verificationToken
        Dim webPage As String = (From w In db.Parametros Where w.Clave = "OFICIAL_WEB_PAGE").First.Valor
        Dim diasActivar As String = (From w In db.Parametros Where w.Clave = "DIAS_ACTIVACION_CUENTA").First.Valor

        Dim viewData As New ViewDataDictionary
        viewData.Add("nombreUser", nombreUser)
        viewData.Add("linkVericationAccount", linkVericationAccount)
        viewData.Add("UsuarioMail", UsuarioMail)
        viewData.Add("contrasenia", contrasenia)
        viewData.Add("diasActivar", diasActivar)
        viewData.Add("webpage", webPage)

        Dim html As String = Util.RenderPartialViewToString(ControllerContext, "MailConfirmacion", viewData)

        Return Mail.SendMail("Portal Fuel: Creación de cuenta.", {html}, {UsuarioMail}, {}, True)
    End Function

    Public Shared Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
        ' Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
        ' obtener una lista completa de códigos de estado.
        Select Case createStatus
            Case MembershipCreateStatus.DuplicateUserName
                Return "El nombre de usuario ya existe. Escriba un nombre de usuario diferente."

            Case MembershipCreateStatus.DuplicateEmail
                Return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Escriba una dirección de correo electrónico diferente."

            Case MembershipCreateStatus.InvalidPassword
                Return "La contraseña especificada no es válida. Escriba un valor de contraseña válido."

            Case MembershipCreateStatus.InvalidEmail
                Return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.InvalidAnswer
                Return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.InvalidQuestion
                Return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.InvalidUserName
                Return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo."

            Case MembershipCreateStatus.ProviderError
                Return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema."

            Case MembershipCreateStatus.UserRejected
                Return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema."

            Case Else
                Return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema."
        End Select
    End Function
#End Region

    Private Function obtenerRoles(Optional ByVal selectedValue As Object = Nothing) As webpages_Roles()
        Return db.webpages_Roles.OrderBy(Function(r) r.Description).ToArray()
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class