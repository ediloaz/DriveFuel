Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiFormasController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiFormas
    Function GetFormas() As IEnumerable(Of Forma)
        Return db.Forma.AsEnumerable()
    End Function

    ' GET api/ApiFormas/5
    Function GetForma(ByVal id As Integer) As Forma
        Dim forma As Forma = db.Forma.Find(id)
        If IsNothing(forma) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return forma
    End Function

    ' PUT api/ApiFormas/5
    Function PutForma(ByVal id As Integer, ByVal forma As Forma) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = forma.IdForma Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(forma).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiFormas
    Function PostForma(ByVal forma As Forma) As HttpResponseMessage
        If ModelState.IsValid Then
            db.Forma.Add(forma)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, forma)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = forma.IdForma}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiFormas/5
    Function DeleteForma(ByVal id As Integer) As HttpResponseMessage
        Dim forma As Forma = db.Forma.Find(id)
        If IsNothing(forma) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.Forma.Remove(forma)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, forma)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class