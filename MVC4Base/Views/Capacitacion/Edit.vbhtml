@ModelType MVC4Base.Capacitacion

@Code
    ViewData("Title") = "Creación de Capacitaciones"
    ViewData("Descripcion") = "Ingresa los datos y configura la capacitación"
End Code

@Using (Html.BeginForm("Edit", "Capacitacion", FormMethod.Post, New With {.enctype = "multipart/form-data", .id = "CreateCapacitacion"}))
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)
    
    @<div class="card">
        <div class="card-header">
            @Html.ActionLink("Regresar", "Index", Nothing, New With {.class = "btn btn-info"})
            <button type="button" class="btn btn-success" id="postGuardaCapacitacion" style="float:right">Guardar Noticia</button>
        </div>
        <div class="card-block p-a-0">
            <div class="box-tab m-b-0" id="rootwizard">
                <ul class="wizard-tabs">
                    <li class="active"><a href="#tab1" data-toggle="tab" aria-expanded="true">Datos Generales</a></li>
                    <li><a href="#tab2" data-toggle="tab">Configuración</a></li>
                </ul>
                <div class="tab-content">
                    @*Información de cabecera para la capacitacion*@
                    <div id="tab1" class="tab-pane active">
                        <div class="card bg-white">
                            <div class="card-block">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-horizontal">
                                            @Html.HiddenFor(Function(x) x.idCapacitacion)
                                            <div class="form-group">
                                                @Html.LabelFor(Function(model) model.NombreCapacitacion, New With {.class = "col-md-2 control-label"})
                                                <div class="col-md-10">
                                                    @Html.TextBoxFor(Function(model) model.NombreCapacitacion, New With {.type = "text", .class = "form-control", .maxlength = "50"})
                                                    <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.NombreCapacitacion)</span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                @Html.LabelFor(Function(model) model.Descripcion, New With {.class = "col-md-2 control-label"})
                                                <div class="col-md-10">
                                                    @*<textarea class="col-md-10 form-control" rows="3" id="Mensaje" name="Mensaje"></textarea>*@
                                                    @Html.TextAreaFor(Function(model) model.Descripcion, New With {.class = "col-md-10 form-control", .rows = 3})
                                                    <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.Descripcion)</span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                @Html.LabelFor(Function(model) model.FechaInicio, New With {.class = "col-md-2 control-label"})
                                                <div class="col-md-2">
                                                    @Html.TextBoxFor(Function(model) model.FechaInicio, "{0:yyyy-MM-dd}", New With {.type = "date", .class = "form-control"})
                                                </div>
                                                <span class="valMessage" id="vmFechaInicio"></span>
                                            </div>
                                            <div class="form-group">
                                                @Html.LabelFor(Function(model) model.FechaFin, New With {.class = "col-md-2 control-label"})
                                                <div class="col-md-2">
                                                    @Html.TextBoxFor(Function(model) model.FechaFin, "{0:yyyy-MM-dd}", New With {.type = "date", .class = "form-control"})
                                                </div>
                                                <span class="valMessage" id="vmFechaFin"></span>
                                            </div>
                                            <div class="form-group">
                                                @Html.Label("Activar", New With {.class = "col-md-2 control-label"})
                                                <div class="col-md-10">
                                                    @Html.CheckBoxFor(Function(model) model.Activo, New With {.hidden = "hidden"})
                                                    <div class="cs-checkbox m-b">
                                                        <input type="checkbox" value="1" id="Activa-v" checked="checked">
                                                        <label for="Activa-v"></label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                @Html.Label("Grupos de Capacitacion", New With {.class = "col-md-2 control-label"})
                                                <div class="col-md-5">
                                                    @Html.DropDownList("idGrupos", Nothing, String.Empty, New With {.class = "col-md-8 chosen-select combo", .multiple = Nothing})
                                                </div>
                                                <span class="valMessage" id="vmGrupos"></span>
                                            </div>
                                            <div id="hiddenDivGruposSeleccionados" class="hidden">
                                                @*Espacio para cargar el detalle de los grupos seleccionados para poder hacer post*@
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    @*información de configuración de secciones*@
                    <div id="tab2" class="tab2">
                        <div class="m-b-md">
                            <a class="btn btn-success btn-sm btn-icon" id="nuevoTema"><i class="icon-plus"></i><span>Agregar Tema</span></a>
                        </div>
                        <div class="tree well">
                            <ul id="treelist">
                                @*Espacio donde se renderea los templates de la vista de arbol*@
                            </ul>
                        </div>
                        <div id="hiddenDivContenidoCapacitacion" class="hidden">
                            @*Espacio para cargar el detalle de la configuración de la capacitación*@
                        </div>
                    </div>
                    <div class="wizard-pager">
                    </div>
                </div>
            </div>
        </div>
    </div>
