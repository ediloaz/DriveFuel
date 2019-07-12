Imports WebMatrix.WebData

Public Class NotificacionesController
    Inherits System.Web.Mvc.Controller

    '
    ' GET: /Notificaciones

    Function Index() As ActionResult
        Return View()
    End Function

    Public Shared Function GuardaNotificacion(ByVal mensaje As String) As RespuestaControlGeneral
        Dim _notificaciones As New Notificaciones
        Dim db As New BaseEntities

        _notificaciones.idUsuario = WebSecurity.CurrentUserId
        _notificaciones.Fecha = Now
        _notificaciones.Mensaje = mensaje
        db.Notificaciones.Add(_notificaciones)

        Try
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
            Return New RespuestaControlGeneral(EnumTipoRespuesta.Fracaso, msg, Nothing)
        End Try

        Return New RespuestaControlGeneral(EnumTipoRespuesta.Exito, "Notificacion Almacenada exitosamente", _notificaciones)
    End Function
End Class