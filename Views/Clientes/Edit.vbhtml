@ModelType MVC4Base.Cliente

@Code
    ViewData("Title") = "Edición de Cliente " & Model.NombreCliente
    ViewData("Descripcion") = "Edita los datos del cliente"
End Code


@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<div class="card bg-white">
        <div class="card-header">
            @Html.ActionLink("Regresar", "Index", Nothing, New With {.class = "btn btn-info"})
            <button type="submit" value="create" class="btn btn-success" style="float:right">Guardar Cliente</button>
        </div>
        <div class="card-block">
            <div class="row">
                <div class="form-horizontal">
                    @Html.HiddenFor(Function(model) model.idCliente)
                    <div class="form-group">
                        @Html.LabelFor(Function(model) model.NombreCliente, New With {.class = "col-md-2 control-label"})
                        <div class="col-md-6">
                            @Html.TextBoxFor(Function(model) model.NombreCliente, New With {.type = "text", .class = "form-control", .maxlength = "20"})
                            <span class="valMessage">@Html.ValidationMessageFor(Function(model) model.NombreCliente)</span>
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
    <style>
        .valMessage {
            color: red;
        }
    </style>
End Section

@Section Scripts
    <script>
        $(document).ready(function () {
            $("#Activo").prop('checked', $("#Activa-v").is(':checked'));
        });

        $("input:checkbox").change(function () {
            $("#Activo").prop('checked', $("#Activa-v").is(':checked'));
        });
    </script>
End Section
