Public Class Reporte
    Public Property Role As String

    Public Property Noticias As Integer
    Public Property Capacitaciones As Integer
    Public Property Cuestionarios As List(Of Cuestionario)

    Public Class Cuestionario
        Public Property Nombre As String
        Public Property Fecha As String
        Public Property Pregunta As String
        Public Property Respuesta As String
    End Class
End Class
