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

Partial Public Class UsuariosAcceso
    Public Property idUsuariosAcceso As Integer
    Public Property idUser As Integer
    Public Property idCliente As Nullable(Of Integer)
    Public Property idProducto As Nullable(Of Integer)
    Public Property idUserAutoriza As Integer
    Public Property fechaAutorización As Date

    Public Overridable Property Cliente As Cliente
    Public Overridable Property Producto As Producto
    Public Overridable Property Usuarios As Usuarios
    Public Overridable Property UsuarioAutoriza As Usuarios

End Class