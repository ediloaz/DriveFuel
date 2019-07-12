Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class ProductoMetadata
    Public Property idProducto As Integer
    <Display(Name:="Cliente")> _
     Public Property idCliente As Integer
    Public Property Clave As String
    <Required(ErrorMessage:="Ingrese el nombre de Producto")> _
   <Display(Name:="Nombre de Producto")> _
    Public Property NombreProducto As String
    Public Property Activo As Boolean
End Class
<MetadataType(GetType(ProductoMetadata))>
Partial Public Class Producto
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