Imports WebMatrix.WebData
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions
Imports Newtonsoft.Json

Public Class UsuariosAccesoController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities
#Region "Vistas"
    '
    ' GET: /UsuariosAcceso/Edit/id

    Function Edit(ByVal id As Integer) As ActionResult
        Dim l_ua = db.UsuariosAcceso.Where(Function(x) x.idUser = id).ToList
        ViewBag.Accesos = l_ua
        GeneraViewBagEditClienteProducto()

        If Not l_ua.Any Then
            Dim ua = New UsuariosAcceso
            ua.idUser = id
            Return View(ua)
        End If

        Return View(l_ua.FirstOrDefault)
    End Function

    <HttpPost()> _
    Function Edit(ByVal ua As UsuariosAcceso) As ActionResult
        ua.idUserAutoriza = WebSecurity.CurrentUserId
        ua.fechaAutorización = Now

        If ua.idProducto = 0 Then ua.idProducto = Nothing

        Try
            db.UsuariosAcceso.Add(ua)
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return View(ua)
        End Try

        Flash.Instance.Success("", "El permiso se guardó Exitosamente")
        Return RedirectToAction("Edit", New With {.id = ua.idUser})

    End Function

    '
    ' GET: /UsuariosAcceso/Delete?idUser=idUser&idAcceso=idAcceso
    Function Delete(ByVal idUser As Integer, ByVal idAcceso As Integer) As ActionResult
        Dim ua = db.UsuariosAcceso.Where(Function(x) x.idUsuariosAcceso = idAcceso).FirstOrDefault
        Try
            db.UsuariosAcceso.Remove(ua)
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return RedirectToAction("Edit", New With {.id = idUser})
        End Try

        Flash.Instance.Success("", "El permiso se eliminó exitosamente")
        Return RedirectToAction("Edit", New With {.id = idUser})
    End Function
    Private Sub GeneraViewBagEditClienteProducto()

        Dim l_clientes As New List(Of Cliente)
        Dim l_productos As New List(Of Producto)

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_clientes = db.Cliente.ToList
        ElseIf Roles.IsUserInRole(WebSecurity.CurrentUserName, "Admin") Then
            l_clientes = ClientesController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
            'ElseIf Roles.IsUserInRole(WebSecurity.CurrentUserName, "Operador") Then
        End If


        ViewBag.idCliente = New SelectList(l_clientes, "idCliente", "NombreCliente")
        ViewBag.idProducto = New SelectList(l_productos, "idProducto", "NombreProducto")
    End Sub

    Public Function ObtenerProductos(ByVal idCliente As Integer) As ActionResult
        Dim l_productos As New List(Of Producto)
        Dim json As ContentResult

        If idCliente = 0 Then
            l_productos.Insert(0, New Producto With {.idProducto = 0, .NombreProducto = "--- Sin Productos ---", .idCliente = idCliente})
        End If

        l_productos = db.Producto.Where(Function(x) x.idCliente = idCliente).ToList
        json = Content(JsonConvert.SerializeObject(l_productos), "application/json")
        Return json

    End Function
#End Region



#Region "Permisos de Acceso"
    Public Shared Function ObtenerClientesPermitidos(ByVal idUser As Integer) As List(Of Integer)
        Dim clientes As New List(Of Integer)
        Dim db As New BaseEntities

        Dim resultados = db.UsuariosAcceso.Where(Function(x) x.idUser = idUser).Select(Function(x) x.idCliente).ToList()

        'Si solo tiene asignado un registro y  el valor es nulo entonces regresar todos los ids encontrados
        If resultados.Contains(Nothing) Then
            Return db.Cliente.Select(Function(x) x.idCliente).ToList()
        Else
            clientes.AddRange(resultados.Select(Function(x) x.Value))
            Return clientes
        End If

    End Function


    Public Shared Function ObtenerProductosPermitidos(ByVal idUser As Integer, ByVal idCliente As Integer) As List(Of Integer)
        Dim productos As New List(Of Integer)
        Dim db As New BaseEntities

        Dim accesos = db.UsuariosAcceso.Where(Function(x) x.idCliente = idCliente And x.idUser = idUser).ToList
        'Dim todosLosProductos = accesos.Where(Function(x) x.idCliente = idCliente And x.idProducto Is Nothing)
        'Dim resultados = db.UsuariosAcceso.Where(Function(x) x.idUser = idUser).Select(Function(x) x.idCliente).ToList()

        'Si tiene alguno de los acceso a todos lo productos, regresar todos los ids para el cliente seleccionado 
        If (accesos.Any(Function(x) x.idProducto Is Nothing)) Then 'Or (todosLosProductos.Count > 0) Then
            Return db.Producto.Where(Function(x) x.idCliente = idCliente).Select(Function(x) x.idProducto).ToList
        Else
            Dim productosCliente = accesos.Select(Function(x) x.idProducto).ToList() 'db.UsuariosAcceso.Where(Function(p) p.idCliente = idCliente).Select(Function(s) s.idProducto).ToList()
            productos.AddRange(productosCliente.Select(Function(x) x.Value))
            Return productos
        End If

    End Function


#End Region

End Class