End Using

@Html.Partial("templateConfiguraCapacitacion")

@Section Styles
    @Styles.Render("~/Content/vendor/chosen")
    @Styles.Render("~/Content/treeView")
    <style>
        .valMessage {
            margin-left: 1em;
            color: red;
        }
        .descripcionContenido{
            width: 500px;
        }
        .treeFileValidation {
            border: 1px solid red;
            margin: 3px;
        }
    </style>

End Section

@Section Scripts
    @Scripts.Render("~/bundles/underscore")
    @Scripts.Render("~/bundles/chosen")
    @Scripts.Render("~/bundles/treeView")
    <script>
        $("#postGuardaCapacitacion").on("click", postForm);

        //Submit de Creación de Retención
        function postForm() {
            if (ListosCamposObligatorios() == false) {
                return 0;
            }

            if (ListosCamposObligatoriosArbol() == false) {
                return 0;
            }

            $("#hiddenDivGruposSeleccionados").html("");
            $.each($("#idGrupos").val(), function (index,value) {
                var html = "<div><input type='hidden' name='gruposSeleccionados["+ index +"]' value='"+ value +"'/></div>"
                $("#hiddenDivGruposSeleccionados").append(html);
            });

            completaPostConfiguracionCapacitacion();

            $("#CreateCapacitacion").submit();
        }

        function SeleccionaOpcionesGrupos() {
            var opciones = @Json.Encode(ViewBag.gruposSeleccionados);
            if (opciones != null && opciones != 0){
                $("#idGrupos").val(opciones);
                $('#idGrupos').trigger('chosen:updated');
            }
        }

        function LlenaArboCapacitacion(temas){
            var temaAnterior;
            var archivoAnterior;
            var idTema = -1;
            var idArchivo = -1;
            $.each(temas,function(index,value){
                if (temaAnterior != value.TituloTema){
                    idTema ++;
                    insertaTema(idTema);
                    $("#Tema-"+idTema).val(value.TituloTema);
                }
                if (archivoAnterior != value.TituloArchivo){
                    idArchivo ++;
                    insertaArchivo(idTema,idArchivo);
                    $("#idTemaCapacitacion-"+idArchivo).val(value.idTemaCapacitacion);
                    $("#idCapacitacionArchivos-"+idArchivo).val(value.idCapacitacionArchivos);
                    $("#TituloArchivo-"+idArchivo).val(value.TituloArchivo);
                    $("#DescripcionArchivo-"+idArchivo).val(value.DescripcionArchivo);
                }
                                
                temaAnterior = value.TituloTema;
                archivoAnterior = value.TituloArchivo;
            });
        }

        //Valida los campos obligatorios del Arbol
        function ListosCamposObligatoriosArbol() {
            var listosCampos = true;

            $.each($("#treelist ul li"), function (index, value) {
                var idTema = $(this).data('idpadre');
                var idArchivo = $(this).data('item');

                var tituloTema = $("#tema-" + idTema + " input").val();
                var tituloArchivo = $("#TituloArchivo-"+idArchivo).val();
                var descripcionArchivo = $("#DescripcionArchivo-"+idArchivo).val();

                $("#tema-" + idTema + " input").removeClass('treeFileValidation');
                if(tituloTema==null || tituloTema==""){
                    listosCampos = false;
                    $("#tema-" + idTema + " [type=text]").addClass('treeFileValidation');
                }
                
                $("#TituloArchivo-"+idArchivo).removeClass('treeFileValidation');
                if(tituloArchivo==null || tituloArchivo==""){
                    listosCampos = false;
                    $("#TituloArchivo-"+idArchivo).addClass('treeFileValidation');
                }

                $("#DescripcionArchivo-"+idArchivo).removeClass('treeFileValidation');
                if(descripcionArchivo==null || descripcionArchivo==""){
                    listosCampos = false;
                    $("#DescripcionArchivo-"+idArchivo).addClass('treeFileValidation');
                }              
            });

            return listosCampos;
        }

        //Valida los campos obligatorios que son combo
        function ListosCamposObligatorios() {
            var msgTipoNoticia = "";
            var msgFechaInicio = "";
            var msgFechaFin = "";
            var listosCampos = true;

            if ($("#FechaInicio").val() == "") {
                msgFechaInicio = "Seleccione la fecha de inicio de la capacitación";
                listosCampos = false;
            }
            if ($("#FechaFin").val() == "") {
                msgFechaFin = "Seleccione la Fecha de finalización de la capacitación";
                listosCampos = false;
            }
            $("#vmFechaInicio").html(msgFechaInicio);
            $("#vmFechaFin").html(msgFechaFin);
            return listosCampos;
        }

        $(document).ready(function () {
            $("#Activo").prop('checked', $("#Activa-v").is(':checked'));

            $(".combo").chosen({
                width: "66%",
                allow_single_deselect: true
            });

            SeleccionaOpcionesGrupos();

            $("#treelist").html('');
            var temas = @Html.Raw(Json.Encode(ViewBag.temas));
            if (temas!= null && temas!=0){
                LlenaArboCapacitacion(temas);
            }else{
                //Inicializa Arbol con 2 temas y 3 espacios de Archivos
                insertaTema(0);
                insertaArchivo(0, 0);
                insertaArchivo(0, 1);
                insertaTema(1);
                insertaArchivo(1, 2);
            }
        });

       //Default para activar pop check por default
        $("input:checkbox").change(function () {
            $("#Activo").prop('checked', $("#Activa-v").is(':checked'));
        });
       
        //------------Sección Scripts para control de arbol de contenido--------------
        $(document).on('click', '.borraArchivo', function () {
            var idTema = $(this).data('idpadre');
            var idArchivo = $(this).data('item');
            $("#tema-" + idTema + " #archivo-" + idArchivo).remove();
        });
        
        $(document).on('click', '.borrarTema', function () {
            var idTema = $(this).data('item');
            $("#tema-" + idTema).remove();
        });

        $(document).on('click', '.nuevoArchivo', function () {
            var idTema = $(this).data('item');
            var noArchivo = $(".item-archivo").length;
            insertaArchivo(idTema,noArchivo)
        });

        $("#nuevoTema").click( function () {
            var noTemas = $(".item-tema").length;
            var noArchivo = $(".item-archivo").length;
            insertaTema(noTemas);
            insertaArchivo(noTemas, noArchivo);
        });

        function insertaTema(noTema) {
            var template = _.template(document.getElementById("templateNuevoTema").textContent);
            var html = template({ notema: noTema });
            $("#treelist").append(html);
        }

        function insertaArchivo(idTema, noArchivo) {
            var template = _.template(document.getElementById("templateArchivoNuevo").textContent);
            var html = template({ notema: idTema, noarchivo: noArchivo });
            $("#tema-" + idTema + " ul").append(html);
        }

        function completaPostConfiguracionCapacitacion() {
            $.each($("#treelist ul li"), function (index, value) {
                var idTema = $(this).data('idpadre');
                var tema = value;
                var tituloTema = $("#tema-" + idTema + " input").val();
                $("#TituloTema-" + index).val(tituloTema);
                $("#DescripcionTema-" + index).val(tituloTema);
                $("#OrdenTema-" + index).val(idTema);
                $("#OrdenArchivo-" + index).val(index);
            });
        }


    </script>

End Section
