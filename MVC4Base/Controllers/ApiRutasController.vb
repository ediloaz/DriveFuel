Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiRutasController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiRutas
    Function GetRutas() As IEnumerable(Of Ruta)
        Return db.Ruta.AsEnumerable()
    End Function

    ' GET api/ApiRutas/5
    Function GetRuta(ByVal id As Integer) As Ruta
        Dim ruta As Ruta = db.Ruta.Find(id)
        If IsNothing(ruta) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return ruta
    End Function

    ' PUT api/ApiRutas/5
    Function PutRuta(ByVal id As Integer, ByVal ruta As Ruta) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = ruta.idRuta Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(ruta).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiRutas
    Function PostRuta(ByVal ruta As Ruta) As HttpResponseMessage
        If ModelState.IsValid Then
            db.Ruta.Add(ruta)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, ruta)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = ruta.idRuta}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiRutas/5
    Function DeleteRuta(ByVal id As Integer) As HttpResponseMessage
        Dim ruta As Ruta = db.Ruta.Find(id)
        If IsNothing(ruta) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.Ruta.Remove(ruta)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, ruta)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class