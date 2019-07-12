Imports System.Net.Mail
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class Mail

    Public Shared Function SendMail(ByVal asunto As String, ByVal cuerpo As String(), ByVal lista As String(), ByVal archivosAdjuntos As String(), Optional ByRef isHtml As Boolean = False) As RespuestaControlGeneral
        Dim response As RespuestaControlGeneral
        Dim db As New BaseEntities
        Dim configs As String() = (From p In db.Parametros Where p.Clave = "CONFIG_CORREO" Select p.Valor).SingleOrDefault.Split("|")

        '   0   |       1           |       2           |     3     |    4    |  5   | 6 |       7        |        8
        'Nombre |     Dirección     |      Usuario      |Contraseña |Protocolo|Puerto|SSL|ServidorEntrante|ServidorSaliente
        'Verquet|noreply@verquet.com|noreply@verquet.com|D3sarrollo;|  SMPT   |  25  | 0 |mail.verquet.com|mail.verquet.com
        Dim ipCorreo = configs(8)
        Dim puertoCorreo = configs(5)

        Dim msg As MailMessage = New MailMessage()
        For Each Mail As String In lista
            If IsValidEmail(Mail) Then
                msg.To.Add(Mail)
            End If
        Next
        msg.From = New MailAddress(configs(1), configs(0), System.Text.Encoding.UTF8)
        msg.Subject = asunto
        msg.SubjectEncoding = System.Text.Encoding.UTF8
        For Each s As String In cuerpo
            msg.Body += s + Environment.NewLine
        Next
        msg.BodyEncoding = System.Text.Encoding.UTF8
        msg.IsBodyHtml = isHtml

        Dim client As SmtpClient = New SmtpClient()
        client.Port = Integer.Parse(puertoCorreo)
        client.Host = ipCorreo
        client.Credentials = New System.Net.NetworkCredential(configs(2), configs(3))
        client.EnableSsl = configs(6)

        For Each archivo As String In archivosAdjuntos
            Try
                msg.Attachments.Add(New Attachment(archivo))
            Catch ex As Exception

                If ex IsNot Nothing Then

                End If
            End Try
        Next
        Try
            client.Timeout() = 100000
            client.Send(msg)
            response = New RespuestaControlGeneral(EnumTipoRespuesta.Exito, "")
        Catch ex As Exception
            client.Dispose()
            client = Nothing

            msg.Attachments.Dispose()
            msg.Dispose()
            msg = Nothing
            response = New RespuestaControlGeneral(EnumTipoRespuesta.Fracaso, ex.Message + _
                                                   If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ""))
        End Try
        Return response
    End Function


    Public Shared Function IsValidEmail(strIn As String) As Boolean
        Dim invalid As Boolean
        invalid = False

        If String.IsNullOrEmpty(strIn) Then Return False
        ' Use IdnMapping class to convert Unicode domain names.
        Try
            strIn = Regex.Replace(strIn, "(@)(.+)$", AddressOf DomainMapper,
                                  RegexOptions.None)
        Catch e As Exception 'RegexMatchTimeoutException
            Return False
        End Try

        If invalid Then Return False
        ' Return true if strIn is in valid e-mail format.
        Try
            Return Regex.IsMatch(strIn, "^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + _
                         "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase)
        Catch e As Exception 'RegexMatchTimeoutException
            Return False
        End Try
    End Function

    Public Shared Function DomainMapper(match As Match) As String
        Dim invalid As Boolean
        ' IdnMapping class with default property values.
        Dim idn As New IdnMapping()

        Dim domainName As String = match.Groups(2).Value
        Try
            domainName = idn.GetAscii(domainName)
        Catch e As ArgumentException
            invalid = True
        End Try
        Return match.Groups(1).Value + domainName
    End Function

End Class
