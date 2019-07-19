@ModelType MVC4Base.Noticias
@Code
    ViewData("Title") = "Creación de Noticias"
    ViewData("Descripcion") = "¿Está seguro que desea eliminar la noticia?"
End Code

<style>
    .valMessage {
        margin-left: 1em;
        color: red;
    }
</style>

@Using (Html.BeginForm("Delete", "Noticias", FormMethod.Post, New With {.enctype = "multipart/form-data"}))

    @Html.AntiForgeryToken()

    @<div class="card bg-white">
        <div class="card-header">
            <input value="Borrar Noticia" type="submit" class="btn btn-danger"/>
            <a href="@Url.Action("Index","Noticias")" class="btn btn-default">Cancelar</a>
        </div>

        <div class="card-block">
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-horizontal">
                       @Html.HiddenFor(Function(m) m.idNoticia)
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.idTipoNoticia, New With {.class = "col-md-2 control-label bold"})
                            <div class="col-md-10" style="padding-top:7px">
                                @Html.DisplayFor(Function(m) m.idTipoNoticia)
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.FechaInicio, New With {.class = "col-md-2 control-label bold"})
                            <div class="col-md-2" style="padding-top:7px">
                                @Html.DisplayFor(Function(model) model.FechaInicio, Model.FechaInicio.ToString("yyyy-MM-dd"), New With {.type = "date", .class = "form-control"})
                                <span class="valMessage" id="vmFechaInicio"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.FechaFin, New With {.class = "col-md-2 control-label bold"})
                            <div class="col-md-2" style="padding-top:7px">
                                @Html.DisplayFor(Function(model) model.FechaFin, Model.FechaFin.ToString("yyyy-MM-dd"), New With {.type = "date", .class = "form-control"})
                                <span class="valMessage" id="vmFechaFin"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Titulo, New With {.class = "col-md-2 control-label bold"})
                            <div class="col-md-2" style="padding-top:7px">
                                @Html.DisplayFor(Function(model) model.Titulo, New With {.type = "text", .class = "form-control", .maxlength = "20"})
                                <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.Titulo)</span>
                            </div>                    
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Mensaje, New With {.class = "col-md-2 control-label bold"})
                            <div class="col-md-10" style="padding-top:7px">
                                @*<textarea class="col-md-10 form-control" rows="3" id="Mensaje" name="Mensaje"></textarea>*@
                                @Html.DisplayFor(Function(model) model.Mensaje, New With {.class = "col-md-10 form-control", .rows = 3})
                                <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.Mensaje)</span>
                            </div>
                        
                        </div>
                        <div class="form-group">
                            @Html.Label("Activar", New With {.class = "col-md-2 control-label bold"})
                            <div class="col-md-10" style="padding-top:7px">
                                @Html.CheckBoxFor(Function(model) model.Activa, New With {.hidden = "hidden"})
                                <div class="cs-checkbox m-b">
                                    <input type="checkbox" value="1" id="Activa-v" checked="checked" disabled="disabled">
                                    <label for="Activa-v"></label>
                                </div>
                            </div>        
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>    
    
End Using


@Section Scripts
  <script>

    $(document).ready(function () {
        $("#Activa-v").prop('checked', $("#Activa").is(':checked'));
    });

    $("#Activa-v").change(function () {
        $("#Activa").prop('checked', $("#Activa-v").is(':checked'));
    });
</script>

End Section