Imports System.Data.Entity
Imports WebMatrix.WebData

Public Class FormasController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Formas/

    Function Index() As ActionResult
        Return View(db.Forma.Include("FormaPregunta").ToList())
    End Function

    '
    ' GET: /Formas/Create

    Function Create() As ActionResult
        GeneraViewBagClienteProducto()
        Return View()
    End Function

    '
    ' POST: /Formas/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal forma As Forma) As ActionResult
        If ModelState.IsValid Then
            db.Forma.Add(forma)
            db.SaveChanges()
            Return RedirectToAction("Edit", New With {.id = forma.IdForma})
        End If

        Return View(forma)
    End Function

    '
    ' GET: /Formas/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim forma As Forma = db.Forma.Find(id)
        GeneraViewBagEditClienteProducto(forma)

        If IsNothing(forma) Then
            Return HttpNotFound()
        End If
       
        Return View(forma)
    End Function

    '
    ' GET: /Formas/Delete/5

    Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim forma As Forma = db.Forma.Find(id)
        If IsNothing(forma) Then
            Return HttpNotFound()
        End If
        Return View(forma)
    End Function

    '
    ' POST: /Formas/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
        Dim forma As Forma = db.Forma.Find(id)
        db.Forma.Remove(forma)
        db.SaveChanges()
        Return RedirectToAction("Index")
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private Sub GeneraViewBagClienteProducto()

        Dim l_clientes As New List(Of Cliente)
        Dim l_productos As New List(Of Producto)

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_clientes = db.Cliente.ToList
        Else
            l_clientes = ClientesController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
        End If

        ViewBag.idCliente = New SelectList(l_clientes, "idCliente", "NombreCliente")
        ViewBag.idProducto = New SelectList(l_productos, "idProducto", "NombreProducto")
    End Sub

    Private Sub GeneraViewBagEditClienteProducto(ByVal forma As Forma)

        Dim l_clientes As New List(Of Cliente)
        Dim l_productos As New List(Of Producto)

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_clientes = db.Cliente.ToList
        Else
            l_clientes = ClientesController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
        End If
        l_productos = db.Producto.Where(Function(x) x.idCliente = forma.idCliente).ToList

        ViewBag.idCliente = New SelectList(l_clientes, "idCliente", "NombreCliente", forma.idCliente)
        ViewBag.idProducto = New SelectList(l_productos, "idProducto", "NombreProducto", forma.idProducto)
    End Sub
End Class