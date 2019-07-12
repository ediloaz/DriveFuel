Imports System.Net
Imports System.Web.Http
Imports System.Net.Http
Imports WebMatrix.WebData


Public Class ApiNotificacionController
    Inherits ApiController

    Private db As New BaseEntities
    ' GET api/ApiNotificacion
    Function GetNotificacion() As ApiNotificacionesViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault

        Dim l_notificaciones = db.Notificaciones.OrderByDescending(Function(x) x.idNotificacion).Take(10)
        Return New ApiNotificacionesViewModel With {.notificaciones = l_notificaciones.AsEnumerable}
    End Function

    ' POST api/ApiNotificacion
    Function PostNotificacion(ByVal notifica As notifica) As HttpResponseMessage

        If notifica.mensaje Is Nothing Then
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No hay un mensaje que enviar")
        End If

        Dim playerIds As String()
        Dim _grupo = db.Grupo.Where(Function(x) x.idGrupo = notifica.idGrupo).FirstOrDefault
        playerIds = _grupo.Usuarios.Where(Function(x) x.CuentaActiva = True And x.PlayerId IsNot Nothing).Select(Function(x) x.PlayerId).ToArray


        If Not playerIds.Any Then
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron usuarios registrados")
        End If

        Dim respuesta = EnviaMensajeOneSignal(notifica.mensaje, playerIds)
        If respuesta.Tipo = EnumTipoRespuesta.Fracaso Then
            Return Request.CreateResponse(HttpStatusCode.NotFound, respuesta.Mensaje)
        End If
        Dim notificacionResult As String = respuesta.Valor

        respuesta = NotificacionesController.GuardaNotificacion(notifica.mensaje)
        If respuesta.Tipo = EnumTipoRespuesta.Fracaso Then
            Return Request.CreateResponse(HttpStatusCode.InternalServerError, respuesta.Mensaje)
        End If

        Return Request.CreateResponse(HttpStatusCode.OK, notificacionResult)
    End Function
    Private Function EnviaMensajeOneSignal(ByVal mensaje As String, ByVal oneSignalIds As String()) As RespuestaControlGeneral
        Dim app_id = db.Parametros.Where(Function(x) x.Clave = "ONESIGNAL_APPID").Select(Function(x) x.Valor).FirstOrDefault()
        Dim oneSignalRequest As HttpWebRequest = WebRequest.Create("https://onesignal.com/api/v1/notifications")
        oneSignalRequest.KeepAlive = True
        oneSignalRequest.Method = "POST"
        oneSignalRequest.ContentType = "application/json; charset=utf-8"
        Dim idsStr As String = String.Join(",", Array.ConvertAll(oneSignalIds, Function(x) """" + x.Trim() + """"))
        Dim dataStr As String = "{" & """app_id"": """ & app_id & """," & """contents"": {""en"": """ + mensaje + """}," & """include_player_ids"": [" + idsStr + "]}"
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
            If ex.InnerException IsNot Nothing Then eStr &= ": " & ex.InnerException.Message
            Return New RespuestaControlGeneral(EnumTipoRespuesta.Fracaso, eStr, Nothing)
        End Try

        Return New RespuestaControlGeneral(EnumTipoRespuesta.Exito, "", responseContent)
    End Function

    Public Class notifica
        Public Property mensaje As String
        Public Property idGrupo As Integer
    End Class
End Class
