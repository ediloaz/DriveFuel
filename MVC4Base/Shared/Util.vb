Public Class Util

    Public Shared Function RenderPartialViewToString(ByVal controllerContext As ControllerContext, ByVal viewName As String, ByVal viewData As ViewDataDictionary) As String
        If String.IsNullOrEmpty(viewName) Then
            viewName = controllerContext.RouteData.GetRequiredString("action")
        End If

        Using sw As New IO.StringWriter
            Dim viewResult As ViewEngineResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName)
            Dim viewContext As ViewContext = New ViewContext(controllerContext, viewResult.View, viewData, New TempDataDictionary, sw)
            viewResult.View.Render(viewContext, sw)

            Return sw.GetStringBuilder().ToString()
        End Using

    End Function

    Public Shared Function ContentType(ByVal FileExtension As String) As String
        Dim d As New Dictionary(Of String, String)
        'Images'
        d.Add("bmp", "image/bmp")
        d.Add("gif", "image/gif")
        d.Add("jpeg", "image/jpeg")
        d.Add("jpg", "image/jpeg")
        d.Add("png", "image/png")
        d.Add("tif", "image/tiff")
        d.Add("tiff", "image/tiff")
        'Documents'
        d.Add("doc", "application/msword")
        d.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        d.Add("pdf", "application/pdf")
        'Slideshows'
        d.Add("ppt", "application/vnd.ms-powerpoint")
        d.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation")
        'Data'
        d.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        d.Add("xls", "application/vnd.ms-excel")
        d.Add("csv", "text/csv")
        d.Add("xml", "text/xml")
        d.Add("txt", "text/plain")
        'Compressed Folders'
        d.Add("zip", "application/zip")
        'Audio'
        d.Add("ogg", "application/ogg")
        d.Add("mp3", "audio/mpeg")
        d.Add("wma", "audio/x-ms-wma")
        d.Add("wav", "audio/x-wav")
        'Video'
        d.Add("wmv", "audio/x-ms-wmv")
        d.Add("swf", "application/x-shockwave-flash")
        d.Add("avi", "video/avi")
        d.Add("mp4", "video/mp4")
        d.Add("mpeg", "video/mpeg")
        d.Add("mpg", "video/mpeg")
        d.Add("qt", "video/quicktime")
        If d.ContainsKey(FileExtension) Then
            Return d(FileExtension)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function ObtenerNombreNavegador(ByVal elementosURL As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Dim respuesta As New Dictionary(Of String, String)
        For Each elemento In elementosURL
            Select Case elemento.Key.ToLower
                Case "home"
                    Continue For
                Case "index"
                    Continue For
                Case Else
                    Dim traduccion As String = Idiomas.Urls.ResourceManager.GetString(elemento.Key)
                    If traduccion Is Nothing Then
#If DEBUG Then
                        traduccion = "_" + elemento.Key + "_"
#Else
                        continue for
#End If
                    ElseIf traduccion.Trim.Length = 0 Then
                        Continue For
                    End If
                    respuesta.Add(traduccion, elemento.Value)
            End Select
        Next
        Return respuesta
    End Function

    Public Shared Function ObtenerParametro(ByVal clave As String) As String
        Dim db As New BaseEntities
        Dim param = (From p In db.Parametros Where p.Clave = clave).FirstOrDefault()
        If param Is Nothing Then
            Return ""
        Else
            Return param.Valor
        End If
    End Function

End Class
