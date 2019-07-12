Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiRutaCheckpointController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiRutaCheckpoint
    Function GetRutaCheckpoints() As IEnumerable(Of RutaCheckpoint)
        Dim rutacheckpoint = db.RutaCheckpoint.Include(Function(r) r.Ruta)
        Return rutacheckpoint.AsEnumerable()
    End Function

    ' GET api/ApiRutaCheckpoint/5
    Function GetRutaCheckpoint(ByVal id As Integer) As RutaCheckpoint
        Dim rutacheckpoint As RutaCheckpoint = db.RutaCheckpoint.Find(id)
        If IsNothing(rutacheckpoint) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return rutacheckpoint
    End Function

    ' PUT api/ApiRutaCheckpoint/5
    Function PutRutaCheckpoint(ByVal id As Integer, ByVal rutacheckpoint As RutaCheckpoint) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = rutacheckpoint.idRutaCheckPoint Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(rutacheckpoint).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiRutaCheckpoint
    Function PostRutaCheckpoint(ByVal rutacheckpoint As RutaCheckpoint) As HttpResponseMessage
        If ModelState.IsValid Then
            db.RutaCheckpoint.Add(rutacheckpoint)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, rutacheckpoint)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = rutacheckpoint.idRutaCheckPoint}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiRutaCheckpoint/5
    Function DeleteRutaCheckpoint(ByVal id As Integer) As HttpResponseMessage
        Dim rutacheckpoint As RutaCheckpoint = db.RutaCheckpoint.Find(id)
        If IsNothing(rutacheckpoint) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.RutaCheckpoint.Remove(rutacheckpoint)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, rutacheckpoint)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class