Imports MvcFlash.Core
Imports MvcFlash.Core.Extensions

Public Class CustomAuthorizeAttribute
    Inherits AuthorizeAttribute

    Private _estricto As Boolean
    Protected _roleKeys As String

    Public Sub New(ByVal roleKeys As String(), Optional ByVal estricto As Boolean = False)
        _estricto = estricto
        _roleKeys = String.Join(",", roleKeys)
        Dim rols As New List(Of String)

        For Each roleKey In roleKeys
            rols.AddRange(CustomSecutity.RolesAccion(roleKey))
        Next

        Roles = String.Join(",", rols.Distinct)
    End Sub

    Public Overrides Sub OnAuthorization(filterContext As AuthorizationContext)
        MyBase.OnAuthorization(filterContext)

        If TypeOf filterContext.Result Is HttpUnauthorizedResult Then
            Dim esControlador As Boolean = False
            Dim actionCustomAttr As CustomAuthorizeAttribute = filterContext.ActionDescriptor.GetCustomAttributes(GetType(CustomAuthorizeAttribute), False).FirstOrDefault
            Dim controllerCustomAttr As CustomAuthorizeAttribute = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(GetType(CustomAuthorizeAttribute), False).FirstOrDefault
            If controllerCustomAttr IsNot Nothing AndAlso Me._roleKeys = controllerCustomAttr._roleKeys Then
                esControlador = True
            End If
            If esControlador Then
                'Si el permiso no es estricto y existe permiso para la acción, probamos a validar con la acción
                If Not Me._estricto And actionCustomAttr IsNot Nothing Then
                    filterContext.Result = Nothing
                    actionCustomAttr.OnAuthorization(filterContext)
                End If
            Else
                'En este punto ya se valido el permiso para el controlador, 
                ' entonces si el permiso no es estricto lo dejamos pasar
                If Not Me._estricto And controllerCustomAttr IsNot Nothing Then
                    filterContext.Result = Nothing
                End If
            End If
        End If

        If TypeOf filterContext.Result Is HttpUnauthorizedResult Then
            If filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated Then
                Flash.Instance.Warning("No tiene permisos para ejecutar esta opción")
                'filterContext.Result = New RedirectResult("/")
            End If
        End If
    End Sub

End Class
