Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports WebMatrix.WebData

Public Class ApiTokenController
    Inherits System.Web.Http.ApiController

    Function PostToken(ByVal info As LoginViewModel) As Object
        If WebSecurity.Login(info.usuario, info.password) Then
            Dim db As New BaseEntities
            Dim usuario As Usuarios = (From u In db.Usuarios Where u.Correo = info.usuario).First
            Dim token As String = JWTController.JWTToken(info.usuario)
            If info.playerId IsNot Nothing And info.playerId.Length > 0 Then
                usuario.PlayerId = info.playerId
                db.SaveChanges()
            End If
            Return New With {.correo = usuario.Correo, .nombre = usuario.Nombre, .token = token}
        End If
        Dim message As String
        If WebSecurity.IsConfirmed(info.usuario) Then
            message = "El nombre de usuario o la contraseña especificados son incorrectos."
        Else
            Dim db As New BaseEntities
            Dim usuarioExiste As Boolean = (From u In db.Usuarios Where u.Correo = info.usuario).Any
            If usuarioExiste Then
                message = "El usuario no ha confirmado su cuenta."
            Else
                message = "No se encontro cuenta con ese usuario."
            End If
        End If
        Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, New With {.message = message}))
    End Function

End Class
