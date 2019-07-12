Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiGruposController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiGrupos
    Function GetGrupoes() As IEnumerable(Of Grupo)
        Return db.Grupo.AsEnumerable()
    End Function

    ' GET api/ApiGrupos/5
    Function GetGrupo(ByVal id As Integer) As Grupo
        Dim grupo As Grupo = db.Grupo.Find(id)
        If IsNothing(grupo) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return grupo
    End Function

    ' PUT api/ApiGrupos/5
    Function PutGrupo(ByVal id As Integer, ByVal grupo As Grupo) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = grupo.idGrupo Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(grupo).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiGrupos
    Function PostGrupo(ByVal grupo As Grupo) As HttpResponseMessage
        If ModelState.IsValid Then
            db.Grupo.Add(grupo)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, grupo)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = grupo.idGrupo}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiGrupos/5
    Function DeleteGrupo(ByVal id As Integer) As HttpResponseMessage
        Dim grupo As Grupo = db.Grupo.Find(id)
        If IsNothing(grupo) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.Grupo.Remove(grupo)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, grupo)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class