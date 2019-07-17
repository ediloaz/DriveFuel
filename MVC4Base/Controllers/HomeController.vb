Imports System.Data.Objects
Imports System.IO
Imports WebMatrix.WebData


<Authorize()>
Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    Function GetNotificacionMasiva(ByVal model As DashboardViewModel) As DashboardViewModel
        Dim l_grupos As New List(Of Grupo)
        Dim result
        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "SuperAdmin") Then
            l_grupos = db.Grupo.ToList
            model.Grupos = l_grupos
        ElseIf Roles.IsUserInRole(WebSecurity.CurrentUserName, "Cliente") Then
            l_grupos = db.Grupo.Include("Usuarios").ToList
            result = l_grupos.Where(Function(x) x.Usuarios.Where(Function(y) y.Correo.Contains(User.Identity.Name)).Count > 0).ToList
            model.Grupos = result
            'l_grupos = db.Grupo.Include("Usuarios").Select(Function(x) x.Usuarios.Where(Function(y) User.Identity.Name = y.Nombre))
        Else
            Dim Ids_ClientesPermitidos = UsuariosAccesoController.ObtenerClientesPermitidos(WebSecurity.CurrentUserId)
            Dim Ids_ProductosPermitidos As New List(Of Integer)
            For Each idCliente In Ids_ClientesPermitidos
                Ids_ProductosPermitidos.AddRange(UsuariosAccesoController.ObtenerProductosPermitidos(WebSecurity.CurrentUserId, idCliente))
            Next
            l_grupos = db.Grupo.Where(Function(x) Ids_ProductosPermitidos.Contains(x.idProducto)).ToList
            model.Grupos = l_grupos
        End If

        Return model
    End Function

    Function GetActividadReciente(ByVal model As DashboardViewModel) As DashboardViewModel
        Dim actividad As DashboardViewModel.Actividad
        Dim fechaMin = Now.AddDays(-15)

        'Obtiene el data para armar la grpafica de Dashboard
        Dim checks = From c In db.CheckIn
                     Where EntityFunctions.TruncateTime(c.Fecha) >= EntityFunctions.TruncateTime(fechaMin) And c.EsEntrada = True
                     Select New DashboardViewModel.Checks With {.fecha = EntityFunctions.TruncateTime(c.Fecha), .checks = c.idCheckin}
        Dim checksPorDia = From c In checks
                           Group By c.fecha Into TotalChecks = Count(c.checks)
                           Select New DashboardViewModel.Checks With {.fecha = fecha, .checks = TotalChecks}

        model.ChecksRecientes = New List(Of DashboardViewModel.Checks)
        'Recorre todos los días para ver llenar el arreglo en días
        For dias As Integer = 0 To DateDiff(DateInterval.Day, fechaMin, Now)
            Dim fecha = fechaMin.AddDays(dias)
            Dim checksFecha = checksPorDia.Where(Function(x) x.fecha = EntityFunctions.TruncateTime(fecha)).Select(Function(x) x.checks).FirstOrDefault()
            model.ChecksRecientes.Add(New DashboardViewModel.Checks With {.fecha = fecha.Date.ToString("dd/MM/yyyy"), .checks = checksFecha})
        Next

        'Obtiene info de los últimos movimientos para sección de movimientos
        Dim checksRecientes = db.CheckIn.OrderByDescending(Function(x) x.idCheckin).Take(5).ToList
        Dim respuetasFormasRecientes = db.FormaRespuesta.OrderByDescending(Function(x) x.idFormaRespuesta).Take(5).ToList
        Dim visitasCapacitacionRecientes = db.CapacitacionVisitas.OrderByDescending(Function(x) x.idCapacitacionVisitas).Take(5).ToList

        Dim ar As New List(Of DashboardViewModel.Actividad)
        For Each check In checksRecientes
            actividad = New DashboardViewModel.Actividad
            actividad.Persona = check.Usuarios.Nombre
            actividad.Actividad = EnumActividad.CheckIn
            actividad.Descripcion = String.Format("Hizo Checkin en {0}", check.RutaCheckpoint.Lugar)
            actividad.Tiempo = DateDiff(DateInterval.Hour, check.Fecha, Now)
            ar.Add(actividad)
        Next
        For Each respuesta In respuetasFormasRecientes
            actividad = New DashboardViewModel.Actividad
            actividad.Persona = respuesta.Usuarios.Nombre
            actividad.Actividad = EnumActividad.Upload
            actividad.Descripcion = String.Format("Cargó información en la forma {0}", respuesta.Forma.Descripcion)
            actividad.Tiempo = DateDiff(DateInterval.Hour, respuesta.FechaInicio, Now)
            ar.Add(actividad)
        Next
        For Each visita In visitasCapacitacionRecientes
            actividad = New DashboardViewModel.Actividad
            actividad.Persona = visita.Usuarios.Nombre
            actividad.Actividad = EnumActividad.Capacitacion
            actividad.Descripcion = String.Format("Visitó la capacitación {0}", visita.CapacitacionArchivos.CapacitacionTema.Capacitacion.NombreCapacitacion)
            actividad.Tiempo = DateDiff(DateInterval.Hour, visita.FechaVisita, Now)
            ar.Add(actividad)
        Next
        model.ActividadReciente = ar.OrderBy(Function(x) x.Tiempo).Take(7).ToList

        Return model
    End Function

    Function GetRutas(ByVal model As DashboardViewModel) As DashboardViewModel
        model.Rutas = (From r In db.Ruta.Include("RutaCheckpoint") Order By r.idRuta Descending).Take(7).ToList()
        Return model
    End Function

    Function GetAusencias(ByVal model As DashboardViewModel) As DashboardViewModel
        Dim ausencias As New List(Of DashboardViewModel.Ausencia)
        For Each usuario As Usuarios In (From u In db.Usuarios.Include("CheckIn") Select u).ToArray()
            Dim ausencia = New DashboardViewModel.Ausencia

            If usuario.CheckIn.Where(Function(x) x.EsEntrada = False).Count() > usuario.CheckIn.Where(Function(x) x.EsEntrada = True).Count() And usuario.CheckIn.Where(Function(x) x.EsEntrada = False).Count() > 0 Then
                ausencia.usuario = usuario.Nombre
                ausencia.ruta = usuario.CheckIn.Where(Function(x) x.EsEntrada = False).First().RutaCheckpoint.Ruta.Descripcion
                ausencia.fecha = usuario.CheckIn.Where(Function(x) x.EsEntrada = False).FirstOrDefault().Fecha
                ausencias.Add(ausencia)
            End If

        Next
        model.Ausencias = ausencias
        Return model
    End Function

    Function GetAsistencias(ByVal model As DashboardViewModel) As DashboardViewModel
        model.CheckIns = (From c In db.CheckIn.Include("Usuarios") Order By c.idCheckin Descending).Take(7).ToList()
        Return model


        Dim usuarios As New List(Of Usuarios)

        'For Each usuario As Usuarios In (From u In db.Usuarios.Include("Grupo").Include("FormaRespuesta") Select u).ToArray()
        '    usuario.GruposCount = usuario.Grupo.Count
        '    usuario.RutasCount = usuario.Grupo.SelectMany(Function(g) g.Ruta.Select(Function(r) r.idRuta)).Distinct().Count()
        '    usuario.Checkins = usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = Ruta.idRuta).Select(Function(c) c.idCheckin).Count()
        '    usuario.Faltas = Rutas.RutaCheckpoint.Count - usuario.CheckIn.Where(Function(c) c.RutaCheckpoint.idRuta = Ruta.idRuta).Select(Function(c) c.idCheckpoint).Distinct().Count()
        '    usuario.Cuestionarios = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = Ruta.idRuta).Select(Function(fr) fr.idFormaRespuesta).Count()
        '    usuario.Multimedia = usuario.FormaRespuesta.Where(Function(fr) fr.RutaCheckpoint.idRuta = Ruta.idRuta).Select(Function(fr) fr.FormaRespuestaDetalle.Select(Function(d) d.FormaPregunta.Tipo = 6).Count()).Sum()
        '    usuario.Capacitacion = usuario.CapacitacionVisitas.Count
        '    usuario.FormaRespuesta = (From fr In usuario.FormaRespuesta Where fr.RutaCheckpoint.idRuta = Ruta.idRuta Select fr).ToArray()
        '    usuario.Ruta = usuario.Grupo.SelectMany(Function(g) g.Ruta).ToList()
        '    usuarios.Add(usuario)
        'Next

        Return model
    End Function

    Function Index() As ActionResult

        Dim model As New DashboardViewModel

        'ActividadReciente
        model = GetActividadReciente(model)

        ' Actualmente solo hay dos dashboard el del Cliente y el General.
        If Roles.IsUserInRole(WebSecurity.CurrentUserName, "Cliente") Then
            model.Role = "Cliente"

            'Rutas
            model = GetRutas(model)

            'Asistencias
            model = GetAsistencias(model)

            'Ausencias
            model = GetAusencias(model)

        Else
            model.Role = "Admin"

            'Noticias y Capacitaciones
            model.Noticias = db.Noticias.Where(Function(x) x.Activa = True).Count()
            model.Capacitaciones = db.Capacitacion.Where(Function(x) x.Activo = True).Count

            'Fotografias
            Dim fotografiasRecientes = db.FormaRespuestaDetalle.Where(Function(x) x.FormaPregunta.Tipo = 6).Take(6).ToList
            model.Imagenes = New List(Of String)
            For Each foto In fotografiasRecientes
                Dim pathBase = Path.Combine(Util.ObtenerParametro("CONTENT_PATH"), "Respuestas") + "\"
                Dim newBase = String.Format("{0}/{1}/", Util.ObtenerParametro("OFICIAL_WEB_PAGE"), "Contenido/Respuestas")
                Dim pathNew = foto.Respuesta.Replace(pathBase, newBase)
                model.Imagenes.Add(pathNew)
            Next


        End If

        'Notificación masiva
        model = GetNotificacionMasiva(model)











        Return View(model)
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your app description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
End Class
