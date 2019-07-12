Imports Newtonsoft.Json
Imports System.ComponentModel.DataAnnotations

Public Class NotificacionesMetadata
    <JsonIgnore>
    Public Overridable Property Usuarios As Usuarios
End Class

<MetadataType(GetType(NotificacionesMetadata))>
Partial Public Class Notificaciones
    Public ReadOnly Property Usuario As String
        Get
            Return Me.Usuarios.Nombre
        End Get

    End Property
End Class