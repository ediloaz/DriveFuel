@ModelType MVC4Base.CheckinsSearch

@Code
    ViewData("Title") = "Checks"
End Code

<div class="card bg-white">
    <div class="card-block">
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)
    @<fieldset>
         <div class="form-group col-xs-4 col-sm-4">
             @Html.LabelFor(Function(model) model.usuario, New With {.class = "control-label"})
             @Html.TextBoxFor(Function(model) model.usuario, New With {.class = "form-control", .placeholder="Usuario"})
             @Html.ValidationMessageFor(Function(model) model.usuario)
         </div>
         <div class="form-group col-xs-4 col-sm-3">
             @Html.LabelFor(Function(model) model.fecha_ini, "Fecha desde", New With {.class = "control-label"})
             @Html.TextBoxFor(Function(model) model.fecha_ini, New With {.class = "form-control m-b", .data_provide = "datepicker", .data_date_format="yyyy-mm-dd", .placeholder = "Fecha desde"})
             @Html.ValidationMessageFor(Function(model) model.fecha_ini)
         </div>
         <div class="form-group col-xs-4 col-sm-3">
             @Html.LabelFor(Function(model) model.fecha_fin, "Fecha hasta", New With {.class = "control-label"})
             @Html.TextBoxFor(Function(model) model.fecha_fin, New With {.class = "form-control m-b", .data_provide = "datepicker", .data_date_format = "yyyy-mm-dd", .placeholder = "Fecha hasta"})
             @Html.ValidationMessageFor(Function(model) model.fecha_fin)
         </div>
         <input type="submit" value="Buscar" class="btn btn-dark pull-right" />
</fieldset>
End Using
        <div class="table-responsive">
            <table class="table table-bordered table-striped datatable m-b-0">
                <thead>
                    <tr>
                        <th>Checkpoint</th>
                        <th>Usuario</th>
                        <th>Entrada</th>
                        <th>Salida</th>
                    </tr>
                </thead>
                <tbody>
                    @For Each checkpoint In ViewBag.respuestas
                        Dim checkPointName As String = checkpoint.Descripcion
                        @For Each item In checkpoint.checkins
                            Dim usuario As MVC4Base.Usuarios = item.Usuarios
                            Dim checks As System.Collections.Generic.IEnumerable(Of MVC4Base.CheckIn) = item.Group
                            Dim entrada As MVC4Base.CheckIn = (From c In checks Where c.EsEntrada = True).FirstOrDefault
                            Dim salida As MVC4Base.CheckIn = (From c In checks Where c.EsEntrada = False).FirstOrDefault
                            @<tr>
                                <td>@checkPointName</td>
                                <td>@usuario.Nombre</td>
                                <td>
                                    @If entrada IsNot Nothing Then
                                        @entrada.Fecha
                                    End If
                                </td>
                                <td>
                                    @If salida IsNot Nothing Then
                                        @salida.Fecha
                                    End If
                                </td>
                            </tr>
                        Next
                    Next
                </tbody>
                <tfoot>
                    <tr>
                        <th>Checkpoint</th>
                        <th>Usuario</th>
                        <th>Entrada</th>
                        <th>Salida</th>
                    </tr>
                </tfoot>
            </table>

        </div>
    </div>
</div>

@Section Styles
    @Styles.Render("~/Content/datepicker")
    @Styles.Render("~/Content/datatable")
End Section

@Section Scripts
    @Scripts.Render("~/bundles/datepicker")
    @Scripts.Render("~/bundles/datatable")
    <script>
        $(document).ready(function () {
            $.fn.datepicker.defaults.autoclose = true;
            $('.datatable').dataTable();
        });
    </script>
End Section
