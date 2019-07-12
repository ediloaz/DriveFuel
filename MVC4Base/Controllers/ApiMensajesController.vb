Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports WebMatrix.WebData

Public Class ApiMensajesController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiMensajes
    Function GetMensajes() As IEnumerable(Of Mensaje)
        Dim mensaje = db.Mensaje.Include(Function(m) m.Conversacion).Include(Function(m) m.Usuarios)
        Return mensaje.AsEnumerable()
    End Function

    ' GET api/ApiMensajes/5
    Function GetMensaje(ByVal id As Integer) As Mensaje
        Dim mensaje As Mensaje = db.Mensaje.Find(id)
        If IsNothing(mensaje) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return mensaje
    End Function

    ' PUT api/ApiMensajes/5
    Function PutMensaje(ByVal id As Integer, ByVal mensaje As Mensaje) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = mensaje.idMensaje Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(mensaje).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiMensajes
    Function PostMensaje(ByVal mensaje As Mensaje) As HttpResponseMessage
        Dim _idUsuario = WebSecurity.CurrentUserId
        If _idUsuario < 0 Then
            Dim correo As String = JWTController.JWTUser(Request)
            _idUsuario = (From u In db.Usuarios Where u.Correo = correo Select u.idUsuario).FirstOrDefault
        End If
        If ModelState.IsValid Then
            mensaje.idUsuario = _idUsuario
            mensaje.FechaCreacion = Now
            db.Mensaje.Add(mensaje)
            db.SaveChanges()

            Dim conversacion As Conversacion = (From c In db.Conversacion Where c.idConversacion = mensaje.idConversacion).FirstOrDefault()
            conversacion.UltimoMensaje = mensaje.Mensaje1
            conversacion.UltimoFecha = mensaje.FechaCreacion
            db.SaveChanges()

            Dim oneSignalIds As String()
            If conversacion.Grupal Then
                Dim tempIds As List(Of String) = conversacion.Usuarios2.Where(Function(u) u.PlayerId IsNot Nothing And u.idUsuario <> _idUsuario).Select(Function(u) u.PlayerId).ToList()
                If conversacion.idUsuario <> _idUsuario AndAlso conversacion.Usuarios.PlayerId IsNot Nothing Then
                    tempIds.Add(conversacion.Usuarios.PlayerId)
                End If
                oneSignalIds = tempIds.ToArray()
            Else
                If conversacion.idUsuarioInvitado = _idUsuario AndAlso conversacion.Usuarios1.PlayerId IsNot Nothing Then
                    oneSignalIds = {conversacion.Usuarios.PlayerId}
                ElseIf conversacion.idUsuario = _idUsuario AndAlso conversacion.Usuarios.PlayerId IsNot Nothing Then
                    oneSignalIds = {conversacion.Usuarios1.PlayerId}
                End If
            End If

            If oneSignalIds IsNot Nothing AndAlso oneSignalIds.Length > 0 Then
                Dim oneSignalRequest As HttpWebRequest = WebRequest.Create("https://onesignal.com/api/v1/notifications")
                oneSignalRequest.KeepAlive = True
                oneSignalRequest.Method = "POST"
                oneSignalRequest.ContentType = "application/json; charset=utf-8"
                Dim idsStr As String = String.Join(",", Array.ConvertAll(oneSignalIds, Function(x) """" + x.Trim() + """"))
                Dim dataStr As String = "{" & """app_id"": ""183eafdb-7d27-4173-a241-1d20256dfbff""," & """contents"": {""en"": """ + mensaje.Mensaje1 + """}," & """include_player_ids"": [" + idsStr + "]}"
                Dim data As Byte() = Encoding.UTF8.GetBytes(dataStr)
                Dim responseContent As String = Nothing

                Try
                    Using writer = oneSignalRequest.GetRequestStream()
                        writer.Write(data, 0, data.Length)
                    End Using
                    Using oneSignalResponse = oneSignalRequest.GetResponse()
                        Using reader = New IO.StreamReader(oneSignalResponse.GetResponseStream())
                            responseContent = reader.ReadToEnd()
                        End Using
                    End Using
                Catch ex As Exception
                    Dim eStr As String = ex.Message
                End Try
            End If

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, mensaje)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = mensaje.idMensaje}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiMensajes/5
    Function DeleteMensaje(ByVal id As Integer) As HttpResponseMessage
        Dim mensaje As Mensaje = db.Mensaje.Find(id)
        If IsNothing(mensaje) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.Mensaje.Remove(mensaje)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, mensaje)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class