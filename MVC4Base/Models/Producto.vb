'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports Newtonsoft.Json

Partial Public Class Producto
    Public Property idProducto As Integer
    Public Property idCliente As Integer
    Public Property Clave As String
    Public Property NombreProducto As String
    Public Property Activo As Boolean
    <JsonIgnore>
    Public Overridable Property Cliente As Cliente
    <JsonIgnore>
    Public Overridable Property Forma As ICollection(Of Forma) = New HashSet(Of Forma)
    <JsonIgnore>
    Public Overridable Property Grupo As ICollection(Of Grupo) = New HashSet(Of Grupo)
    <JsonIgnore>
    Public Overridable Property Ruta As ICollection(Of Ruta) = New HashSet(Of Ruta)
    <JsonIgnore>
    Public Overridable Property UsuariosAcceso As ICollection(Of UsuariosAcceso) = New HashSet(Of UsuariosAcceso)

End Class
