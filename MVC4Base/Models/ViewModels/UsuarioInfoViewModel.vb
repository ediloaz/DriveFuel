Imports System.ComponentModel.DataAnnotations

Public Class UsuarioInfoViewModel
    <Required()> _
    <Display(Name:="Nombre")> _
    <StringLength(50)> _
    Public Property Nombre As String

    <Required()> _
    <Display(Name:="Correo")> _
    <StringLength(50)> _
    <DataType(DataType.EmailAddress)>
    <EmailAddress>
    Public Property Correo As String

    <Display(Name:="Información de Contacto")> _
    <StringLength(250)> _
    Public Property InformacionContacto As String
End Class
