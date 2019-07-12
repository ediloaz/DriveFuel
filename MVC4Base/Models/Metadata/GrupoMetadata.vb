Partial Public Class Grupo
    Public Property CheckinPorcentaje As Integer
    Public Property Cuestionarios As Integer
    Public Property PromotoresActivos As Integer
    Public Property Fotos As Integer
    Public Property Llegada() As Date
    Public Property Salida() As Date
    Public ReadOnly Property Faltantes() As Integer
        Get
            Return 100 - CheckinPorcentaje
        End Get
    End Property
End Class