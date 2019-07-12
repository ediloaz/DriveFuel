Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Public Class ApiCheckInController
    Inherits System.Web.Http.ApiController

    Private db As New BaseEntities

    ' GET api/ApiCheckIn
    Function GetCheckIns() As ApiCheckinsUsuarioViewModel
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        Return New ApiCheckinsUsuarioViewModel With {.checkins = usuario.CheckIn.AsEnumerable}
    End Function

    ' GET api/ApiCheckIn/5
    Function GetCheckIn(ByVal id As Integer) As CheckIn
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        Dim checkin As CheckIn = db.CheckIn.Find(id)
        If checkin.idUsuario <> usuario.idUsuario Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.Forbidden))
        End If
        If IsNothing(checkin) Then
            Throw New HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound))
        End If
        Return checkin
    End Function

    ' PUT api/ApiCheckIn/5
    'Function PutCheckIn(ByVal id As Integer, ByVal checkin As CheckIn) As HttpResponseMessage
    '    If Not ModelState.IsValid Then
    '        Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
    '    End If

    '    If Not id = checkin.idCheckin Then
    '        Return Request.CreateResponse(HttpStatusCode.BadRequest)
    '    End If

    '    db.Entry(checkin).State = EntityState.Modified

    '    Try
    '        db.SaveChanges()
    '    Catch ex As DbUpdateConcurrencyException
    '        Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
    '    End Try

    '    Return Request.CreateResponse(HttpStatusCode.OK)
    'End Function

    ' POST api/ApiCheckIn
    Function PostCheckIn(ByVal checkin As CheckIn) As HttpResponseMessage
        Dim correo As String = JWTController.JWTUser(Request)
        Dim usuario = (From u In db.Usuarios Where u.Correo = correo).FirstOrDefault
        checkin.idUsuario = usuario.idUsuario
        If ModelState.IsValid Then
            db.CheckIn.Add(checkin)
            db.SaveChanges()

            Dim response As HttpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, checkin)
            response.Headers.Location = New Uri(Url.Link("DefaultApi", New With {.id = checkin.idCheckin}))
            Return response
        Else
            Return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState)
        End If
    End Function

    ' DELETE api/ApiCheckIn/5
    Function DeleteCheckIn(ByVal id As Integer) As HttpResponseMessage
        Dim checkin As CheckIn = db.CheckIn.Find(id)
        If IsNothing(checkin) Then
            Return Request.CreateResponse(HttpStatusCode.NotFound)
        End If

        db.CheckIn.Remove(checkin)

        Try
            db.SaveChanges()
        Catch ex As DbUpdateConcurrencyException
            Return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex)
        End Try

        Return Request.CreateResponse(HttpStatusCode.OK, checkin)
    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class