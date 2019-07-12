Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class CapacitacionMetadata
    <Display(Name:="ID")> _
    Public Property idCapacitacion As Integer
    <Required(ErrorMessage:="Ingrese el título de la capacitacion")> _
    <Display(Name:="Título")> _
    Public Property NombreCapacitacion As String
    <Required(ErrorMessage:="Ingrese la descripción de la capacitación")> _
    Public Property Descripcion As String
    <Display(Name:="Fecha de Inicio")> _
    Public Property FechaInicio As Nullable(Of Date)
    <Display(Name:="Fecha de Finalización")> _
    Public Property FechaFin As Nullable(Of Date)
    Public Property Activo As String
    <JsonIgnore>
    Public Overridable Property Grupo As ICollection(Of Grupo) = New HashSet(Of Grupo)
End Class

<MetadataType(GetType(CapacitacionMetadata))>
Partial Public Class Capacitacion
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