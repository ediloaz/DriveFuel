Public Module Enums
    Public Enum EnumTipoRespuesta As Integer
        Exito = 1
        Fracaso = 0
        ErrorComunicacion = -1
        Advertencia = 2
    End Enum

    Public Enum EnumActividad As Integer
        Upload = 1
        CheckIn = 2
        CheckOut = 3
        Capacitacion = 4
    End Enum

    Public Enum EnumTipoArchivo As Integer
        Ilegible = 0
        EsImagen = 1
        EsVideo = 2
        EsArchivo = 3
    End Enum
End Module

