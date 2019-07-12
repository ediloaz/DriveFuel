Imports System.Data.Entity
Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions

<CustomAuthorize({"AdminRoles"})>
Public Class RolesController
    Inherits System.Web.Mvc.Controller

    Private db As New BaseEntities

    '
    ' GET: /Roles/

    Function Index() As ActionResult
        Return View(db.webpages_Roles.ToList())
    End Function

    '
    ' GET: /Roles/Details/5

    Function Details(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim webpages_roles As webpages_Roles = db.webpages_Roles.Find(id)
        If IsNothing(webpages_roles) Then
            Return HttpNotFound()
        End If
        Return View(webpages_roles)
    End Function

    '
    ' GET: /Roles/Create

    Function Create() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Roles/Create

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Create(ByVal webpages_roles As webpages_Roles) As ActionResult
        If ModelState.IsValid Then
            db.webpages_Roles.Add(webpages_roles)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End If

        Return View(webpages_roles)
    End Function

    '
    ' GET: /Roles/Edit/5

    Function Edit(Optional ByVal id As Integer = Nothing) As ActionResult
        Dim webpages_rol As webpages_Roles = (From r In db.webpages_Roles Where r.RoleId = id).SingleOrDefault

        If IsNothing(webpages_rol) Then
            Return HttpNotFound()
        End If

        ViewBag.acciones = (From a In db.webpages_Acciones).ToArray
        ViewBag.roles = (From r In db.webpages_Roles).ToArray

        Return View(webpages_rol)
    End Function

    '
    ' POST: /Roles/Edit/5

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Function Edit(ByVal model As webpages_Roles, ByVal roles As Integer(), ByVal acciones As Integer()) As ActionResult
        Dim webpages_rol As webpages_Roles = (From r In db.webpages_Roles.Include("webpages_RolesSub").Include("webpages_Acciones")
                                              Where r.RoleId = model.RoleId).SingleOrDefault

        If ModelState.IsValid Then
            webpages_rol.RoleName = model.RoleName
            webpages_rol.Description = model.Description

            webpages_rol.webpages_RolesSub.Clear()
            If roles IsNot Nothing Then
                For Each rol In (From r In db.webpages_Roles Where roles.Contains(r.RoleId))
                    webpages_rol.webpages_RolesSub.Add(rol)
                Next
            End If

            webpages_rol.webpages_Acciones.Clear()
            If acciones IsNot Nothing Then
                For Each accion In (From r In db.webpages_Acciones Where acciones.Contains(r.idAccion))
                    webpages_rol.webpages_Acciones.Add(accion)
                Next
            End If

            db.SaveChanges()
            Flash.Instance.Success("Datos guardados")
        End If

        ViewBag.acciones = (From a In db.webpages_Acciones).ToArray
        ViewBag.roles = (From r In db.webpages_Roles).ToArray

        Return View(webpages_rol)
    End Function

    ''
    '' GET: /Roles/Delete/5

    'Function Delete(Optional ByVal id As Integer = Nothing) As ActionResult
    '    Dim webpages_roles As webpages_Roles = db.webpages_Roles.Find(id)
    '    If IsNothing(webpages_roles) Then
    '        Return HttpNotFound()
    '    End If
    '    Return View(webpages_roles)
    'End Function

    ''
    '' POST: /Roles/Delete/5

    '<HttpPost()> _
    '<ActionName("Delete")> _
    '<ValidateAntiForgeryToken()> _
    'Function DeleteConfirmed(ByVal id As Integer) As RedirectToRouteResult
    '    Dim webpages_roles As webpages_Roles = db.webpages_Roles.Find(id)
    '    db.webpages_Roles.Remove(webpages_roles)
    '    db.SaveChanges()
    '    Return RedirectToAction("Index")
    'End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

End Class