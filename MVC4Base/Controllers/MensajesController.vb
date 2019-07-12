Imports WebMatrix.WebData

Public Class MensajesController
    Inherits System.Web.Mvc.Controller

    '
    ' GET: /Mensajes

    Function Index() As ActionResult
        ViewBag.idUsuario = WebSecurity.CurrentUserId
        Return View()
    End Function

End Class