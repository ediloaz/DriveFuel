Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiRespuestaUsuarioController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiRespuestaUsuario
    Function GetFormaRespuestas() As IEnumerable(Of FormaRespuesta)
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        Dim formarespuesta = db.FormaRespuesta.Include(Function(f) f.Forma).Include(Function(f) f.RutaCheckpoint).Include(Function(f) f.Usuarios).
            Where(Function(f) f.idUsuario = usuario.idUsuario)
        Return formarespuesta.AsEnumerable()
    End Function

    ' GET api/ApiRespuestaUsuario/5
    Function GetFormaRespuesta(ByVal id As Integer) As FormaRespuesta
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        Dim formarespuesta As FormaRespuesta = db.FormaRespuesta.Find(id)
        If IsNothing(formarespuesta) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        If formarespuesta.idUsuario <> usuario.idUsuario Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return formarespuesta
    End Function

    ' PUT api/ApiRespuestaUsuario/5
    Function PutFormaRespuesta(ByVal id As Integer, ByVal formarespuesta As FormaRespuesta) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = formarespuesta.idFormaRespuesta Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(formarespuesta).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiRespuestaUsuario
    Function PostFormaRespuesta(ByVal formarespuesta As FormaRespuesta) As HttpResponseMessage
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        If ModelState.IsValid Then
            formarespuesta.Usuarios = usuario
            For Each respuesta In formarespuesta.FormaRespuestaDetalle
                Dim formaPregunta As FormaPregunta = db.FormaPregunta.Find(respuesta.idFormaPregunta)
                If formaPregunta.Tipo = 6 Then
                    Dim temp = respuesta.Respuesta.Split(",")
                    Dim ext = temp.First().Split("/").Last().Split(";").First()
                    Dim base64 = temp.Last().Replace("_", "/").Replace("-", "+").Replace(" ", "+")
                    Dim imageBytes As Byte() = Convert.FromBase64String(base64)

                    'Save the Byte Array as Image File.
                    Dim fileName As String = System.Guid.NewGuid.ToString + "." + ext
                    Dim filePath As String = Path.Combine(Util.ObtenerParametro("CONTENT_PATH"), "Respuestas", fileName)
                    File.WriteAllBytes(filePath, imageBytes)
                    respuesta.Respuesta = fileName
                End If
            Next
            db.FormaRespuesta.Add(formarespuesta)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, formarespuesta)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = formarespuesta.idFormaRespuesta}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiRespuestaUsuario/5
    Function DeleteFormaRespuesta(ByVal id As Integer) As HttpResponseMessage
        Dim formarespuesta As FormaRespuesta = db.FormaRespuesta.Find(id)
        If IsNothing(formarespuesta) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.FormaRespuesta.Remove(formarespuesta)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, formarespuesta)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class