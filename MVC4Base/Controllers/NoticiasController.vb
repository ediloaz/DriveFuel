Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions
Imports System.IO
Imports WebMatrix.WebData

Public Class NoticiasController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities
    '
    ' GET: /Noticias

    Function Index(Optional ByVal estado As String = "activas") As ActionResult
        Return View(ObtenerNoticias(estado))
    End Function

    Function Create() As ActionResult
        GeneraViewBags()
        Return View()
    End Function
    <HttpPost()> _
    Function Create(ByVal noticia As Noticias, ByVal gruposSeleccionados As Integer()) As ActionResult
        GeneraViewBags()
        If Not ModelState.IsValid Then
            Flash.Instance.Error("", "Uno o más campos no son válidos, por favor verifique el formulario y capture los datos completos")
            Return View(noticia)
        End If

        'Si hay una imagen en el modelo extendido se convierte en base64 y se actualiza la variable de imagen del modelo original
        If noticia.ImageFile IsNot Nothing Then
            'Dim fs As System.IO.Stream = noticia.ImageFile.InputStream
            'Dim br As System.IO.BinaryReader = New System.IO.BinaryReader(fs)
            'Dim bytes As Byte() = br.ReadBytes(fs.Length)
            'Dim sImagen As String = Convert.ToBase64String(bytes, 0, bytes.Length)
            noticia.Imagen = GuardaArchivoNoticia(noticia.ImageFile)
        End If

        'Llena la relacion NoticiasGrupo
        For Each _idGrupo In gruposSeleccionados
            noticia.Grupo.Add(db.Grupo.Where(Function(x) x.idGrupo = _idGrupo).FirstOrDefault())
        Next

        Try
            db.Noticias.Add(noticia)
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return View(noticia)
        End Try

        Flash.Instance.Success("", "La noticia se ha guardado exitosamente")
        Return View("Index", ObtenerNoticias("activas"))
    End Function
    Function Edit(ByVal id As Integer) As ActionResult
        Dim noticia = db.Noticias.Where(Function(x) x.idNoticia = id).FirstOrDefault

        If noticia Is Nothing Then
            Flash.Instance.Error("", "No se encontró la noticia que desea editar")
            Return View("Index")
        End If

        Dim l_tipoNoticia = db.TipoNoticia.ToList()
        ViewBag.TiposNoticia = New SelectList(l_tipoNoticia, "idTipoNoticia", "Descripcion", noticia.idNoticia)
        ViewBag.gruposSeleccionados = noticia.Grupo.Select(Function(x) x.idGrupo).ToArray

        Dim l_grupos = db.Grupo.ToList()
        ViewBag.idGrupos = New SelectList(l_grupos, "idGrupo", "Descripcion")

        Return View(noticia)
    End Function
    <HttpPost()> _
    Function Edit(ByVal model As Noticias, ByVal gruposSeleccionados As Integer()) As ActionResult
        GeneraViewBags()

        Dim noticia = db.Noticias.Where(Function(x) x.idNoticia = model.idNoticia).FirstOrDefault
        With model
            noticia.idTipoNoticia = model.idTipoNoticia
            noticia.FechaInicio = .FechaInicio
            noticia.FechaFin = .FechaFin
            noticia.Titulo = .Titulo
            noticia.Mensaje = .Mensaje
            noticia.Activa = .Activa
        End With

        'Si hay una imagen en el modelo se almacena
        If model.ImageFile IsNot Nothing Then
            noticia.Imagen = GuardaArchivoNoticia(model.ImageFile)
        End If

        'Borra todos los grupo asociados a la capacitacion
        noticia.Grupo.Clear()
        db.SaveChanges()

        'Llena la relacion CapacitacionGruposCapacitacionConfiguracion
        For Each _idGrupo In gruposSeleccionados
            noticia.Grupo.Add(db.Grupo.Where(Function(x) x.idGrupo = _idGrupo).FirstOrDefault())
        Next

        Try
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return View(noticia)
        End Try

        Flash.Instance.Success("", "La noticia se actualizó exitosamente")
        Return View("Index", ObtenerNoticias("activas"))
    End Function

    Function Delete(ByVal id As Integer) As ActionResult
        Dim noticia = db.Noticias.Where(Function(x) x.idNoticia = id).FirstOrDefault

        If noticia Is Nothing Then
            Flash.Instance.Error("", "No se encontró la noticia que desea editar")
            Return View("Index")
        End If

        Return View(noticia)
    End Function

    <HttpPost()> _
    Function Delete(ByVal noticia As Noticias) As ActionResult
        Dim _noticia = db.Noticias.Where(Function(x) x.idNoticia = noticia.idNoticia).FirstOrDefault

        If _noticia Is Nothing Then
            Flash.Instance.Error("", "No se encontró la noticia que desea eliminar")
            Return View("Index")
        End If

        db.Noticias.Remove(_noticia)

        Try
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return View("Index")
        End Try

        Flash.Instance.Success("", "La noticia se eliminó exitosamente")
        Return View("Index", ObtenerNoticias("activas"))
    End Function

    Private Function ObtenerNoticias(ByVal estado As String) As List(Of Noticias)

        Dim l_noticias As New List(Of Noticias)
        Select Case estado
            Case "activas"
                l_noticias = db.Noticias.Where(Function(x) x.Activa = True).ToList
            Case "inactivas"
                l_noticias = db.Noticias.Where(Function(x) x.Activa = False).ToList
            Case Else
                l_noticias = db.Noticias.OrderByDescending(Function(x) x.idNoticia).ToList()
        End Select
        Return l_noticias
    End Function

    Private Sub GeneraViewBags()

        Dim l_grupos As New List(Of Grupo)

        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_grupos = db.Grupo.ToList
        Else
            Dim Ids_ClientesPermitidos = UsuariosAccesoController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
            Dim Ids_ProductosPermitidos As New List(Of Integer)
            For Each idCliente In Ids_ClientesPermitidos
                Ids_ProductosPermitidos.AddRange(UsuariosAccesoController.ObtenerProductosPermitidos(WebSecurity.CurrentUserId, idCliente))
            Next
            l_grupos = db.Grupo.Where(Function(x) Ids_ProductosPermitidos.Contains(x.idProducto)).ToList
        End If


        Dim l_tipoNoticia = db.TipoNoticia.ToList()

        ViewBag.idGrupos = New SelectList(l_grupos, "idGrupo", "Descripcion")
        ViewBag.idTipoNoticia = New SelectList(l_tipoNoticia, "idTipoNoticia", "Descripcion")
    End Sub

    Private Function GuardaArchivoNoticia(ByVal MultimediaFile As HttpPostedFileBase) As String
        If MultimediaFile.ContentLength > 0 Then
            Dim newFileName As String = System.Guid.NewGuid.ToString + "." + MultimediaFile.FileName.Split(".").Last()
            Dim filePath As String = Path.Combine(Util.ObtenerParametro("CONTENT_PATH"), "Noticias", newFileName)
            MultimediaFile.SaveAs(filePath)
            Return newFileName
        End If

        Return String.Empty
    End Function
End Class