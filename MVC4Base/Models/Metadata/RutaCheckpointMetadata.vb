Partial Public Class RutaCheckpoint
    Public Property CheckinPorcentaje As Integer
    Public Property Cuestionarios As Integer
    Public Property PromotoresActivos As Integer
    Public Property Fotos As Integer
    Public ReadOnly Property Faltantes() As Integer
        Get
            Return 100 - CheckinPorcentaje
        End Get
    End Property
End Class