Imports System.Data.Entity

Public Class ReportesController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Reportes/

    Function Index() As ActionResult
        Return View(db.Ruta.ToList())
    End Function

    '
    ' GET: /Reportes/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim ruta As Ruta = db.Ruta.Include("RutaCheckpoint").Include("Grupo").SingleOrDefault(Function(x) x.idRuta = id)
        If Request.QueryString.Get("pdf") IsNot Nothing OrElse Request.QueryString.Get("excel") IsNot Nothing Then
            Dim idsUsuario As New List(Of Integer)
            For Each grupo As Grupo In ruta.Grupo
                idsUsuario.AddRange(grupo.Usuarios.Select(Function(u) u.idUsuario).ToArray())
            Next
            idsUsuario = idsUsuario.Distinct().ToList()

            Dim usuarios As New List(Of Usuarios)

            For Each usuario As Usuarios In (From u In db.Usuarios.Include("Grupo").Include("FormaRespuesta") Where idsUsuario.Contains(u.idUsuario) Select u).ToArray()
                usuario.GruposCount = usuario.Grupo.Count
                usuario.RutasCount = usuario.Grupo.SelectMany(Function(g) g.Ruta.Select(Function(r) r.idRuta)).Distinct().Count()
                usuario.Checkins = usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(c) c.idCheckin).Count()
                usuario.Faltas = ruta.RutaCheckpoint.Count - usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(c) c.idCheckpoint).Distinct().Count()
                usuario.Cuestionarios = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(fr) fr.idFormaRespuesta).Count()
                usuario.Multimedia = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(fr) fr.FormaRespuestaDetalle.Select(Function(d) d.FormaPregunta.Tipo = 6).Count()).Sum()
                usuario.Capacitacion = usuario.CapacitacionVisitas.Count
                usuario.FormaRespuesta = (From fr In usuario.FormaRespuesta Where fr.RutaCheckpoint.idRuta = ruta.idRuta Select fr).ToArray()
                usuario.Ruta = usuario.Grupo.SelectMany(Function(g) g.Ruta).ToList()
                usuarios.Add(usuario)
            Next

            If Request.QueryString.Get("pdf") IsNot Nothing Then
                ViewBag.esPdf = True
                Return New Rotativa.ViewAsPdf("DetailsPdf", usuarios)
            ElseIf Request.QueryString.Get("excel") IsNot Nothing Then
                Dim csv As String = String.Empty
                Dim tiposFormaOptions As Boolean() = {False, False, False, True, True, True, False}
                For Each usuario As Usuarios In usuarios
                    csv += "Nombre,Grupos,Rutas" + vbCrLf
                    csv += usuario.Nombre + "," + usuario.GruposCount.ToString() + "," + usuario.RutasCount.ToString() + vbCrLf
                    csv += "Checkins,Faltas,Cuestionarios,Multimedia,Capacitación" + vbCrLf
                    csv += usuario.Checkins.ToString() + "," + usuario.Faltas.ToString() + "," + usuario.Cuestionarios.ToString() + "," + usuario.Multimedia.ToString() + "," + usuario.Capacitacion.ToString() + "," + vbCrLf
                    csv += "Rutas" + vbCrLf
                    csv += String.Join(",", usuario.Ruta.Select(Function(r) r.Descripcion)) + vbCrLf
                    csv += "Grupos" + vbCrLf
                    csv += String.Join(",", usuario.Grupo.Select(Function(r) r.Descripcion)) + vbCrLf
                    csv += vbCrLf + "Cuestionarios" + vbCrLf
                    For Each respuesta As FormaRespuesta In usuario.FormaRespuesta
                        csv += respuesta.Forma.Descripcion + "," + respuesta.FechaFin.ToString("dd/MM/hh:mm tt") + vbCrLf
                        For Each pregunta As FormaPregunta In respuesta.Forma.FormaPregunta.Where(Function(p) p.Tipo <> 6)
                            Dim respuestaTexto As String = ""
                            If {3, 5}.Contains(pregunta.Tipo) Then
                                Dim detalles = (From d In respuesta.FormaRespuestaDetalle2 Where d.idFormaPregunta = pregunta.idFormaPregunta Select d.FormaPreguntaOpcion.Opcion).ToArray
                                respuestaTexto = String.Join(", ", detalles)
                            Else
                                Dim detalle = (From d In respuesta.FormaRespuestaDetalle Where d.idFormaPregunta = pregunta.idFormaPregunta Select d).FirstOrDefault
                                If detalle Is Nothing Then
                                    respuestaTexto = ""
                                Else
                                    If tiposFormaOptions(pregunta.Tipo) Then
                                        respuestaTexto = ""
                                        If detalle.FormaPreguntaOpcion IsNot Nothing Then
                                            respuestaTexto = detalle.FormaPreguntaOpcion.Opcion
                                        End If
                                    Else
                                        respuestaTexto = detalle.Respuesta
                                    End If
                                End If
                            End If
                            csv += pregunta.Pregunta + "," + respuestaTexto + vbCrLf
                        Next
                        csv += vbCrLf
                    Next
                    csv += vbCrLf + vbCrLf + vbCrLf
                Next
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=Reporte.csv")
                Response.Charset = Encoding.UTF8.WebName
                Response.ContentType = "application/text"
                Response.Output.Write(csv)
                Response.Flush()
                Response.End()
            End If
        End If
        For Each grupo As Grupo In ruta.Grupo
            Dim idUsuarios As Integer() = grupo.Usuarios.Select(Function(u) u.idUsuario).ToArray()
            grupo.PromotoresActivos = grupo.Usuarios.Count
            Dim usuariosCheckin As Integer = ruta.RutaCheckpoint.SelectMany(Function(rc) rc.CheckIn.Select(Function(c) c.Usuarios.idUsuario)).Distinct().Count()
            If usuariosCheckin > 0 Then
                grupo.CheckinPorcentaje = Math.Round(usuariosCheckin / grupo.PromotoresActivos * 100)
            Else
                grupo.CheckinPorcentaje = 0
            End If
            grupo.Cuestionarios = ruta.RutaCheckpoint.Select(Function(rc) rc.FormaRespuesta.Where(Function(fr) idUsuarios.Contains(fr.idUsuario)).Count).Sum()
            grupo.Fotos = ruta.RutaCheckpoint.Select(Function(rc) rc.FormaRespuesta.Where(Function(fr) idUsuarios.Contains(fr.idUsuario)).Select(Function(fr) fr.FormaRespuestaDetalle.Where(Function(d) d.FormaPregunta.Tipo = 6))).Sum(Function(y) y.Count())
            grupo.Llegada = ruta.RutaCheckpoint.OrderBy(Function(rc) rc.Llegada).FirstOrDefault().Llegada
            grupo.Salida = ruta.RutaCheckpoint.OrderByDescending(Function(rc) rc.Llegada).FirstOrDefault().Salida
        Next
        Return View(ruta)
    End Function

    '
    ' GET: /Reportes/RutaGrupo/6

    Function RutaGrupo(Optional ByVal id As Integer = Nothing, Optional ByVal id2 As Integer = Nothing) As ActionResult
        Dim ruta As Ruta = db.Ruta.Include("RutaCheckpoint").Include("Grupo").SingleOrDefault(Function(x) x.idRuta = id)
        Dim grupo As Grupo = (From g In ruta.Grupo Where g.idGrupo = id2).SingleOrDefault()

        For Each usuario As Usuarios In Grupo.Usuarios
            usuario.GruposCount = usuario.Grupo.Count
            usuario.RutasCount = usuario.Grupo.SelectMany(Function(g) g.Ruta.Select(Function(r) r.idRuta)).Distinct().Count()
            usuario.Checkins = usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(c) c.idCheckin).Count()
            usuario.Faltas = ruta.RutaCheckpoint.Count - usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(c) c.idCheckpoint).Distinct().Count()
            usuario.Cuestionarios = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(fr) fr.idFormaRespuesta).Count()
            usuario.Multimedia = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(fr) fr.FormaRespuestaDetalle.Select(Function(d) d.FormaPregunta.Tipo = 6).Count()).Sum()
            usuario.Capacitacion = usuario.CapacitacionVisitas.Count
        Next
        ViewBag.idRuta = ruta.idRuta
        Return View(grupo)
    End Function

    '
    ' GET: /Reportes/RutaUsuario/6

    Function RutaUsuario(Optional ByVal id As Integer = Nothing, Optional ByVal id2 As Integer = Nothing) As ActionResult
        Dim rutaDB As Ruta = db.Ruta.Include("RutaCheckpoint").Include("Grupo").SingleOrDefault(Function(x) x.idRuta = id)
        Dim usuario As Usuarios = (From u In db.Usuarios.Include("Grupo") Where u.idUsuario = id2).SingleOrDefault()
        usuario.GruposCount = usuario.Grupo.Count
        usuario.RutasCount = usuario.Grupo.SelectMany(Function(g) g.Ruta.Select(Function(r) r.idRuta)).Distinct().Count()
        usuario.Checkins = usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = rutaDB.idRuta).Select(Function(c) c.idCheckin).Count()
        usuario.Faltas = rutaDB.RutaCheckpoint.Count - usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = rutaDB.idRuta).Select(Function(c) c.idCheckpoint).Distinct().Count()
        usuario.Cuestionarios = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = rutaDB.idRuta).Select(Function(fr) fr.idFormaRespuesta).Count()
        usuario.Multimedia = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = rutaDB.idRuta).Select(Function(fr) fr.FormaRespuestaDetalle.Select(Function(d) d.FormaPregunta.Tipo = 6).Count()).Sum()
        usuario.Capacitacion = usuario.CapacitacionVisitas.Count
        usuario.FormaRespuesta = (From fr In usuario.FormaRespuesta Where fr.RutaCheckpoint.idRuta = rutaDB.idRuta Select fr).ToArray()
        usuario.Ruta = usuario.Grupo.SelectMany(Function(g) g.Ruta).ToList()
        ViewBag.idRuta = rutaDB.idRuta
        ViewBag.mapApiKey = "AIzaSyB5Mo7NODp6OV-XY949xwgX51iALjnco00"
        ViewBag.esPdf = False
        If Request.QueryString.Get("pdf") IsNot Nothing Then
            ViewBag.esPdf = True
            Return New Rotativa.ViewAsPdf(usuario)
        ElseIf Request.QueryString.Get("excel") IsNot Nothing Then
            Dim tiposFormaOptions As Boolean() = {False, False, False, True, True, True, False}
            Dim csv As String = String.Empty

            csv += "Nombre,Grupos,Rutas" + vbCrLf
            csv += usuario.Nombre + "," + usuario.GruposCount.ToString() + "," + usuario.RutasCount.ToString() + vbCrLf
            csv += "Checkins,Faltas,Cuestionarios,Multimedia,Capacitación" + vbCrLf
            csv += usuario.Checkins.ToString() + "," + usuario.Faltas.ToString() + "," + usuario.Cuestionarios.ToString() + "," + usuario.Multimedia.ToString() + "," + usuario.Capacitacion.ToString() + "," + vbCrLf
            csv += "Rutas" + vbCrLf
            csv += String.Join(",", usuario.Ruta.Select(Function(r) r.Descripcion)) + vbCrLf
            csv += "Grupos" + vbCrLf
            csv += String.Join(",", usuario.Grupo.Select(Function(r) r.Descripcion)) + vbCrLf
            csv += vbCrLf + "Cuestionarios" + vbCrLf
            For Each respuesta As FormaRespuesta In usuario.FormaRespuesta
                csv += respuesta.Forma.Descripcion + "," + respuesta.FechaFin.ToString("dd/MM/hh:mm tt") + vbCrLf
                For Each pregunta As FormaPregunta In respuesta.Forma.FormaPregunta.Where(Function(p) p.Tipo <> 6)
                    Dim respuestaTexto As String = ""
                    If {3, 5}.Contains(pregunta.Tipo) Then
                        Dim detalles = (From d In respuesta.FormaRespuestaDetalle2 Where d.idFormaPregunta = pregunta.idFormaPregunta Select d.FormaPreguntaOpcion.Opcion).ToArray
                        respuestaTexto = String.Join(", ", detalles)
                    Else
                        Dim detalle = (From d In respuesta.FormaRespuestaDetalle Where d.idFormaPregunta = pregunta.idFormaPregunta Select d).FirstOrDefault
                        If detalle Is Nothing Then
                            respuestaTexto = ""
                        Else
                            If tiposFormaOptions(pregunta.Tipo) Then
                                respuestaTexto = ""
                                If detalle.FormaPreguntaOpcion IsNot Nothing Then
                                    respuestaTexto = detalle.FormaPreguntaOpcion.Opcion
                                End If
                            Else
                                respuestaTexto = detalle.Respuesta
                            End If
                        End If
                    End If
                    csv += pregunta.Pregunta + "," + respuestaTexto + vbCrLf
                Next
                csv += vbCrLf
            Next

            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=Reporte.csv")
            Response.Charset = Encoding.UTF8.WebName
            Response.ContentType = "application/text"
            Response.Output.Write(csv)
            Response.Flush()
            Response.End()
        End If
        Return View(usuario)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class