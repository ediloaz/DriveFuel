@ModelType MVC4Base.Usuarios
@imports MVC4Base

@Code
    ViewData("Title") = "Reporte usuario"
    If ViewBag.esPdf Then
        Layout = "~/Views/Shared/_LayoutEmpty.vbhtml"
    End If
    Dim mapApiKey As String = ViewBag.mapApiKey
    Dim tiposFormaOptions As Boolean() = {False, False, False, True, True, True, False}
End Code

<style>
    body{
        @If ViewBag.esPdf Then
            @("width: 1080px;padding: 10px;")
        End If
    }
  .cuadro{
    background: #8ba73a;
    height: 100%;
  }
  .contenedor_datos{
    padding: 10px;
    text-align: center;
  }
  .contenedor_grupos{
    margin-top: 10px;
  }
  .contenedor_usuario{
    margin-top: 4px;
  }
  .contenedor{
    padding:  10px;
  }
  .fila{
    height: 108px;
  }
  .fila-gps{
    height: 180px;
  }
  .fotos {
  	height: auto;
  }
  .fotos > div {
  	margin-top: 10px;
  }
  .fotos > div > img {
  	max-width: 100%;
  }
  /*.fotos > img {
		max-width: 32%;
		margin-top: 10px;
	}
	.fotos > img:nth-child(3n+2) {
		margin-left: 1%;
		margin-right: 1%;
	}*/
  h3{
    margin-top: 0 !important;
  }
  .bg-ficha{
    background-color: #e9e9ea;
    @If ViewBag.esPdf Then
        @("font-size: .1em;")
    End If
  }
</style>

<div class="main-content">
    <div class="card card-block bg-white no-border" style="overflow: hidden;">
        <div class="row fila">
            <div class="col-xs-3 contenedor_usuario">
                <h3>@Model.Nombre</h3>
                <h3>Grupos: @Model.GruposCount</h3>
                <h3>Rutas @Model.RutasCount</h3>
            </div>
            <div class="col-xs-2">
                <div class="contenedor_datos bg-ficha">
                    <h3>Checkins</h3>
                    <h3>@Model.Checkins</h3>
                </div>
            </div>
            <div class="col-xs-1">
                <div class="contenedor_datos bg-ficha">
                    <h3>Faltas</h3>
                    <h3>@Model.Faltas</h3>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="contenedor_datos bg-ficha">
                    <h3>Cuestionarios</h3>
                    <h3>@Model.Cuestionarios</h3>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="contenedor_datos bg-ficha">
                    <h3>Multimedia</h3>
                    <h3>@Model.Multimedia</h3>
                </div>
            </div>
            <div class="col-xs-2">
                <div class="contenedor_datos bg-ficha">
                    <h3>Capacitación</h3>
                    <h3>@Model.Capacitacion</h3>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
          <div class="col-xs-4">
              <div class="card-block bg-white" >
                <h3>Rutas:</h3>
                @For Each ruta In Model.Ruta
                    @<h3>@ruta.Descripcion</h3>
                Next
              </div>
            <div class="card-block bg-white contenedor_grupos" >
              <h3>Grupos</h3>
              @For Each grupo In Model.Grupo
	                @<h3>@grupo.Descripcion</h3>
	            Next
            </div>
              @If Not ViewBag.esPdf Then
            @<div class="contenedor_grupos">
                <div class="col-xs-6">
                    <a href="?excel=1" target="_blank" class="btn btn-dark">Exportar a Excel</a>
                </div>
                <div class="col-xs-6">
                    <a href="?pdf=1" target="_blank" class="btn btn-dark">Exportar a PDF</a>
                </div>
            </div>
              End If
          </div>
          <div class="col-xs-8">
              @For Each respuesta As FormaRespuesta In Model.FormaRespuesta
            @<div class="row contenedor bg-white same-height-cards fila-gps  ">
              <div class="col-xs-5">
                <h4>Cuestionario: @respuesta.Forma.Descripcion</h4>
                <h4>Checkin: @respuesta.FechaFin.ToString("dd/MM/hh:mm tt")</h4>
              </div>
              <div class="col-xs-7 text-white">
                @*<img src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=560x160&markers=color:black%7Clabel:S%7C11211%7C11206%7C11222&key=AIzaSyClLCcDDdycXnXbmx-C2TFx_6_dwU49_40&key2=@(mapApiKey)" />*@
                  <img src="https://maps.googleapis.com/maps/api/staticmap?zoom=13&size=560x160&markers=color:black%7C@(respuesta.RutaCheckpoint.Latitud),@(respuesta.RutaCheckpoint.Longitud)&key=@(mapApiKey)" />
              </div>
            </div>
            @For Each pregunta As FormaPregunta In respuesta.Forma.FormaPregunta.Where(Function(p) p.Tipo <> 6)
                      Dim respuestaTexto As String = ""
                      If {3, 5}.Contains(pregunta.Tipo) Then
                          Dim detalles = (From d In respuesta.FormaRespuestaDetalle2 Where d.idFormaPregunta = pregunta.idFormaPregunta Select d.FormaPreguntaOpcion.Opcion).ToArray
                          respuestaTexto = String.Join(", ", detalles)
                      Else
                          Dim detalle = (From d In respuesta.FormaRespuestaDetalle Where d.idFormaPregunta = pregunta.idFormaPregunta Select d).FirstOrDefault
                          If detalle Is Nothing Then
                              respuestaTexto = ""
                          Else
                              If tiposFormaOptions(pregunta.Tipo) Then
                                  respuestaTexto = ""
                                  If detalle.FormaPreguntaOpcion IsNot Nothing Then
                                      respuestaTexto = detalle.FormaPreguntaOpcion.Opcion
                                  End If
                              Else
                                  respuestaTexto = detalle.Respuesta
                              End If
                          End If
                          If pregunta.Tipo = 6 Then
                              If respuesta IsNot Nothing AndAlso respuestaTexto.Length > 0 Then
                                  respuestaTexto = ViewBag.contentPath + "/Respuestas/" + respuesta
                              Else
                                  respuestaTexto = ""
                              End If
                          End If
                      End If
                @<div class="row contenedor bg-white same-height-cards">
                    <div class="col-xs-5">
                    <h4>@pregunta.Pregunta</h4>
                    </div>
                    <div class="col-xs-7">
                    <div class="row contenedor bg-ficha">
                        <h4>@respuestaTexto</h4>
                    </div>
                    </div>
                </div>          
            Next
            @<div class="row contenedor bg-white same-height-cards">
              <div class="col-xs-5">
                <h4>Multimedia</h4>
              </div>
              <div class="col-xs-7">
                <div class="contenedor bg-ficha fila row fotos">
                @For Each detalle As FormaRespuestaDetalle In respuesta.FormaRespuestaDetalle.Where(Function(d) d.FormaPregunta.Tipo = 6).ToArray
                      Dim respuestaTexto = ""
                      If detalle.Respuesta.Length > 0 Then
                          respuestaTexto = ViewBag.contentPath + "/Respuestas/" + detalle.Respuesta
                      End If
                    @<div class="col-xs-4">
                        <img src="@respuestaTexto" />
                    </div>
                  Next
                </div>
              </div>
            </div>
              Next
          </div>
        </div>
</div>