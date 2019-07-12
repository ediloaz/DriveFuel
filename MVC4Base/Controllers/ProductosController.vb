Imports System.Data.Entity
Imports Newtonsoft.Json
Imports WebMatrix.WebData
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions

Public Class ProductosController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Productos/

    Function Index() As ActionResult
        Dim producto = db.Producto.Include(Function(p) p.Cliente)
        Return View(producto.ToList())
    End Function

    '
    ' GET: /Productos/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim producto As Producto = db.Producto.Find(id)
        If IsNothing(producto) Then
            Return HttpNotFound()
        End If
        Return View(producto)
    End Function

    '
    ' GET: /Productos/Create

    Function Create() As ActionResult
        ViewBag.idCliente = New SelectList(db.Cliente, "idCliente", "NombreCliente")
        Return View()
    End Function

    '
    ' POST: /Productos/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal producto As Producto) As ActionResult
        If ModelState.IsValid Then
            db.Producto.Add(producto)
            db.SaveChanges()
            Flash.Instance.Success("", "El producto se ha guardado exitosamente")
            Return RedirectToAction("Index")
        End If

        ViewBag.idCliente = New SelectList(db.Cliente, "idCliente", "NombreCliente", producto.idCliente)
        Return View(producto)
    End Function

    '
    ' GET: /Productos/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim producto As Producto = db.Producto.Find(id)
        If IsNothing(producto) Then
            Return HttpNotFound()
        End If
        ViewBag.idCliente = New SelectList(db.Cliente, "idCliente", "NombreCliente", producto.idCliente)
        Return View(producto)
    End Function

    '
    ' POST: /Productos/Edit/5

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal producto As Producto) As ActionResult
        Dim _producto = db.Producto.Where(Function(x) x.idProducto = producto.idProducto).FirstOrDefault

        If ModelState.IsValid Then
            _producto.idCliente = producto.idCliente
            _producto.NombreProducto = producto.NombreProducto
            _producto.Clave = producto.Clave
            _producto.Activo = producto.Activo
            db.SaveChanges()
            Flash.Instance.Success("", "El producto se ha actualizado exitosamente")
            Return RedirectToAction("Index")
        End If

        ViewBag.idCliente = New SelectList(db.Cliente, "idCliente", "NombreCliente", producto.idCliente)
        Return View(producto)
    End Function

    '
    ' GET: /Productos/Delete/5

    Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim producto As Producto = db.Producto.Find(id)
        If IsNothing(producto) Then
            Return HttpNotFound()
        End If
        Return View(producto)
    End Function

    '
    ' POST: /Productos/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
        Dim producto As Producto = db.Producto.Find(id)
        db.Producto.Remove(producto)
        Try
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return RedirectToAction("Index")
        End Try
        Flash.Instance.Success("", "El producto se ha eliminado exitosamente")
        Return RedirectToAction("Index")
    End Function

    Public Function ObtenerProductos(ByVal idCliente As Integer) As ActionResult

        Dim l_productos As New List(Of Producto)

        If idCliente = 0 Then
            l_productos.Insert(0, New Producto With {.idProducto = 0, .NombreProducto = "--- Sin Productos ---", .idCliente = idCliente})

        End If

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_productos = db.Producto.Where(Function(x) x.idCliente = idCliente).ToList

        Else
            Dim ids_productos = UsuariosAccesoController.ObtenerProductosPermitidos(WebSecurity.CurrentUserId, idCliente)
            l_productos = db.Producto.Where(Function(x) ids_productos.Contains(x.idProducto)).ToList
        End If

        If l_productos.Count = 0 Then
            l_productos.Insert(0, New Producto With {.idProducto = 0, .NombreProducto = "--- Sin Productos ---", .idCliente = idCliente})
        End If

        Dim json As ContentResult
        json = Content(JsonConvert.SerializeObject(l_productos), "application/json")
        Return json

    End Function
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class