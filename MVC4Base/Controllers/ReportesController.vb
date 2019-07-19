Imports System.Data.Entity
Imports Pechkin.Util
Imports Pechkin.SimplePechkin
Imports Pechkin.GlobalConfig
Imports Pechkin
Imports Pechkin.Synchronized


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
        If ruta IsNot Nothing Then
            Dim model As New Reporte

            model.UsuarioSolicitante = User.Identity.Name
            model.Cliente = ruta.Cliente.NombreCliente
            model.Producto = ruta.Producto.NombreProducto
            model.Ruta = ruta.Descripcion
            model.CantidadCheckPoints = 0
            model.CantidadPromotores = 0
            model.CantidadAsistencias = 0
            model.CantidadAusencias = 0
            model.CantidadFormularios = 0
            model.CantidadImagenes = 0


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

            End If
            model.Promotores = New List(Of Usuarios)
            For Each grupo As Grupo In ruta.Grupo
                Dim idUsuarios As Integer() = grupo.Usuarios.Select(Function(u) u.idUsuario).ToArray()
                For Each usuario As Usuarios In grupo.Usuarios
                    usuario.Correo = grupo.Descripcion 'Correo=Nombre del grupo
                    usuario.Checkins = usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(c) c.idCheckin).Count()
                    usuario.Faltas = ruta.RutaCheckpoint.Count - usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = ruta.idRuta).Select(Function(c) c.idCheckpoint).Distinct().Count()
                    model.CantidadAsistencias += usuario.Checkins
                    model.CantidadAusencias += usuario.Faltas
                    model.Promotores.Add(usuario)
                Next
                grupo.PromotoresActivos = grupo.Usuarios.Count
                model.CantidadPromotores += grupo.PromotoresActivos ' NUEVO * * * * * * * *  
                Dim usuariosCheckin As Integer = ruta.RutaCheckpoint.SelectMany(Function(rc) rc.CheckIn.Select(Function(c) c.Usuarios.idUsuario)).Distinct().Count()
                If usuariosCheckin > 0 Then
                    grupo.CheckinPorcentaje = Math.Round(usuariosCheckin / grupo.PromotoresActivos * 100)
                Else
                    grupo.CheckinPorcentaje = 0
                End If
                grupo.Cuestionarios = ruta.RutaCheckpoint.Select(Function(rc) rc.FormaRespuesta.Where(Function(fr) idUsuarios.Contains(fr.idUsuario)).Count).Sum()
                grupo.Fotos = ruta.RutaCheckpoint.Select(Function(rc) rc.FormaRespuesta.Where(Function(fr) idUsuarios.Contains(fr.idUsuario)).Select(Function(fr) fr.FormaRespuestaDetalle.Where(Function(d) d.FormaPregunta.Tipo = 6))).Sum(Function(y) y.Count())
                model.CantidadImagenes += grupo.Fotos  'NUEVO * * * * * * * 
                model.FechaInicio = ruta.RutaCheckpoint.OrderBy(Function(rc) rc.Llegada).FirstOrDefault().Llegada.ToLongDateString()
                model.FechaFinal = ruta.RutaCheckpoint.OrderByDescending(Function(rc) rc.Llegada).FirstOrDefault().Salida.ToLongDateString()
            Next


            'CHECKPOINTS
            Dim CheckPoints = (From r In ruta.RutaCheckpoint
                               Select r.Descripcion, r.Lugar, checkins = (From c In r.CheckIn Group By c.Usuarios Into Group).ToArray()).ToArray()
            model.CantidadCheckPoints = CheckPoints.Count()


            'IMAGENES
            model.Imagenes = New List(Of String)
            Dim baseUri = New Uri(Util.ObtenerParametro("OFICIAL_WEB_PAGE"))
            Dim contentPathUri = New Uri(baseUri, Util.ObtenerParametro("CONTENT_VIRTUAL_PATH"))
            Dim contentPath = contentPathUri.AbsoluteUri
            Dim UltimasFotos = db.FormaRespuestaDetalle.Where(Function(x) x.FormaPregunta.Tipo = 6 And x.FormaPregunta.idFormaPregunta = x.idFormaPregunta And x.Respuesta <> "").OrderByDescending(Function(x) x.idFormaRespuesta).Take(6)
            For Each foto In UltimasFotos
                Dim temp = foto.Respuesta.Split("\")
                Dim temp2 = temp.Last()
                model.Imagenes.Add(contentPath + "/Respuestas/" + temp2)
            Next

            'FORMULARIOS
            model.Cuestionarios = New List(Of Reporte.Cuestionario)
            Dim QueryCuestionarios = db.Forma.Include("FormaPregunta").Include("FormaRespuesta")
            For Each row In QueryCuestionarios
                Dim cuestionario As New Reporte.Cuestionario
                cuestionario.Nombre = row.Descripcion
                Try
                    cuestionario.Fecha = row.FormaRespuesta.FirstOrDefault().FechaInicio.ToLongDateString()
                Catch ex As Exception
                    cuestionario.Fecha = "sin respuestas"
                End Try
                model.CantidadFormularios += 1
                model.Cuestionarios.Add(cuestionario)
            Next

            ViewBag.CheckPoints = CheckPoints

            If Request.QueryString.Get("pdf") IsNot Nothing Then
                ViewBag.esPdf = True

                ' ' // create global configuration object
                ' GlobalConfig GC = New GlobalConfig();

                ' ' // set it up using fluent notation
                ' GC.SetMargins(New Margins(300, 100, 150, 100))
                ' .SetDocumentTitle("Test document")
                ' .SetPaperSize(PaperKind.Letter);
                ' ' //... etc

                ' ' // create converter
                ' IPechkin Pechkin = New SynchronizedPechkin(GC);

                ' ' // subscribe to events
                ' Pechkin.Begin += OnBegin();
                ' Pechkin.Error += OnError();
                ' Pechkin.Warning += OnWarning();
                ' Pechkin.PhaseChanged += OnPhase;
                ' Pechkin.ProgressChanged += OnProgress;
                ' Pechkin.Finished += OnFinished();

                ' ' // create document configuration object
                ' ObjectConfig oc = New ObjectConfig();

                ''  // And set it up using fluent notation too
                ' oc.SetCreateExternalLinks(False)
                ' .SetFallbackEncoding(Encoding.ASCII)
                ' .SetLoadImages(False)
                ' .SetPageUri("http://google.com");
                ' ' //... etc

                ' ' // convert document
                ' Byte[] pdfBuf = pechkin.Convert(oc);

                Return New Rotativa.ViewAsPdf(model)
            ElseIf Request.QueryString.Get("excel") IsNot Nothing Then
                Dim tiposFormaOptions As Boolean() = {False, False, False, True, True, True, False}
                Dim contenido As String = String.Empty
                contenido += "══════════════════════" + vbCrLf
                contenido += "REPORTE DE ACTIVIDADES" + vbCrLf
                contenido += "Generado por " + model.UsuarioSolicitante + vbCrLf
                contenido += vbCrLf + vbCrLf
                contenido += "INFORMACIÓN GENERAL" + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "Cliente: ," + model.Cliente + vbCrLf
                contenido += "Producto: ," + model.Producto + vbCrLf
                contenido += "Ruta: ," + model.Ruta + vbCrLf
                contenido += "Período: ," + model.FechaInicio + " - " + model.FechaFinal + vbCrLf
                contenido += "╚════════════════════════════════════════╝" + vbCrLf
                contenido += vbCrLf
                contenido += " DATOS ESTADÍSTICOS " + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "CheckPoints: ," + model.CantidadCheckPoints.ToString() + vbCrLf
                contenido += "Promotores: ," + model.CantidadPromotores.ToString() + vbCrLf
                contenido += "Asistencias: ," + model.CantidadAsistencias.ToString() + vbCrLf
                contenido += "Ausencias: ," + model.CantidadAusencias.ToString() + vbCrLf
                contenido += "Formularios: ," + model.CantidadFormularios.ToString() + vbCrLf
                contenido += "Imágenes: ," + model.CantidadImagenes.ToString() + vbCrLf
                contenido += "╚════════════════════════════════════════╝" + vbCrLf
                contenido += vbCrLf
                contenido += " CHECKPOINTS " + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "# , DIRECCIÓN , CHECKPOINT" + vbCrLf
                Dim cont = 0
                For Each CheckPoint In ViewBag.CheckPoints
                    cont += 1
                    contenido += cont.ToString() + "," + CheckPoint.Lugar + "," + CheckPoint.Descripcion + vbCrLf
                Next
                contenido += "╚════════════════════════════════════════╝" + vbCrLf
                contenido += vbCrLf
                contenido += " PROMOTORES" + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "PROMOTOR , GRUPO , CHECK-IN , FALTAS " + vbCrLf
                For Each usuario In model.Promotores
                    contenido += usuario.Nombre + "," + usuario.Correo + "," + usuario.Checkins.ToString() + "," + usuario.Faltas.ToString() + vbCrLf
                Next
                contenido += "╚════════════════════════════════════════╝" + vbCrLf
                contenido += vbCrLf
                contenido += " ASISTENCIAS" + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "PROMOTOR , GRUPO , CHECK-IN " + vbCrLf
                For Each usuario In model.Promotores
                    If usuario.Checkins <> "0" Then
                        contenido += usuario.Nombre + "," + usuario.Correo + "," + "✓" + vbCrLf
                    End If
                Next
                contenido += "╚════════════════════════════════════════╝" + vbCrLf
                contenido += vbCrLf
                contenido += " AUSENCIAS " + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "PROMOTOR , GRUPO , CHECK-IN " + vbCrLf
                For Each usuario In model.Promotores
                    If usuario.Checkins <> "0" Then
                        contenido += usuario.Nombre + "," + usuario.Correo + "," + "✗" + vbCrLf
                    End If
                Next
                contenido += "╚════════════════════════════════════════╝" + vbCrLf
                contenido += vbCrLf
                contenido += " FORMULARIOS " + vbCrLf
                contenido += "╔════════════════════════════════════════╗" + vbCrLf
                contenido += "NOMBRE , FECHA " + vbCrLf
                For Each cuestionario In model.Cuestionarios
                    contenido += cuestionario.Nombre + "," + cuestionario.Fecha + vbCrLf
                Next
                contenido += "╚════════════════════════════════════════╝" + vbCrLf


                'For Each respuesta As FormaRespuesta In usuario.FormaRespuesta
                '    csv += respuesta.Forma.Descripcion + "," + respuesta.FechaFin.ToString("dd/MM/hh:mm tt") + vbCrLf
                '    For Each pregunta As FormaPregunta In respuesta.Forma.FormaPregunta.Where(Function(p) p.Tipo <> 6)
                '        Dim respuestaTexto As String = ""
                '        If {3, 5}.Contains(pregunta.Tipo) Then
                '            Dim detalles = (From d In respuesta.FormaRespuestaDetalle2 Where d.idFormaPregunta = pregunta.idFormaPregunta Select d.FormaPreguntaOpcion.Opcion).ToArray
                '            respuestaTexto = String.Join(", ", detalles)
                '        Else
                '            Dim detalle = (From d In respuesta.FormaRespuestaDetalle Where d.idFormaPregunta = pregunta.idFormaPregunta Select d).FirstOrDefault
                '            If detalle Is Nothing Then
                '                respuestaTexto = ""
                '            Else
                '                If tiposFormaOptions(pregunta.Tipo) Then
                '                    respuestaTexto = ""
                '                    If detalle.FormaPreguntaOpcion IsNot Nothing Then
                '                        respuestaTexto = detalle.FormaPreguntaOpcion.Opcion
                '                    End If
                '                Else
                '                    respuestaTexto = detalle.Respuesta
                '                End If
                '            End If
                '        End If
                '        csv += pregunta.Pregunta + "," + respuestaTexto + vbCrLf
                '    Next
                contenido += vbCrLf
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=Reporte de " + model.UsuarioSolicitante + ".csv")
                Response.Charset = Encoding.UTF8.WebName
                Response.ContentType = "application/text"
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble())
                Response.Output.Write(contenido)
                Response.Flush()
                Response.End()
            End If

            Return View(model)

        End If













    End Function

    '
    ' GET: /Reportes/RutaGrupo/6

    Function RutaGrupo(Optional ByVal id As Integer = Nothing, Optional ByVal id2 As Integer = Nothing) As ActionResult
        Dim ruta As Ruta = db.Ruta.Include("RutaCheckpoint").Include("Grupo").SingleOrDefault(Function(x) x.idRuta = id)
        Dim grupo As Grupo = (From g In ruta.Grupo Where g.idGrupo = id2).SingleOrDefault()

        For Each usuario As Usuarios In grupo.Usuarios
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