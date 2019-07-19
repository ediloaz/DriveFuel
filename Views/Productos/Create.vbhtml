@ModelType MVC4Base.Producto

@Code
    ViewData("Title") = "Creación de Productos"
    ViewData("Descripcion") = "Ingresa y selecciona los datos para crear un producto"
End Code


@Using (Html.BeginForm("Create", "Productos", FormMethod.Post, New With {.enctype = "multipart/form-data", .id = "CreateProducto"}))
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<div class="card bg-white">
        <div class="card-header">
            @Html.ActionLink("Regresar", "Index", Nothing, New With {.class = "btn btn-info"})
            <button type="button" class="btn btn-success" id="postGuardaProducto" style="float:right">Guardar Producto</button>
        </div>
        <div class="card-block">
            <div class="row">
                <div class="form-horizontal">
                    <div class="form-group">
                        @Html.Label("Cliente", New With {.class = "col-md-2 control-label"})
                        <div class="col-md-2">
                            @Html.DropDownList("idCliente", Nothing, String.Empty, New With {.class = "col-md-8 chosen-select combo"})
                        </div>
                        <span class="valMessage" id="vmCliente"></span>
                    </div>
                    <div class="form-group">
                        @Html.LabelFor(Function(model) model.NombreProducto, New With {.class = "col-md-2 control-label"})
                        <div class="col-md-6">
                            @Html.TextBoxFor(Function(model) model.NombreProducto, New With {.type = "text", .class = "form-control", .maxlength = "20"})
                            <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.NombreProducto)</span>
                        </div>
                    </div>
                    <div class="form-group">
                        @Html.LabelFor(Function(model) model.Clave, New With {.class = "col-md-2 control-label"})
                        <div class="col-md-3">
                            @Html.TextBoxFor(Function(model) model.Clave, New With {.type = "text", .class = "form-control", .maxlength = "20"})
                            <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.Clave)</span>
                        </div>
                    </div>
                    <div class="form-group">
                        @Html.Label("Activo", New With {.class = "col-md-2 control-label"})
                        <div class="col-md-10">
                            @Html.CheckBoxFor(Function(model) model.Activo, New With {.hidden = "hidden"})
                            <div class="cs-checkbox m-b">
                                <input type="checkbox" value="1" id="Activa-v" checked="checked">
                                <label for="Activa-v"></label>
                            </div>
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
            color: red;
        }
    </style>
End Section

@Section Scripts
    @Scripts.Render("~/bundles/chosen")
    <script>
        $("#postGuardaProducto").on("click", postForm);

        //Submit de Creación de Retención
        function postForm() {

            if (ListosCamposObligatorios() == false) {
                return 0;
            }

            $("#CreateProducto").submit();

        }

        //Valida los campos obligatorios que son combo
        function ListosCamposObligatorios() {
            var msgCliente = "";
            var listosCampos = true;

            if ($("#idCliente").val() == "") {
                msgCliente = "Selecciona un cliente";
                listosCampos = false;
            }

            $("#vmCliente").html(msgCliente);
            return listosCampos;
        }

        $(document).ready(function () {
            $("#Activo").prop('checked', $("#Activa-v").is(':checked'));

            $(".combo").chosen({
                width: "100%",
                allow_single_deselect: true
            });
        });

        $("input:checkbox").change(function () {
            $("#Activo").prop('checked', $("#Activa-v").is(':checked'));
        });
    </script>
End Section


