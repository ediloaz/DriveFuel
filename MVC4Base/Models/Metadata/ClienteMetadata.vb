Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class ClienteMetadata
    Public Property idCliente As Integer
    <Required(ErrorMessage:="Ingrese el nombre de Cliente")> _
    <Display(Name:="Nombre de Cliente")> _
    Public Property NombreCliente As String
    Public Property Logo As String
    Public Property Activo As Boolean
End Class
<MetadataType(GetType(ClienteMetadata))>
Partial Public Class Cliente
    Public ReadOnly Property ActivoColor As String
        Get
            If Me.Activo = True Then
                Return "success"
            Else
                Return "danger"
            End If
        End Get
    End Property

    Public ReadOnly Property ActivoTexto As String
        Get
            If Me.Activo = True Then
                Return "Activo"
            Else
                Return "Inactivo"
            End If
        End Get
    End Property
End Class