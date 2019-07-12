Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Net.Http.Formatting

Public Class ApiFormaPreguntasController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiFormaPreguntas
    Function GetFormaPreguntas() As IEnumerable(Of FormaPregunta)
        Dim formapregunta = db.FormaPregunta.Include(Function(f) f.Forma)
        Return formapregunta.AsEnumerable()
    End Function

    ' GET api/ApiFormaPreguntas/5
    Function GetFormaPregunta(ByVal id As Integer) As FormaPregunta
        Dim formapregunta As FormaPregunta = db.FormaPregunta.Find(id)
        If IsNothing(formapregunta) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return formapregunta
    End Function

    ' PUT api/ApiFormaPreguntas/5
    Function PutFormaPregunta(ByVal id As Integer, ByVal formapregunta As FormaPregunta) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = formapregunta.idFormaPregunta Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(formapregunta).State = EntityState.Modified
        For Each opcion In formapregunta.FormaPreguntaOpcion.ToArray
            If opcion.Opcion = "__delete__" Then
                db.Entry(opcion).State = EntityState.Deleted
            ElseIf opcion.idFormaPreguntaOpcion > 0 Then
                db.Entry(opcion).State = EntityState.Modified
            ElseIf opcion.idFormaPreguntaOpcion <= 0 Then
                db.Entry(opcion).State = EntityState.Added
            End If
        Next

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, formapregunta, JsonMediaTypeFormatter.DefaultMediaType)
    End Function

    ' POST api/ApiFormaPreguntas
    Function PostFormaPregunta(ByVal formapregunta As FormaPregunta) As HttpResponseMessage
        If ModelState.IsValid Then
            db.FormaPregunta.Add(formapregunta)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, formapregunta)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = formapregunta.idFormaPregunta}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiFormaPreguntas/5
    Function DeleteFormaPregunta(ByVal id As Integer) As HttpResponseMessage
        Dim formapregunta As FormaPregunta = db.FormaPregunta.Find(id)
        If IsNothing(formapregunta) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        For Each opcion In formapregunta.FormaPreguntaOpcion.ToArray
            db.FormaPreguntaOpcion.Remove(opcion)
        Next

        db.FormaPregunta.Remove(formapregunta)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, formapregunta)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class