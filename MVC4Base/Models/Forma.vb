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

Partial Public Class Forma
    Public Property IdForma As Integer
    Public Property Descripcion As String
    Public Property idProducto As Integer
    Public Property idCliente As Integer

    Public Overridable Property FormaPregunta As ICollection(Of FormaPregunta) = New HashSet(Of FormaPregunta)
    <JsonIgnore>
    Public Overridable Property Ruta As ICollection(Of Ruta) = New HashSet(Of Ruta)
    <JsonIgnore>
    Public Overridable Property FormaRespuesta As ICollection(Of FormaRespuesta) = New HashSet(Of FormaRespuesta)
    <JsonIgnore>    
	Public Overridable Property Cliente As Cliente
    <JsonIgnore>    
	Public Overridable Property Producto As Producto

End Class