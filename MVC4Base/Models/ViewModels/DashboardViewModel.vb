Public Class DashboardViewModel
    Public Property Role As String
    Public Property ActividadReciente As List(Of Actividad)
    Public Property Noticias As Integer
    Public Property Capacitaciones As Integer
    Public Property Imagenes As List(Of String)
    Public Property ChecksRecientes As List(Of Checks)
    Public Property Grupos As List(Of Grupo)
    Public Property Rutas As List(Of Ruta)
    Public Property CheckIns As List(Of CheckIn)
    Public Property Ausencias As List(Of Ausencia)

    Public Class Actividad
        Public Property Actividad As EnumActividad
        Public Property Persona As String
        Public Property Descripcion As String
        Public Property Tiempo As Integer

    End Class
    Public Class Checks
        Public Property fecha As String
        Public Property checks As Integer

    End Class

    Public Class Ausencia
        Public Property fecha As String
        Public Property usuario As String
        Public Property ruta As String

    End Class


End Class
