@ModelType MVC4Base.Ruta

@Code
    ViewData("Title") = "Details"
End Code

<style>
  .circulo{
    border: 5px solid #8ba73a;
    border-radius: 50%;
    padding: 35px;
    width: 20px;
    height: 20px;
    background: white;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
  }
  .scores > div { 
  	text-align: center;
  }
  .scores > div > div { 
  	margin-left: auto;
  	margin-right: auto;
  }
  .card-title > h5 { 
  	text-align: center;
  }
  .score{
    font-weight: 900;
    font-size: 20px;
  }
  .bg-ficha{
    background-color: #e9e9ea;
  }
</style>

<div class="card card-block bg-white no-border" style="overflow: hidden;">
    @For Each item In Model.Grupo
        @<div class="col-sm-12 col-md-6">
    <div class="card-block bg-ficha">
      <h4 class="card-title">Zona: @item.Descripcion</h4>
      <h4 class="card-title">Fecha: @item.Llegada.ToLongDateString - @item.Salida.ToLongDateString</h4>
      <div class="row scores" style="margin-top: 25px"><!--******Circulos**********-->
        <div class="col-sm-3">
          <div class="score circulo">@item.CheckinPorcentaje%</div>
        </div>
        <div class="col-sm-3 ">
          <div class="score circulo">@item.Faltantes%</div>
        </div>
        <div class="col-sm-3 ">
          <div class="score circulo">@item.Cuestionarios</div>
        </div>
        <div class="col-sm-3 ">
          <div class="score circulo">@item.PromotoresActivos</div>
        </div>
      </div>
      <div class="row"> <!--******Etiquetas**********-->
        <div class="col-sm-3">
          <div class="card-title" style="padding-left: 8px;"><h5>Check-in</h5></div>
        </div>
        <div class="col-sm-3">
          <div class="card-title" style="padding-left: 8px;"><h5>Faltantes</h5></div>
        </div>
        <div class="col-sm-3">
          <div class="card-title"><h5>Cuestionarios capturados</h5></div>
        </div>
        <div class="col-sm-3">
          <div class="card-title"><h5>Promotores Activos</h5></div>
        </div>
      </div>
      <div class="row" style="margin-top: 25px">
        <div class="col-sm-5" style="padding-top: 10px;">
          <h6 class="card-title">Fotografias capturadas: @item.Fotos</h6>
        </div>
        <div class="col-sm-7 text-right">
            <a href="?excel=1" target="_blank" class="btn btn-dark">Exportar a Excel</a>
            <a href="?pdf=1" target="_blank" class="btn btn-dark">Exportar a PDF</a>
            @Html.ActionLink("Ver Detalle", "RutaGrupo", New With {.id = Model.idRuta, .id2 = item.idGrupo}, New With {.class = "btn btn-success"})
        </div>
      </div>
    </div>
  </div>
    Next
</div>