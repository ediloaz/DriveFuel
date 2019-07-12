Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiUsuariosController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiUsuarios
    Function GetUsuarios() As IEnumerable(Of Usuarios)
        Return db.Usuarios.AsEnumerable()
    End Function

    ' GET api/ApiUsuarios/5
    Function GetUsuarios(ByVal id As Integer) As Usuarios
        Dim usuarios As Usuarios = db.Usuarios.Find(id)
        If IsNothing(usuarios) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return usuarios
    End Function

    ' PUT api/ApiUsuarios/5
    Function PutUsuarios(ByVal id As Integer, ByVal usuarios As Usuarios) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = usuarios.idUsuario Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(usuarios).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiUsuarios
    Function PostUsuarios(ByVal usuarios As Usuarios) As HttpResponseMessage
        If ModelState.IsValid Then
            db.Usuarios.Add(usuarios)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, usuarios)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = usuarios.idUsuario}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiUsuarios/5
    Function DeleteUsuarios(ByVal id As Integer) As HttpResponseMessage
        Dim usuarios As Usuarios = db.Usuarios.Find(id)
        If IsNothing(usuarios) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.Usuarios.Remove(usuarios)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, usuarios)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class