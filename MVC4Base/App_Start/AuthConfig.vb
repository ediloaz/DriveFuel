﻿Imports Microsoft.Web.WebPages.OAuth
Imports WebMatrix.WebData

Public Class AuthConfig
    Public Shared Sub RegisterAuth()
        ' Para permitir que los usuarios de este sitio inicien sesión con sus cuentas de otros sitios como, por ejemplo, Microsoft, Facebook y Twitter,
        ' es necesario actualizar este sitio. Para obtener más información, visite http://go.microsoft.com/fwlink/?LinkID=252166

        ' OAuthWebSecurity.RegisterMicrosoftClient(
        '     clientId:="",
        '     clientSecret:="")

        ' OAuthWebSecurity.RegisterTwitterClient(
        '     consumerKey:="",
        '     consumerSecret:="")

        ' OAuthWebSecurity.RegisterFacebookClient(
        '     appId:="",
        '     appSecret:="")

        ' OAuthWebSecurity.RegisterGoogleClient()
        WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Usuarios", "idUsuario", "Correo", autoCreateTables:=False)
    End Sub
End Class