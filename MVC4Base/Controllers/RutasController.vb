Imports System.IO
Imports Newtonsoft.Json
Imports WebMatrix.WebData

Public Class RutasController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Rutas/

    Function Index() As ActionResult
        ViewBag.UsuarioActual = User.Identity.Name
        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "Cliente") Then
            ViewBag.RoleActual = "Cliente"
        End If
        Dim consulta = (From r In db.Ruta.Include("RutaCheckpoint").Include("Grupo") Order By r.idRuta Descending).Distinct.ToList()
        Return View(consulta)
    End Function

    '
    ' GET: /Rutas

    Function Create() As ActionResult
        GeneraViewBagClienteProducto()
        ViewBag.mapApiKey = "AIzaSyB5Mo7NODp6OV-XY949xwgX51iALjnco00"
        Return View()
    End Function

    Function Edit(ByVal id As Integer) As ActionResult
        ViewBag.idRuta = id
        GeneraViewBagEditClienteProducto(id)
        ViewBag.mapApiKey = "AIzaSyB5Mo7NODp6OV-XY949xwgX51iALjnco00"
        Return View("Create")
    End Function

    Function Usuarios(ByVal id As Integer) As ActionResult
        Dim ruta = db.Ruta.Find(id)
        If ruta IsNot Nothing Then
            Dim usuariosIds = (From u In ruta.Usuarios Select u.idUsuario).ToArray()
            Dim gruposIds = (From u In ruta.Grupo Select u.idGrupo).ToArray()
            Return Json(New With {.usuarios = usuariosIds, .grupos = gruposIds}, behavior:=JsonRequestBehavior.AllowGet)
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
            'Dim var1 = New With {Key .usuarios = {1, 2}, Key .grupos = {1, 2}}
            input = JsonConvert.DeserializeObject(Of UsuariosGruposApiViewModel)(json)
            'input = JsonConvert.DeserializeObject(Of Dictionary(Of String, Integer()))(json)
        Catch ex As Exception
            Return New HttpStatusCodeResult(500)
        End Try
        Dim ruta = db.Ruta.Find(id)
        ruta.Usuarios.Clear()
        ruta.Grupo.Clear()
        db.SaveChanges()
        For Each usuario As Usuarios In From u In db.Usuarios Where input.usuarios.Contains(u.idUsuario) Select u
            ruta.Usuarios.Add(usuario)
        Next
        For Each grupo As Grupo In From u In db.Grupo Where input.grupos.Contains(u.idGrupo) Select u
            ruta.Grupo.Add(grupo)
        Next
        db.SaveChanges()
        Return New HttpStatusCodeResult(200)
    End Function

    Function Formas(ByVal id As Integer) As ActionResult
        Dim ruta = db.Ruta.Find(id)
        If ruta IsNot Nothing Then
            Dim formasIds = (From u In ruta.Forma Select u.IdForma).ToArray()
            Return Json(formasIds, behavior:=JsonRequestBehavior.AllowGet)
        End If
        Return New HttpStatusCodeResult(404)
    End Function

    <HttpPost()> _
    Function Formas(ByVal id As Integer, ByVal ids As Integer()) As ActionResult
        Dim req As Stream = Request.InputStream
        req.Seek(0, System.IO.SeekOrigin.Begin)
        Dim sr As StreamReader = New StreamReader(req)
        Dim json As String = sr.ReadToEnd()
        Dim input As Integer() = Nothing
        Try
            input = JsonConvert.DeserializeObject(Of Integer())(json)
        Catch ex As Exception
            Return New HttpStatusCodeResult(500)
        End Try
        Dim ruta = db.Ruta.Find(id)
        ruta.Forma.Clear()
        db.SaveChanges()
        For Each forma As Forma In From u In db.Forma Where input.Contains(u.IdForma) Select u
            ruta.Forma.Add(forma)
        Next
        db.SaveChanges()
        Return New HttpStatusCodeResult(200)
    End Function

    Function Respuestas(ByVal id As Integer, ByVal id2 As Integer) As ActionResult
        Dim forma = db.Forma.Find(id2)
        If forma IsNot Nothing Then
            ViewBag.forma = forma
            Dim baseUri = New Uri(Util.ObtenerParametro("OFICIAL_WEB_PAGE"))
            Dim contentPathUri = New Uri(baseUri, Util.ObtenerParametro("CONTENT_VIRTUAL_PATH"))
            ViewBag.contentPath = contentPathUri.AbsoluteUri
            Dim objs = (From r In db.FormaRespuesta Where r.idRutaCheckpoint = id And r.idForma = id2 Select r).ToArray()
            Return View("Respuestas", objs)
        End If
        Return New HttpStatusCodeResult(404)
    End Function

    Function RespuestasFormas(ByVal id As Integer) As ActionResult
        Dim ruta = db.Ruta.Find(id)
        If ruta IsNot Nothing Then
            ViewBag.idRuta = id
            Dim checks = (From c In ruta.RutaCheckpoint Select c.idRutaCheckPoint).ToArray()
            Dim respuestas = (From r In db.FormaRespuesta Where checks.Contains(r.idRutaCheckpoint) Select New With {
                 .idRutaCheckpoint = r.idRutaCheckpoint,
                 .idForma = r.idForma,
                 .checkPoint = r.RutaCheckpoint.Descripcion,
                 .forma = r.Forma.Descripcion
             }).Distinct().ToArray()
            Return View("RespuestasFormas", respuestas)
        End If
        Return New HttpStatusCodeResult(404)
    End Function

    Function Checks(ByVal id As Integer) As ActionResult
        Dim ruta = db.Ruta.Find(id)
        If ruta IsNot Nothing Then
            ViewBag.idRuta = id
            ViewBag.respuestas = (From r In ruta.RutaCheckpoint
                    Select r.Descripcion, checkins = (From c In r.CheckIn Group By c.Usuarios Into Group).Take(10).ToArray()).Take(5).ToArray()
            Return View("Checks")
        End If
        Return New HttpStatusCodeResult(404)
    End Function

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Checks(ByVal id As Integer, ByVal model As CheckinsSearch) As ActionResult
        Dim ruta = db.Ruta.Find(id)
        If ruta IsNot Nothing Then
            ViewBag.idRuta = id
            If model.usuario IsNot Nothing And model.fecha_ini IsNot Nothing Then
                ViewBag.respuestas = (From r In ruta.RutaCheckpoint
                        Select r.Descripcion, checkins = (From c In r.CheckIn
                        Where c.Usuarios.Nombre.ToLower().Contains(model.usuario.ToLower()) And c.Fecha >= model.fecha_ini And c.Fecha <= model.fecha_fin
                        Group By c.Usuarios Into Group).ToArray()).ToArray()

            ElseIf model.usuario IsNot Nothing Then
                ViewBag.respuestas = (From r In ruta.RutaCheckpoint
                        Select r.Descripcion, checkins = (From c In r.CheckIn
                        Where c.Usuarios.Nombre.ToLower().Contains(model.usuario.ToLower())
                        Group By c.Usuarios Into Group).ToArray()).ToArray()
            ElseIf model.fecha_ini IsNot Nothing Then
                ViewBag.respuestas = (From r In ruta.RutaCheckpoint
                        Select r.Descripcion, checkins = (From c In r.CheckIn
                        Where c.Fecha >= model.fecha_ini And c.Fecha <= model.fecha_fin
                        Group By c.Usuarios Into Group).ToArray()).ToArray()
            Else
                ViewBag.respuestas = (From r In ruta.RutaCheckpoint
                    Select r.Descripcion, checkins = (From c In r.CheckIn Group By c.Usuarios Into Group).Take(10).ToArray()).Take(5).ToArray()
            End If
        End If
        Return View("Checks", model)
        Return New HttpStatusCodeResult(404)
    End Function

    'Function Checkpoints(ByVal id As Integer) As ActionResult
    '    Dim ruta = db.Ruta.Find(id)
    '    If ruta IsNot Nothing Then
    '        Dim checks = (From c In ruta.RutaCheckpoint Select c).ToArray()
    '        Return View("Checkpoints", checks)
    '    End If
    '    Return New HttpStatusCodeResult(404)
    'End Function


    '
    ' GET: /RutasTemp/Delete/5

    Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim ruta As Ruta = db.Ruta.Find(id)
        ruta.RutaCheckpoint.Clear()
        If IsNothing(ruta) Then
            Return HttpNotFound()
        End If
        Return View(ruta)
    End Function

    '
    ' POST: /RutasTemp/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
        Dim ruta As Ruta = db.Ruta.Find(id)
        For Each item In ruta.RutaCheckpoint.ToArray
            db.RutaCheckpoint.Remove(item)
        Next
        ruta.Usuarios.Clear()
        ruta.Grupo.Clear()
        ruta.Forma.Clear()
        ruta.RutaCheckpoint.Clear()
        db.SaveChanges()
        db.Ruta.Remove(ruta)
        db.SaveChanges()
        Return RedirectToAction("Index")
    End Function

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

    Private Sub GeneraViewBagEditClienteProducto(ByVal idRuta As Integer)
        Dim l_clientes As New List(Of Cliente)
        Dim l_productos As New List(Of Producto)

        Dim _ruta = db.Ruta.Where(Function(x) x.idRuta = idRuta).FirstOrDefault

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_clientes = db.Cliente.ToList
        Else
            l_clientes = ClientesController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
        End If
        l_productos = db.Producto.Where(Function(x) x.idCliente = _ruta.idCliente).ToList


        ViewBag.idCliente = New SelectList(l_clientes, "idCliente", "NombreCliente")
        ViewBag.idProducto = New SelectList(l_productos, "idProducto", "NombreProducto")
    End Sub

End Class