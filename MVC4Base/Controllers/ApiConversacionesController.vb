Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports WebMatrix.WebData

Public Class ApiConversacionesController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiConversaciones
    Function GetConversacions() As ApiConversacionesViewModel
        Dim _idUsuario = WebSecurity.CurrentUserId
        If _idUsuario < 0 Then
            Dim correo As String = JWTController.JWTUser(Request)
            _idUsuario = (From u In db.Usuarios Where u.Correo = correo Select u.idUsuario).FirstOrDefault
        End If
        'Include(Function(c) c.Mensaje.Last()).
        '.Mensaje = c.Mensaje.Last(),
        Dim conversacion As List(Of ApiConversacionViewModel) = db.Conversacion.Include(Function(c) c.Usuarios1).
            Where(Function(c) c.idUsuario = _idUsuario).OrderByDescending(Function(c) c.UltimoFecha).
            Select(Function(c) New ApiConversacionViewModel With {
                       .idConversacion = c.idConversacion,
                       .Nombre = c.Usuarios1.Nombre,
                       .idUsuario = c.idUsuario,
                       .idUsuarioInvitado = If(c.Grupal, 0, c.idUsuarioInvitado),
                       .FechaCreacion = c.FechaCreacion,
                       .Grupal = c.Grupal,
                       .UltimoMensaje = c.UltimoMensaje,
                       .UltimoFecha = c.UltimoFecha
                   }).ToList()
        conversacion.AddRange(db.Conversacion.Include(Function(c) c.Usuarios1).
                                 Where(Function(c) c.idUsuarioInvitado = _idUsuario).OrderByDescending(Function(c) c.UltimoFecha).
                                 Select(Function(c) New ApiConversacionViewModel With {
                                           .idConversacion = c.idConversacion,
                                           .Nombre = c.Usuarios.Nombre,
                                           .idUsuario = c.idUsuarioInvitado,
                                           .idUsuarioInvitado = c.idUsuario,
                                           .FechaCreacion = c.FechaCreacion,
                                           .Grupal = c.Grupal,
                                           .UltimoMensaje = c.UltimoMensaje,
                                           .UltimoFecha = c.UltimoFecha
                                           }).ToList())
        conversacion.AddRange(db.Conversacion.Include(Function(c) c.Usuarios1).
                                 Where(Function(c) c.Usuarios2.Any(Function(u) u.idUsuario = _idUsuario) And c.idUsuario <> _idUsuario).OrderByDescending(Function(c) c.UltimoFecha).
                                 Select(Function(c) New ApiConversacionViewModel With {
                                           .idConversacion = c.idConversacion,
                                           .Nombre = c.Nombre,
                                           .idUsuario = _idUsuario,
                                           .idUsuarioInvitado = 0,
                                           .FechaCreacion = c.FechaCreacion,
                                           .Grupal = c.Grupal,
                                           .UltimoMensaje = c.UltimoMensaje,
                                           .UltimoFecha = c.UltimoFecha
                                           }).ToList())
        Return New ApiConversacionesViewModel With {.conversations = conversacion.ToArray()}
    End Function

    ' GET api/ApiConversaciones/5
    Function GetConversacion(ByVal id As Integer) As Conversacion
        Dim _idUsuario = WebSecurity.CurrentUserId
        If _idUsuario < 0 Then
            Dim correo As String = JWTController.JWTUser(Request)
            _idUsuario = (From u In db.Usuarios Where u.Correo = correo Select u.idUsuario).FirstOrDefault
        End If
        Dim conversacion As Conversacion = db.Conversacion.Find(id)
        If IsNothing(conversacion) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        If conversacion.idUsuario <> _idUsuario AndAlso conversacion.idUsuarioInvitado <> _idUsuario AndAlso Not conversacion.Usuarios2.Any(Function(u) u.idUsuario = _idUsuario) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return conversacion
    End Function

    ' PUT api/ApiConversaciones/5
    Function PutConversacion(ByVal id As Integer, ByVal conversacion As Conversacion) As HttpResponseMessage
        If Not ModelState.IsValid Then
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If

        If Not id = conversacion.idConversacion Then
            Return Request.CreateResponse(HttpStatusCode.BadRequest)
        End If

        db.Entry(conversacion).State = EntityState.Modified

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK)
    End Function

    ' POST api/ApiConversaciones
    Function PostConversacion(ByVal conversacion As ApiConversacionViewModel) As HttpResponseMessage
        Dim _idUsuario = WebSecurity.CurrentUserId
        If _idUsuario < 0 Then
            Dim correo As String = JWTController.JWTUser(Request)
            _idUsuario = (From u In db.Usuarios Where u.Correo = correo Select u.idUsuario).FirstOrDefault
        End If
        If ModelState.IsValid Then
            Dim conversacionDb = New Conversacion
            conversacionDb.idUsuario = _idUsuario
            conversacionDb.FechaCreacion = Now
            conversacionDb.UltimoFecha = conversacionDb.FechaCreacion
            conversacionDb.UltimoMensaje = ""
            conversacionDb.Grupal = conversacion.idUsuarioInvitado Is Nothing
            If conversacionDb.Grupal Then
                conversacionDb.Nombre = conversacion.Nombre
                Dim usuariosSelected = (From u In db.Usuarios Where conversacion.Usuarios.Contains(u.idUsuario)).ToList()
                Dim grupos = (From g In db.Grupo Where conversacion.Grupos.Contains(g.idGrupo) Select g.Usuarios).ToArray()
                For Each grupo In grupos
                    usuariosSelected.AddRange(grupo)
                Next
                usuariosSelected = usuariosSelected.Distinct().ToList()
                conversacionDb.Usuarios2 = usuariosSelected
            Else
                conversacionDb.idUsuarioInvitado = conversacion.idUsuarioInvitado
            End If
            db.Conversacion.Add(conversacionDb)
            db.SaveChanges()

            If Not conversacionDb.Grupal Then
                Dim usuarioInvitado As Usuarios = (From u In db.Usuarios Where u.idUsuario = conversacionDb.idUsuarioInvitado).FirstOrDefault()
                conversacionDb.Nombre = usuarioInvitado.Nombre
            End If

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, conversacionDb)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = conversacion.idConversacion}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiConversaciones/5
    Function DeleteConversacion(ByVal id As Integer) As HttpResponseMessage
        Dim conversacion As Conversacion = db.Conversacion.Find(id)
        If IsNothing(conversacion) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.Conversacion.Remove(conversacion)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, conversacion)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class