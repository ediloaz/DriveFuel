@ModelType IEnumerable(Of MVC4Base.InactividadViewModel)

@Code
    ViewData("Title") = "Inactividad de " & Model.FirstOrDefault.Usuario
    ViewData("Descripcion") = "Detalle sobre la actividad del usuario sobre las capacitaciones"
End Code

<style>
    .tituloCapacitacion {
        background: #7c8478;
        color: #ffffff;
        margin-bottom: 1em;
        text-align: center;
        font-size: 16px;
        font-weight: 700;
    }
</style>
<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Inactividad", "Inactividad", "Capacitacion", New With {.class = "btn btn-info"})
    </div>
    <div class="card-block">
        @Code
            Dim l_idCapacitaciones = Model.Select(Function(x) x.idCapacitacion).Distinct().ToList()
            Dim l_idTemas = Model.Select(Function(x) x.idTema).Distinct().ToList()
        End Code

        @For Each idCapacitacion In l_idCapacitaciones
            Dim capacitacion = Model.Where(Function(x) x.idCapacitacion = idCapacitacion).Select(Function(x) x.Capacitacion).FirstOrDefault()
            @<div class="row">
                 <div class="col-md-12 tituloCapacitacion">
                     <span>@capacitacion</span>
                 </div>
                @For Each idTema In l_idTemas
                Dim tema = Model.Where(Function(x) x.idTema = idTema).Select(Function(x) x.Tema).FirstOrDefault()
                Dim l_capitulos = Model.Where(Function(x) x.idCapacitacion = idCapacitacion And x.idTema = idTema).ToList()
                If l_capitulos.Any Then
                    @<div class="col-md-4">
                        <div class="card bg-white">
                            <div class="card-header bg-success text-white">
                                <div class="pull-left">@tema</div>
                            </div>
                            <div class="card-block">
                                @For Each capitulo In l_capitulos
                                        Dim currentItem = capitulo
                                    @<div class="row">
                                        <div class="col-md-9">
                                            <span>@currentItem.Archivo</span>
                                        </div>
                                        @If currentItem.Leida Then
                                            @<div>
                                                <span class="label label-success">Completado</span>
                                            </div>
                                        Else
                                            @<div>
                                                <span class="label label-danger">Incompleto</span>
                                            </div>
                                        End If
                                    </div>
                                    Next

                            </div>
                        </div>
                    </div>
                End If

            Next
            </div>
        Next

    </div>
</div>

@Section Styles
    @Styles.Render("~/Content/datatable")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/datatable")
    
End Section



