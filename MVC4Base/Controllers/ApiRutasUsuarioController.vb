Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.IdentityModel.Tokens.Jwt
Imports Microsoft.IdentityModel.Tokens

Public Class ApiRutasUsuarioController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiRutasUsuario
    Function GetRutas() As ApiRutasUsuarioViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault

        Dim rutas As New List(Of Ruta)
        rutas = usuario.Ruta.ToList

        Dim grupos = usuario.Grupo
        For Each grupo In grupos
            If grupo.Ruta.Count > 0 Then
                rutas.AddRange(grupo.Ruta.ToList())
            End If
        Next

        Return New ApiRutasUsuarioViewModel With {.rutas = rutas.Distinct().AsEnumerable()}
    End Function

    ' GET api/ApiRutasUsuario/5
    Function GetRuta(ByVal id As Integer) As Ruta
        Dim ruta As Ruta = db.Ruta.Find(id)
        If IsNothing(ruta) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return ruta
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class