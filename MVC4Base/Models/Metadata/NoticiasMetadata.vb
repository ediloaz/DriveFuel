Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class NoticiasMetaData
    <Required(ErrorMessage:="Ingrese el título de la noticia")> _
    <Display(Name:="Título")> _
    Public Property Titulo As String

    <Required(ErrorMessage:="Ingrese la fecha de Inicio para la noticia")> _
    <Display(Name:="Fecha de Inicio")> _
    Public Property FechaInicio As Date

    <Required(ErrorMessage:="Ingrese la fecha final para la noticia")> _
    <Display(Name:="Fecha de Finalización")> _
    Public Property FechaFin As Date

    <Required()> _
    <Display(Name:="Tipo de Noticia")> _
    Public Property idTipoNoticia As String

    <Required(ErrorMessage:="Ingrese el mensaje del cuerpo")> _
    <Display(Name:="Mensaje Principal")> _
    Public Property Mensaje

    <JsonIgnore>
    <Display(Name:="Noticia Activa")> _
    Public Property Activa As Boolean

    <JsonIgnore>
    Public Overridable Property TipoNoticia As TipoNoticia

End Class


<MetadataType(GetType(NoticiasMetaData))>
Partial Public Class Noticias
    <JsonIgnore>
    Public Property ImageFile As HttpPostedFileBase
    <JsonIgnore>
     Public ReadOnly Property TipoNoticiaColor As String
        Get
            Select Case Me.idTipoNoticia
                Case "INFO"
                    Return "info"
                Case "ERROR"
                    Return "danger"
                Case "SUCCESS"
                    Return "success"
                Case "WARNING"
                    Return "warning"
                Case Else
                    Return "default"
            End Select
            Return ""
        End Get
    End Property

End Class


