Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Data.Entity.Validation

Public Class RespuestaUsuarioController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities
    '
    ' GET: /RespuestaUsuario

    Function Index(ByVal respuesta As FormaRespuesta) As ActionResult
        Dim httpRequest = HttpContext.Request
        Dim correo As String = JWTController.JWTUser2(httpRequest)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        If ModelState.IsValid Then
            respuesta.Usuarios = usuario
            For Each item As FormaRespuestaDetalle In respuesta.FormaRespuestaDetalle
                If item.Respuesta Is Nothing Then
                    item.Respuesta = ""
                End If
            Next
            For Each fileName As String In httpRequest.Files
                Dim idFormaPregunta As Integer = Int(fileName.Split(".").First())
                Dim newFileName As String = System.Guid.NewGuid.ToString + "." + fileName.Split(".").Last()
                Dim filePath As String = Path.Combine(Util.ObtenerParametro("CONTENT_PATH"), "Respuestas", newFileName)
                Dim _file = httpRequest.Files(fileName)
                _file.SaveAs(filePath)
                respuesta.FormaRespuestaDetalle.Add(New FormaRespuestaDetalle() With {.idFormaPregunta = idFormaPregunta, .Respuesta = filePath})
            Next
            db.FormaRespuesta.Add(respuesta)
            Try
                db.SaveChanges()
            Catch ex As DbEntityValidationException
                Dim errores As New List(Of String)
                For Each dbEntityValidationResult As DbEntityValidationResult In ex.EntityValidationErrors
                    For Each dbValidationError As DbValidationError In dbEntityValidationResult.ValidationErrors
                        errores.Add("Propiedad: " + dbValidationError.PropertyName + " Error:" + dbValidationError.ErrorMessage)
                        System.Console.WriteLine("Propiedad: {0} Error: {1}", dbValidationError.PropertyName, dbValidationError.ErrorMessage)
                    Next
                Next
                Return Json(New With {.errores = errores})
            End Try

            Return Json(New With {.id = respuesta.idFormaRespuesta})
        Else
            Return Json(New With {.error = "Petición incorrecta"})
            'Return httpRequest. .CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

End Class