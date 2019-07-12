Imports System.Data.Entity
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions
Imports System.IO
Public Class CapacitacionController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Capacitacion/

    Function Index(Optional ByVal estado As String = "activas") As ActionResult
        Dim l_capacitacion As New List(Of Capacitacion)
        Select Case estado
            Case "activas"
                l_capacitacion = db.Capacitacion.Where(Function(x) x.Activo = True).ToList
            Case "inactivas"
                l_capacitacion = db.Capacitacion.Where(Function(x) x.Activo = False).ToList
            Case Else
                l_capacitacion = db.Capacitacion.ToList
        End Select
        Return View(l_capacitacion)
    End Function

    '
    ' GET: /Capacitacion/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim capacitacion As Capacitacion = db.Capacitacion.Find(id)
        If IsNothing(capacitacion) Then
            Return HttpNotFound()
        End If
        Return View(capacitacion)
    End Function

    '
    ' GET: /Capacitacion/Create

    Function Create() As ActionResult
        GeneraViewBags()
        Return View()
    End Function

    '
    ' POST: /Capacitacion/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal capacitacion As Capacitacion, ByVal gruposSeleccionados As Integer(), ByVal temas As CapacitacionConfiguracion()) As ActionResult
        Dim temasCopy As CapacitacionConfiguracion()
        temasCopy = temas
        GeneraViewBags()
        ViewBag.gruposSeleccionados = gruposSeleccionados
        For Each tema In temasCopy
            tema.MultimediaFile = Nothing
        Next
        ViewBag.temas = temasCopy

        If Not ModelState.IsValid Then
            Flash.Instance.Error("", "Uno o más campos no son válidos, por favor verifique el formulario y capture los datos completos")
            Return View(capacitacion)
        End If

        'Llena la relacion CapacitacionGruposCapacitacionConfiguracion
        For Each _idGrupo In gruposSeleccionados
            capacitacion.Grupo.Add(db.Grupo.Where(Function(x) x.idGrupo = _idGrupo).FirstOrDefault())
        Next

        'Llena la relación CapacitacionTemas
        Dim l_temas As String() = temas.Select(Function(x) x.TituloTema).Distinct().ToArray
        Dim ct As CapacitacionTema
        Dim l_ct As New List(Of CapacitacionTema)
        Dim ca As CapacitacionArchivos
        Dim l_ca As New List(Of CapacitacionArchivos)

        Dim iTema As Integer = 0

        For Each _tema In l_temas

            Dim capConf = temas.Where(Function(x) x.TituloTema = _tema).FirstOrDefault()
            ct = New CapacitacionTema
            ct.Titulo = capConf.TituloTema
            ct.Descripcion = capConf.DescripcionTema
            ct.Orden = capConf.OrdenTema

            Dim l_archivos = temas.Where(Function(x) x.TituloTema = _tema)
            For Each _archivo In l_archivos
                ca = New CapacitacionArchivos
                ca.Titulo = _archivo.TituloArchivo
                ca.Descripcion = _archivo.DescripcionArchivo
                ca.Orden = _archivo.OrdenArchivo

                'Busca el indice del Request File para almacenarlo
                iTema = temas.ToList.FindIndex(Function(x) x.TituloTema = _tema And x.TituloArchivo = _archivo.TituloArchivo)
                If Request.Files(iTema).ContentLength > 0 Then
                    ca.URL = GuardaArchivoCapacitacion(Request.Files(iTema))
                End If

                ct.CapacitacionArchivos.Add(ca)

            Next
            l_ct.Add(ct)
        Next
        capacitacion.CapacitacionTema = l_ct

        Try
            db.Capacitacion.Add(capacitacion)
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return View(capacitacion)
        End Try

        Flash.Instance.Success("", "La capacitación ha sido creado")
        Return RedirectToAction("Index")

    End Function

    '
    ' GET: /Capacitacion/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        GeneraViewBags()
        Dim capacitacion As Capacitacion = db.Capacitacion.Find(id)

        If IsNothing(capacitacion) Then
            Return HttpNotFound()
        End If

        Dim l_cc As New List(Of CapacitacionConfiguracion)
        Dim cc As CapacitacionConfiguracion

        ViewBag.gruposSeleccionados = capacitacion.Grupo.Select(Function(x) x.idGrupo).ToArray
        For Each tema In capacitacion.CapacitacionTema
            For Each archivo In tema.CapacitacionArchivos
                cc = New CapacitacionConfiguracion
                cc.idTemaCapacitacion = tema.idTemaCapacitacion
                cc.TituloTema = tema.Titulo
                cc.DescripcionTema = tema.Descripcion
                cc.OrdenTema = tema.Orden
                cc.idCapacitacionArchivos = archivo.idCapacitacionArchivos
                cc.TituloArchivo = archivo.Titulo
                cc.DescripcionArchivo = archivo.Descripcion
                cc.URLArchivo = archivo.URL
                cc.OrdenArchivo = archivo.Orden
                l_cc.Add(cc)
            Next
        Next
        ViewBag.temas = l_cc.OrderBy(Function(x) CInt(x.idCapacitacionArchivos)).ToArray

        Return View(capacitacion)
    End Function

    '
    ' POST: /Capacitacion/Edit/5

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal capacitacion As Capacitacion, ByVal gruposSeleccionados As Integer(), ByVal temas As CapacitacionConfiguracion()) As ActionResult
        Dim temasCopy As CapacitacionConfiguracion()
        temasCopy = temas
        GeneraViewBags()
        ViewBag.gruposSeleccionados = gruposSeleccionados
        For Each tema In temasCopy
            tema.MultimediaFile = Nothing
        Next
        ViewBag.temas = temasCopy

        If Not ModelState.IsValid Then
            Flash.Instance.Error("", "Uno o más campos no son válidos, por favor verifique el formulario y capture los datos completos")
            Return View(capacitacion)
        End If

        Dim _capacitacion = db.Capacitacion.Where(Function(x) x.idCapacitacion = capacitacion.idCapacitacion).FirstOrDefault()
        _capacitacion.NombreCapacitacion = capacitacion.NombreCapacitacion
        _capacitacion.Descripcion = capacitacion.Descripcion
        _capacitacion.FechaFin = capacitacion.FechaFin
        _capacitacion.Activo = capacitacion.Activo

        'Borra todos los grupo asociados a la capacitacion
        _capacitacion.Grupo.Clear()
        db.SaveChanges()

        'Llena la relacion CapacitacionGruposCapacitacionConfiguracion
        For Each _idGrupo In gruposSeleccionados
            _capacitacion.Grupo.Add(db.Grupo.Where(Function(x) x.idGrupo = _idGrupo).FirstOrDefault())
        Next

        'Borra los temas que ya fueron eliminados del arbol
        Dim temasActuales = _capacitacion.CapacitacionTema.Select(Function(x) x.idTemaCapacitacion).ToList
        Dim temasNuevos = temas.Where(Function(x) x.idTemaCapacitacion IsNot Nothing).Select(Function(x) CInt(x.idTemaCapacitacion)).Distinct().ToList
        Dim idsEliminados = temasActuales.Except(temasNuevos)
        For Each _tema In _capacitacion.CapacitacionTema.ToArray
            If idsEliminados.Contains(_tema.idTemaCapacitacion) Then
                For Each _archivo In _tema.CapacitacionArchivos.ToArray
                    db.CapacitacionArchivos.Remove(_archivo)
                Next
                db.CapacitacionTema.Remove(_tema)
            End If
        Next
        db.SaveChanges()

        'Llena la relación CapacitacionTemas
        Dim ct As CapacitacionTema
        Dim l_ct As New List(Of CapacitacionTema)
        Dim ca As CapacitacionArchivos
        Dim l_ca As New List(Of CapacitacionArchivos)
        Dim iTema As Integer = 0
        'Primero vamos a actualizar los temas que si tienen id
        For Each _temaId As Integer In temas.Where(Function(x) x.idTemaCapacitacion IsNot Nothing).Select(Function(x) x.idTemaCapacitacion).Distinct()

            ct = db.CapacitacionTema.Find(_temaId)
            ct.Titulo = temas.Where(Function(x) x.idTemaCapacitacion = _temaId).Select(Function(x) x.TituloTema).FirstOrDefault
            ct.Descripcion = temas.Where(Function(x) x.idTemaCapacitacion = _temaId).Select(Function(x) x.DescripcionTema).FirstOrDefault
            ct.Orden = temas.Where(Function(x) x.idTemaCapacitacion = _temaId).Select(Function(x) x.OrdenTema).FirstOrDefault

            For Each _archivo In temas.Where(Function(x) x.idTemaCapacitacion = _temaId)

                ca = New CapacitacionArchivos
                'El archivo es nuevo
                If _archivo.idCapacitacionArchivos = 0 Then
                    ca.Titulo = _archivo.TituloArchivo
                    ca.Descripcion = _archivo.DescripcionArchivo
                    ca.Orden = _archivo.OrdenArchivo

                    'Busca el indice del Request File para almacenarlo
                    iTema = temas.ToList.FindIndex(Function(x) x.idTemaCapacitacion = _temaId And x.TituloArchivo = _archivo.TituloArchivo)
                    ca.URL = GuardaArchivoCapacitacion(Request.Files(iTema))

                    ct.CapacitacionArchivos.Add(ca)
                Else
                    ca = db.CapacitacionArchivos.Find(CInt(_archivo.idCapacitacionArchivos))
                    ca.Titulo = _archivo.TituloArchivo
                    ca.Descripcion = _archivo.DescripcionArchivo
                    ca.Orden = _archivo.OrdenArchivo
                    iTema = temas.ToList.FindIndex(Function(x) x.idTemaCapacitacion = _temaId And x.idCapacitacionArchivos = _archivo.idCapacitacionArchivos)
                    If Request.Files(iTema).ContentLength > 0 Then ca.URL = GuardaArchivoCapacitacion(Request.Files(iTema))
                End If

                Try
                    db.SaveChanges()
                Catch ex As Exception
                    Dim msg As String = ex.Message
                    If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
                    Flash.Instance.Error("", msg)
                    Return View(capacitacion)
                End Try
            Next
        Next

        'Ahora acutalizaremos los que no tienen ID
        For Each _tema In temas.Where(Function(x) x.idTemaCapacitacion Is Nothing).Select(Function(x) x.TituloTema).Distinct()

            Dim _temasActuales = temas.Where(Function(x) x.TituloTema = _tema).ToList

            ct = New CapacitacionTema
            ct.Titulo = _temasActuales.First().TituloTema
            ct.Descripcion = _temasActuales.First().DescripcionTema
            ct.Orden = _temasActuales.First().OrdenTema

            For Each _archivo In _temasActuales
                'El archivo es nuevo porque esta sobre un tema nuevo
                ca = New CapacitacionArchivos
                ca.Titulo = _archivo.TituloArchivo
                ca.Descripcion = _archivo.DescripcionArchivo
                ca.Orden = _archivo.OrdenArchivo

                iTema = temas.ToList.FindIndex(Function(x) x.TituloTema = _tema And x.TituloArchivo = _archivo.TituloArchivo)
                ca.URL = GuardaArchivoCapacitacion(Request.Files(iTema))

                ct.CapacitacionArchivos.Add(ca)
            Next
            _capacitacion.CapacitacionTema.Add(ct)

            Try
                db.SaveChanges()
            Catch ex As Exception
                Dim msg As String = ex.Message
                If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
                Flash.Instance.Error("", msg)
                Return View(capacitacion)
            End Try
        Next

        Flash.Instance.Success("", "La capacitación ha sido actualizada exitosamente")
        Return RedirectToAction("Index")

    End Function

    '
    ' GET: /Capacitacion/Delete/5

    Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim capacitacion As Capacitacion = db.Capacitacion.Find(id)
        If IsNothing(capacitacion) Then
            Return HttpNotFound()
        End If
        Return View(capacitacion)
    End Function

    '
    ' POST: /Capacitacion/Delete/5

    <HttpPost()> _
    <ActionName("Delete")> _
    <ValidateAntiForgeryToken()> _
    Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
        Dim capacitacion As Capacitacion = db.Capacitacion.Find(id)
        Dim idTemas = db.CapacitacionTema.Where(Function(x) x.idCapacitacion = capacitacion.idCapacitacion).Select(Function(x) x.idTemaCapacitacion).ToList
        Dim archivos = db.CapacitacionArchivos.Where(Function(x) idTemas.Contains(x.idTemaCapacitacion))

        For Each tema In capacitacion.CapacitacionTema.ToArray
            For Each archivo In tema.CapacitacionArchivos.ToArray
                If archivo.URL IsNot Nothing Then
                    Dim filePath As String = Path.Combine(Util.ObtenerParametro("CONTENT_PATH"), "Capacitacion", archivo.URL)
                    If System.IO.File.Exists(filePath) Then
                        System.IO.File.Delete(filePath)
                    End If
                End If
                db.CapacitacionArchivos.Remove(archivo)
            Next
            db.CapacitacionTema.Remove(tema)
        Next
        capacitacion.CapacitacionTema.Clear()
        capacitacion.Grupo.Clear()


        Try
            db.Capacitacion.Remove(capacitacion)
            db.SaveChanges()
        Catch ex As Exception
            Dim msg As String = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= ": " & ex.InnerException.Message
            Flash.Instance.Error("", msg)
            Return RedirectToAction("Index")
        End Try

        Flash.Instance.Success("", "Capacitación eliminada con éxito")
        Return RedirectToAction("Index")
    End Function

    '
    ' GET: /Capacitacion/Visitas

    Function Visitas() As ActionResult
        Return View(db.CapacitacionVisitas.ToList())
    End Function
    '
    ' GET: /Capacitacion/ArchivoDetalle
    Function ArchivoDetalle(ByVal id As Integer) As ActionResult
        Dim _archivo = db.CapacitacionArchivos.Where(Function(x) x.idCapacitacionArchivos = id).FirstOrDefault
        Return View(_archivo)
    End Function
    '
    ' GET: /Capacitacion/Inactividad
    Function Inactividad() As ActionResult
        Dim l_inactividad As New List(Of InactividadViewModel)
        Dim _inactividad As InactividadViewModel

        Dim _capacitaciones = db.Capacitacion.Where(Function(x) x.Activo = True).ToList 'Todas las capacitaciones Activas
        Dim idsCapacitacionArchivos = db.CapacitacionArchivos.Where(Function(x) x.CapacitacionTema.Capacitacion.Activo = True).Select(Function(x) x.idCapacitacionArchivos).ToList 'Ids CapacitacionArchivos de CapacitacionesActivas
        Dim visitasCapacitacionesActivas = db.CapacitacionVisitas.Where(Function(x) idsCapacitacionArchivos.Contains(x.idCapacitacionArchivos)).ToList 'Todas las visitas que lso archivos de capacitaciones activas

        For Each _capacitacion In _capacitaciones
            For Each _tema In _capacitacion.CapacitacionTema
                For Each _archivo In _tema.CapacitacionArchivos
                    For Each _grupo In _capacitacion.Grupo
                        For Each _usuario In _grupo.Usuarios
                            Dim visitoTema = visitasCapacitacionesActivas.Where(Function(x) x.idUsuario = _usuario.idUsuario And x.idCapacitacionArchivos = _archivo.idCapacitacionArchivos).FirstOrDefault
                            If visitoTema Is Nothing Then
                                _inactividad = New InactividadViewModel
                                With _inactividad
                                    .idCapacitacion = _capacitacion.idCapacitacion
                                    .Capacitacion = _capacitacion.NombreCapacitacion
                                    .idTema = _tema.idTemaCapacitacion
                                    .Tema = _tema.Descripcion
                                    .idArchivo = _archivo.idCapacitacionArchivos
                                    .Archivo = _archivo.Titulo
                                    .idUsuario = _usuario.idUsuario
                                    .Usuario = _usuario.Nombre.Split(" ").FirstOrDefault
                                End With
                                l_inactividad.Add(_inactividad)
                            End If
                        Next
                    Next
                Next
            Next
        Next

        Return View(l_inactividad)
    End Function
    '
    ' GET: /Capacitacion/Inactividad
    Function InactividadUsuario(ByVal id As Integer) As ActionResult
        Dim l_inactividad As New List(Of InactividadViewModel)
        Dim _inactividad As InactividadViewModel

        Dim _usuario = db.Usuarios.Where(Function(x) x.idUsuario = id).FirstOrDefault()
        Dim _capacitaciones = db.Capacitacion.Where(Function(x) x.Activo = True).ToList 'Todas las capacitaciones Activas
        Dim visitasCapacitacionesUsuario = _usuario.CapacitacionVisitas.ToList 'db.CapacitacionVisitas.Where(Function(x) x.idUsuario = id).ToList 'Todas las visitas que lo archivos de capacitaciones activas

        For Each _capacitacion In _capacitaciones
            For Each _tema In _capacitacion.CapacitacionTema
                For Each _archivo In _tema.CapacitacionArchivos
                    For Each _grupo In _capacitacion.Grupo
                        For Each _usuario In _grupo.Usuarios.Where(Function(x) x.idUsuario = id).ToList
                            If l_inactividad.Any(Function(x) x.idCapacitacion = _capacitacion.idCapacitacion And x.idTema = _tema.idTemaCapacitacion And x.idArchivo = _archivo.idCapacitacionArchivos) Then
                                Continue For
                            End If
                            Dim visitoTema = visitasCapacitacionesUsuario.Where(Function(x) x.idCapacitacionArchivos = _archivo.idCapacitacionArchivos).FirstOrDefault
                            _inactividad = New InactividadViewModel
                            _inactividad.idCapacitacion = _capacitacion.idCapacitacion
                            _inactividad.Capacitacion = _capacitacion.NombreCapacitacion.ToUpper
                            _inactividad.idTema = _tema.idTemaCapacitacion
                            _inactividad.Tema = _tema.Descripcion.ToUpper
                            _inactividad.idArchivo = _archivo.idCapacitacionArchivos
                            _inactividad.Archivo = _archivo.Titulo
                            _inactividad.idUsuario = _usuario.idUsuario
                            _inactividad.Usuario = _usuario.Nombre
                            If visitoTema Is Nothing Then
                                _inactividad.Leida = False
                            Else
                                _inactividad.Leida = True
                            End If
                            l_inactividad.Add(_inactividad)
                        Next
                    Next
                Next
            Next
        Next

        Return View(l_inactividad)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private Sub GeneraViewBags()
        Dim l_grupos = db.Grupo.ToList()
        ViewBag.idGrupos = New SelectList(l_grupos, "idGrupo", "Descripcion")
    End Sub

    Private Function GuardaArchivoCapacitacion(ByVal MultimediaFile As HttpPostedFileBase) As String
        If MultimediaFile.ContentLength > 0 Then
            Dim newFileName As String = System.Guid.NewGuid.ToString + "." + MultimediaFile.FileName.Split(".").Last()
            Dim filePath As String = Path.Combine(Util.ObtenerParametro("CONTENT_PATH"), "Capacitacion", newFileName)
            MultimediaFile.SaveAs(filePath)
            Return newFileName
        End If

        Return String.Empty
    End Function

End Class

Public Class CapacitacionConfiguracion
    Public Property idTemaCapacitacion As String
    Public Property TituloTema As String
    Public Property DescripcionTema As String
    Public Property OrdenTema As String
    Public Property idCapacitacionArchivos As String
    Public Property TituloArchivo As String
    Public Property DescripcionArchivo As String
    Public Property URLArchivo As String
    Public Property OrdenArchivo As String
    Public Property MultimediaFile As HttpPostedFileBase
End Class