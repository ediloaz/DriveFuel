@ModelType MVC4Base.Noticias
@Code
    ViewData("Title") = "Creación de Noticias"
    ViewData("Descripcion") = "Ingresa los datos correspondientes para crear una noticia en el newsfeed del dashboard y app"
End Code

<style>
    .valMessage {
        margin-left: 1em;
        color: red;
    }
</style>

@Using (Html.BeginForm("Edit", "Noticias", FormMethod.Post, New With {.enctype = "multipart/form-data", .id = "EditNoticias"}))

    @Html.AntiForgeryToken()

    @<div class="card bg-white">
        <div class="card-header">
            <button type="button" class="btn btn-success" id="postGuardaNoticia">Actualizar Noticia</button>
        </div>

        <div class="card-block">
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-horizontal">
                        @Html.HiddenFor(Function(m) m.idNoticia)
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.idTipoNoticia, New With {.class = "col-md-2 control-label"})
                            <div class="col-md-10">
                                @Html.DropDownList(name:="idTipoNoticia", selectList:=CType(ViewBag.TiposNoticia, SelectList), htmlAttributes:=New With {.class = "form-control"})
                                <span class="valMessage" id="vmTipoNoticia"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.FechaInicio, New With {.class = "col-md-2 control-label"})
                            <div class="col-md-2">
                                @Html.TextBoxFor(Function(model) model.FechaInicio, Model.FechaInicio.ToString("yyyy-MM-dd"), New With {.type = "date", .class = "form-control"})
                                <span class="valMessage" id="vmFechaInicio"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.FechaFin, New With {.class = "col-md-2 control-label"})
                            <div class="col-md-2">
                                @Html.TextBoxFor(Function(model) model.FechaFin, Model.FechaFin.ToString("yyyy-MM-dd"), New With {.type = "date", .class = "form-control"})
                                <span class="valMessage" id="vmFechaFin"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Titulo, New With {.class = "col-md-2 control-label"})
                            <div class="col-md-2">
                                @Html.TextBoxFor(Function(model) model.Titulo, New With {.type = "text", .class = "form-control", .maxlength = "20"})
                                <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.Titulo)</span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.Mensaje, New With {.class = "col-md-2 control-label"})
                            <div class="col-md-10">
                                @*<textarea class="col-md-10 form-control" rows="3" id="Mensaje" name="Mensaje"></textarea>*@
                                @Html.TextAreaFor(Function(model) model.Mensaje, New With {.class = "col-md-10 form-control", .rows = 3})
                                <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.Mensaje)</span>
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.Label("Grupos de Noticias", New With {.class = "col-md-2 control-label"})
                            <div class="col-md-5">
                                @Html.DropDownList("idGrupos", Nothing, String.Empty, New With {.class = "col-md-8 chosen-select combo", .multiple = Nothing})
                            </div>
                            <span class="valMessage" id="vmGrupos"></span>
                        </div>
                        <div class="form-group">
                            @Html.LabelFor(Function(model) model.ImageFile, New With {.class = "col-md-2 control-label"})
                            <div class="col-md-10">
                                @Html.TextBoxFor(Function(model) model.ImageFile, New With {.type = "file", .accept = ".jpg", .tamanio = "6144", .class = "inputfile"})
                                @If Model.Imagen Is Nothing Then
                                    @<p class="help-block">La imagen es opcional</p>
                                Else
                                    @<p class="help-block">Hay una imagen previamente cargada, si modifica este input la imagen será reemplazada</p>
                                End If
                            </div>
                        </div>
                        <div class="form-group">
                            @Html.Label("Activar", New With {.class = "col-md-2 control-label"})
                            <div class="col-md-10">
                                @Html.CheckBoxFor(Function(model) model.Activa, New With {.hidden = "hidden"})
                                <div class="cs-checkbox m-b">
                                    <input type="checkbox" value="1" id="Activa-v" checked="checked">
                                    <label for="Activa-v"></label>
                                </div>
                            </div>
                        </div>
                        <div id="hiddenDivGruposSeleccionados" class="hidden">
                            @*Espacio para cargar el detalle de los grupos seleccionados para poder hacer post*@
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

End Using

@Section Styles
    @Styles.Render("~/Content/vendor/chosen")
    <style>
        .valMessage {
            margin-left: 1em;
            color: red;
        }
    </style>

End Section

@Section Scripts
    @Scripts.Render("~/bundles/chosen")
    <script>
        $("#postGuardaNoticia").on("click", postForm);

        //Submit de Creación de Retención
        function postForm() {

            if (ListosCamposObligatorios() == false) {
                return 0;
            }

            $("#hiddenDivGruposSeleccionados").html("");
            $.each($("#idGrupos").val(), function (index, value) {
                var html = "<div><input type='hidden' name='gruposSeleccionados[" + index + "]' value='" + value + "'/></div>"
                $("#hiddenDivGruposSeleccionados").append(html);
            });

            $("#EditNoticias").submit();

        }

        //Valida los campos obligatorios que son combo
        function ListosCamposObligatorios() {
            var msgTipoNoticia = "";
            var msgFechaInicio = "";
            var msgFechaFin = "";
            var listosCampos = true;

            if ($("#idTipoNoticia").val() == "") {
                msgTipoNoticia = "Seleccione el tipo de Noticia";
                listosCampos = false;
            }

            if ($("#FechaInicio").val() == "") {
                msgFechaInicio = "Seleccione la fecha de inicio de la noticia";
                listosCampos = false;
            }

            if ($("#FechaFin").val() == "") {
                msgFechaFin = "Seleccione la Fecha de finalización de la noticia";
                listosCampos = false;
            }

            $("#vmTipoNoticia").html(msgTipoNoticia);
            $("#vmFechaInicio").html(msgFechaInicio);
            $("#vmFechaFin").html(msgFechaFin);

            return listosCampos;
        }

        function SeleccionaOpcionesGrupos() {
            var opciones = @Json.Encode(ViewBag.gruposSeleccionados);
            if (opciones != null && opciones != 0){
                $("#idGrupos").val(opciones);
                $('#idGrupos').trigger('chosen:updated');
            }
        }

        $(document).ready(function () {
            $("#Activa").prop('checked', $("#Activa-v").is(':checked'));

            $(".combo").chosen({
                width: "66%",
                allow_single_deselect: true
            });

            SeleccionaOpcionesGrupos();
        });

        $("input:checkbox").change(function () {
            $("#Activa").prop('checked', $("#Activa-v").is(':checked'));
        });
    </script>

End Section