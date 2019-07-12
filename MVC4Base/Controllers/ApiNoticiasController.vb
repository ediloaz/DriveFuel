Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports WebMatrix.WebData
Public Class ApiNoticiasController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiNoticias
    Function GetNoticias() As ApiNoticiasViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault

        Dim l_noticias = db.Noticias
        Return New ApiNoticiasViewModel With {.noticias = l_noticias.AsEnumerable}
    End Function

End Class