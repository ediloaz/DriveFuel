Imports System.Data.Entity
Imports System.IO
Imports Newtonsoft.Json
Imports WebMatrix.WebData

Public Class GruposController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Grupos/

    Function Index() As ActionResult
        Return View(db.Grupo.ToList())
    End Function

    '
    ' GET: /Grupos/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim grupo As Grupo = db.Grupo.Find(id)
        If IsNothing(grupo) Then
            Return HttpNotFound()
        End If
        Return View(grupo)
    End Function

    '
    ' GET: /Grupos/Create

    Function Create() As ActionResult
       GeneraViewBagClienteProducto()
        Return View()
    End Function

    '
    ' POST: /Grupos/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal grupo As Grupo) As ActionResult
        If ModelState.IsValid Then
            db.Grupo.Add(grupo)
            db.SaveChanges()
            Return RedirectToAction("Edit", New With {.id = grupo.idGrupo})
        End If

        Return View(grupo)
    End Function

    '
    ' GET: /Grupos/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim grupo As Grupo = db.Grupo.Find(id)
        GeneraViewBagEditClienteProducto(grupo)
        If IsNothing(grupo) Then
            Return HttpNotFound()
        End If
        
        ViewBag.id = id
        Return View(grupo)
    End Function

    '
    ' POST: /Grupos/Edit/5

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal grupo As Grupo) As ActionResult
        If ModelState.IsValid Then
            db.Entry(grupo).State = EntityState.Modified
            db.SaveChanges()
            Return RedirectToAction("Index")
        End If

        Return View(grupo)
    End Function

    '
    ' GET: /Grupos/Delete/5

    Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim grupo As Grupo = db.Grupo.Find(id)
        If IsNothing(grupo) Then
            Return HttpNotFound()
        End If
        Return View(grupo)
    End Function

    '
    ' POST: /Grupos/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
        Dim grupo As Grupo = db.Grupo.Find(id)
        grupo.Usuarios.Clear()
        grupo.Ruta.Clear()
        db.Grupo.Remove(grupo)
        db.SaveChanges()
        Return RedirectToAction("Index")
    End Function

    Function Usuarios(ByVal id As Integer) As ActionResult
        Dim grupo = db.Grupo.Find(id)
        If grupo IsNot Nothing Then
            Dim usuariosIds = (From u In grupo.Usuarios Select u.idUsuario).ToArray()
            Return Json(New With {.usuarios = usuariosIds}, behavior:=JsonRequestBehavior.AllowGet)
        End If
        Return New HttpStatusCodeResult(404)
    End Function

    <HttpPost()> _
    Function Usuarios(ByVal id As Integer, ByVal ids As Integer()) As ActionResult
        Dim req As Stream = Request.InputStream
        req.Seek(0, System.IO.SeekOrigin.Begin)
        Dim sr As StreamReader = New StreamReader(req)
        Dim json As String = sr.ReadToEnd()
        Dim input As UsuariosGruposApiViewModel = Nothing
        Try
            input = JsonConvert.DeserializeObject(Of UsuariosGruposApiViewModel)(json)
        Catch ex As Exception
            Return New HttpStatusCodeResult(500)
        End Try
        Dim grupo = db.Grupo.Find(id)
        grupo.Usuarios.Clear()
        db.SaveChanges()
        For Each usuario As Usuarios In From u In db.Usuarios Where input.usuarios.Contains(u.idUsuario) Select u
            grupo.Usuarios.Add(usuario)
        Next
        db.SaveChanges()
        Return New HttpStatusCodeResult(200)
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

    Private Sub GeneraViewBagEditClienteProducto(ByVal grupo As Grupo)
        Dim l_clientes As New List(Of Cliente)
        Dim l_productos As New List(Of Producto)

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_clientes = db.Cliente.ToList
        Else
            l_clientes = ClientesController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
        End If
        l_productos = db.Producto.Where(Function(x) x.idCliente = grupo.idCliente).ToList

        ViewBag.idCliente = New SelectList(l_clientes, "idCliente", "NombreCliente", grupo.idCliente)
        ViewBag.idProducto = New SelectList(l_productos, "idProducto", "NombreProducto", grupo.idProducto)
    End Sub

End Class