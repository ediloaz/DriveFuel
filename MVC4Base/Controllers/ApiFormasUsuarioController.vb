Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports DotNetOpenAuth.Messaging

Public Class ApiFormasUsuarioController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiFormasUsuario
    Function GetFormas() As ApiFormasUsuarioViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        Dim formas As List(Of Forma) = New List(Of Forma)()
        For Each ruta As Ruta In usuario.Ruta
            formas.AddRange(ruta.Forma)
        Next
        Return New ApiFormasUsuarioViewModel With {.formas = formas.AsEnumerable()}
    End Function

    ' GET api/ApiFormasUsuario/5
    ' Obtener formas de x ruta (id)
    Function GetFormas(ByVal id As Integer) As ApiFormasUsuarioViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        Dim ruta As Ruta = (From r In db.Ruta Where r.idRuta = id).FirstOrDefault()
        If IsNothing(ruta) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        If ruta.Usuarios.Contains(usuario) Then
            Return New ApiFormasUsuarioViewModel With {.formas = ruta.Forma.AsEnumerable()}
        End If
        Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class