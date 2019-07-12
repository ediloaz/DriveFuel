Imports System.Net
Imports System.Web.Http
Imports System.Net.Http
Imports System.IO

Public Class ApiCapacitacionController
    Inherits ApiController

    Private db As New BaseEntities

    Function GetCapacitacion() As ApiCapacitacionViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault

        Dim l_capacitaciones As New List(Of Capacitacion)
        For Each _grupo As Grupo In usuario.Grupo.ToList
            Dim _capacitaciones = db.Capacitacion.Where(Function(x) x.Grupo.Any(Function(y) y.idGrupo = _grupo.idGrupo)).ToList
            l_capacitaciones.AddRange(_capacitaciones)
        Next

        Return New ApiCapacitacionViewModel With {.Capacitaciones = l_capacitaciones.AsEnumerable}

    End Function

    Function PostRevisaCapacitacion(ByVal idCapacitacionArchivos As Integer) As HttpResponseMessage

        Dim correo As String = JWTController.JWTUser(Request)
        Dim _idUsuario = (From u In db.Usuarios Where u.Correo = correo Select u.idUsuario).FirstOrDefault

        Dim visita = db.CapacitacionVisitas.Where(Function(x) x.idUsuario = _idUsuario And x.idCapacitacionArchivos = idCapacitacionArchivos).ToList
        If Not visita.Any Then
            Dim _cv As New CapacitacionVisitas

            With _cv
                .FechaVisita = Now
                .idCapacitacionArchivos = idCapacitacionArchivos
                .idUsuario = _idUsuario
            End With
            db.CapacitacionVisitas.Add(_cv)
        Else
            'visita.First().FechaVisita = Now
        End If


        Try
            db.SaveChanges()
        Catch ex As Exception
            Return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex)
        End Try


        Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK)
        Return response

    End Function

End Class
