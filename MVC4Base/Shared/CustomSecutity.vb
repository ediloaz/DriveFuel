Imports WebMatrix.WebData

Public Class CustomSecutity

    Public Shared Function PermisoAccion(ByVal user As System.Security.Principal.IPrincipal, ByVal accion As String) As Boolean
        If Not WebSecurity.IsAuthenticated Then Return False

        Dim rols As String() = RolesAccion(accion)
        For Each rol In rols
            If user.IsInRole(rol) Then
                Return True
            End If
        Next

        Return False
    End Function
    Public Shared Function PermisoAccion(ByVal user As System.Security.Principal.IPrincipal, ByVal acciones As String()) As Boolean
        For Each accion In acciones
            If PermisoAccion(user, accion) Then Return True
        Next

        Return False
    End Function

    Public Shared Function RolesAccion(ByVal accion As String) As String()
        Dim _acciones As webpages_Acciones()
        Dim rols As New List(Of String)

        _acciones = System.Web.HttpContext.Current.Cache.Get("acciones")
        If _acciones Is Nothing Then
            Using db As New BaseEntities
                _acciones = (From r In db.webpages_Acciones.Include("webpages_Roles")).ToArray
                System.Web.HttpContext.Current.Cache.Add("acciones", _acciones,
                                Nothing, Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, Nothing)
            End Using
        End If

        Dim accion1 As webpages_Acciones = (From a In _acciones Where a.Nombre.Equals(accion, StringComparison.OrdinalIgnoreCase)).SingleOrDefault
        If accion1 IsNot Nothing Then
            rols.AddRange((From r In accion1.webpages_Roles Select r.RoleName))
        End If

        Return rols.Distinct.ToArray
        'Return String.Join(",", rols.Distinct)
    End Function

End Class
