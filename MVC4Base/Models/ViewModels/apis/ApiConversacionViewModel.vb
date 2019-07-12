Public Class ApiConversacionViewModel
    Public Property idConversacion As Integer
    Public Property FechaCreacion As Date
    Public Property Grupal As Boolean
    Public Property Nombre As String
    Public Property UltimoMensaje As String
    Public Property UltimoFecha As Nullable(Of Date)
    Public Property idUsuario As Integer
    Public Property idUsuarioInvitado As Nullable(Of Integer)

    Public Property Usuarios As Integer()
    Public Property Grupos As Integer()

    'Public Overridable Property Usuarios As Usuarios
    'Public Overridable Property Usuarios1 As Usuarios
    'Public Overridable Property Mensaje As IEnumerable(Of Mensaje)
End Class
