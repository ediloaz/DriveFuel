Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class RouteConfig
    Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        routes.MapRoute( _
            name:="Default2", _
            url:="{controller}/{action}/{id}/{id2}", _
            defaults:=New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional, .id2 = UrlParameter.Optional} _
        )
        routes.MapRoute( _
            name:="Default", _
            url:="{controller}/{action}/{id}", _
            defaults:=New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional} _
            )
    End Sub
End Class