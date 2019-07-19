Public Class Reporte
    Public Property UsuarioSolicitante As String
    Public Property Cliente As String
    Public Property Producto As String
    Public Property Ruta As String
    Public Property FechaInicio As String
    Public Property FechaFinal As String


    Public Property CantidadCheckPoints As Integer
    Public Property CantidadPromotores As Integer
    Public Property CantidadAsistencias As Integer
    Public Property CantidadAusencias As Integer
    Public Property CantidadFormularios As Integer
    Public Property CantidadImagenes As Integer
    'Public Property CheckPoints As List(Of CheckPoint)
    Public Property Promotores As List(Of Usuarios)
    Public Property Cuestionarios As List(Of Cuestionario)
    Public Property Imagenes As List(Of String)

    Public Class CheckPoint

    End Class

    Public Class Cuestionario
        Public Property Nombre As String
        Public Property Fecha As String
        Public Property Pregunta As String
        Public Property Respuesta As String
    End Class
End Class
