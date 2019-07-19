@ModelType IEnumerable(Of MVC4Base.FormaRespuesta)

@Code
    ViewData("Title") = "Formas"
    ViewData("Descripcion") = "Formas de ruta"
    Dim forma As MVC4Base.Forma = ViewBag.forma
    Dim tiposFormaOptions As Boolean() = {False, False, False, True, True, True, False}
End Code

<style>
    img {
        max-height: 150px;
    }
    tr.wrapper {
        font-weight: bold;
    }
</style>

<div class="card bg-white">
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-striped m-b-0">
                <thead>
                    <tr>
                        <th>Pregunta</th>
                        <th>Respuesta</th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>Pregunta</th>
                        <th>Respuesta</th>
                    </tr>
                </tfoot>
                <tbody>
                    @For Each item In Model
                        Dim currentItem As MVC4Base.FormaRespuesta = item
                        @<tr class="wrapper">
                            <td>
                                @currentItem.idFormaRespuesta - @currentItem.Usuarios.Nombre
                            </td>
                             <td>
                                 @currentItem.FechaInicio - @currentItem.FechaFin
                             </td>
                        </tr>
                        For Each pregunta In forma.FormaPregunta
                            Dim respuesta As String = ""
                            If {3, 5}.Contains(pregunta.Tipo) Then
                                Dim detalles = (From d In currentItem.FormaRespuestaDetalle2 Where d.idFormaPregunta = pregunta.idFormaPregunta Select d.FormaPreguntaOpcion.Opcion).ToArray
                                respuesta = String.Join(", ", detalles)
                            Else
                                Dim detalle = (From d In currentItem.FormaRespuestaDetalle Where d.idFormaPregunta = pregunta.idFormaPregunta Select d).FirstOrDefault
                                If detalle Is Nothing Then
                                    respuesta = ""
                                Else
                                    If tiposFormaOptions(pregunta.Tipo) Then
                                        respuesta = ""
                                        If detalle.FormaPreguntaOpcion IsNot Nothing Then
                                            respuesta = detalle.FormaPreguntaOpcion.Opcion
                                        End If
                                    Else
                                        respuesta = detalle.Respuesta
                                    End If
                                End If
                                If pregunta.Tipo = 6 Then
                                    If respuesta IsNot Nothing AndAlso respuesta.Length > 0 Then
                                        respuesta = ViewBag.contentPath + "/Respuestas/" + respuesta.Split("\").Last
                                    Else
                                        respuesta = ""
                                    End If
                                End If
                            End If
                            @<tr>
                                 <td>
                                     @pregunta.Pregunta
                                 </td>
                                 <td>
                                     @If pregunta.Tipo = 6 Then
                                        @If respuesta.Length > 0 Then
                                            @<img src="@respuesta" />
                                        End If
                                     Else
                                         @respuesta
                                     End If
                                 </td>
                            </tr>
                        Next
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>
