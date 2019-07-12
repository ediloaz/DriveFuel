Imports System.Data.Entity
Imports WebMatrix.WebData
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions

Public Class ClientesController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Clientes/

    Function Index() As ActionResult
        Return View(db.Cliente.ToList())
    End Function

    '
    ' GET: /Clientes/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim cliente As Cliente = db.Cliente.Find(id)
        If IsNothing(cliente) Then
            Return HttpNotFound()
        End If
        Return View(cliente)
    End Function

    '
    ' GET: /Clientes/Create

    Function Create() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Clientes/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal cliente As Cliente) As ActionResult
        If ModelState.IsValid Then
            db.Cliente.Add(cliente)
            db.SaveChanges()
            Flash.Instance.Success("", "El cliente se ha guardado exitosamente")
            Return RedirectToAction("Index")
        End If

        Return View(cliente)
    End Function

    '
    ' GET: /Clientes/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim cliente As Cliente = db.Cliente.Find(id)
        If IsNothing(cliente) Then
            Return HttpNotFound()
        End If
        Return View(cliente)
    End Function

    '
    ' POST: /Clientes/Edit/5

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal cliente As Cliente) As ActionResult
        If ModelState.IsValid Then
            Dim _cliente = db.Cliente.Where(Function(x) x.idCliente = cliente.idCliente).FirstOrDefault()
            _cliente.NombreCliente = cliente.NombreCliente
            _cliente.Activo = cliente.Activo
            db.SaveChanges()
            Flash.Instance.Success("", "El cliente se ha actualizado exitosamente")
            Return RedirectToAction("Index")
        End If

        Return View(cliente)
    End Function

    '
    ' GET: /Clientes/Delete/5
    Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim cliente As Cliente = db.Cliente.Find(id)
        If IsNothing(cliente) Then
            Return HttpNotFound()
        End If
        Return View(cliente)
    End Function

    '
    ' POST: /Clientes/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
        Dim cliente As Cliente = db.Cliente.Find(id)
        db.Cliente.Remove(cliente)
        Try
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return RedirectToAction("Index")
        End Try
        Flash.Instance.Success("", "El cliente se ha eliminado exitosamente")
        Return RedirectToAction("Index")
    End Function
    Public Shared Function ObtenerClientesPermitidos(ByVal idUser As Integer)
        Dim dbc As New BaseEntities
        Dim l_clientes As New List(Of Cliente)

        Dim Ids_ClientesPemitidos = UsuariosAccesoController.ObtenerClientesPermitidos(idUser)
        l_clientes = dbc.Cliente.Where(Function(x) Ids_ClientesPemitidos.Contains(x.idCliente)).ToList

        Return l_clientes
    End Function
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class