Imports System.ComponentModel.DataAnnotations
Imports Newtonsoft.Json

Public Class CapacitacionTemaMetadata
    <JsonIgnore>
    Public Overridable Property Capacitacion As Capacitacion

End Class

<MetadataType(GetType(CapacitacionTemaMetadata))>
Partial Public Class CapacitacionTema
   

End Class