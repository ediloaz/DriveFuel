Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class CapacitacionArchivosMetadata
    <JsonIgnore>
    Public Overridable Property CapacitacionVisitas As ICollection(Of CapacitacionVisitas) = New HashSet(Of CapacitacionVisitas)
    <JsonIgnore>
    Public Overridable Property CapacitacionTema As CapacitacionTema
End Class

<MetadataType(GetType(CapacitacionArchivosMetadata))>
Partial Public Class CapacitacionArchivos
    Public ReadOnly Property URLCompleta As String
        Get
            If _URL IsNot Nothing AndAlso _URL <> String.Empty Then
                Return Util.ObtenerParametro("OFICIAL_WEB_PAGE").ToString & Util.ObtenerParametro("CONTENT_VIRTUAL_PATH").ToString & "/Capacitacion/" & URL.Trim
            End If
            Return String.Empty
        End Get

    End Property

    Public ReadOnly Property TipoArchivo As EnumTipoArchivo

        Get
            If _URL IsNot Nothing AndAlso _URL <> String.Empty Then
                Dim l_tipoFoto As New List(Of String)(New String() {"png", "jpg", "jpeg"})
                Dim l_video As New List(Of String)(New String() {"mov", "avi", "wmp"})
                Dim extension As String = _URL.Split(".").Last()

                If l_tipoFoto.Contains(extension) Then Return EnumTipoArchivo.EsImagen
                If l_video.Contains(extension) Then Return EnumTipoArchivo.EsVideo
                Return EnumTipoArchivo.EsArchivo
            End If

            Return EnumTipoArchivo.Ilegible

        End Get
        
    End Property
End Class