@ModelType MVC4Base.Grupo

@Code
    ViewData("Title") = "Editar grupo " & Model.Descripcion
End Code

<script>
    $("#idCliente").change(function () {
        var ddlProducts = $("#idProducto");
        var selectedItemValue = $(this).val();
        if(selectedItemValue == ""){
            ddlProducts.html('');
            return 0;
        }

        $.ajax({
            cache: false,
            type: "GET",
            url: '@Url.Action("ObtenerProductos", "Productos")',
            data: { "idCliente": selectedItemValue },
            success: function (data) {
                if (data.length == 0){
                    alert('No tiene permiso para visualizar productos');
                    return 0;
                }

                ddlProducts.html('');
                $.each(data, function (id, option) {
                    ddlProducts.append($('<option></option>').val(option.idProducto).html(option.NombreProducto));
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error al cargar producto.');
            }
        });
    });
</script>

<div class="card no-border bg-white">
    <div class="card-block row-equal align-middle">

        @Using Html.BeginForm()
            @Html.AntiForgeryToken()
            @Html.ValidationSummary(True)
            @Html.HiddenFor(Function(model) model.idGrupo)
            @<div class="form-group">
                <label class="col-sm-2 control-label">Cliente</label>
                @Html.DropDownList("idCliente", Nothing, String.Empty, New With {.class = "form-control"})
            </div>
            @<div class="form-group">
                <label class="col-sm-2 control-label">Producto</label>
                @Html.DropDownList("idProducto", Nothing, String.Empty, New With {.class = "form-control"})
            </div>
            @<div class="form-group">
                @Html.LabelFor(Function(model) model.Descripcion, New With {.class = "col-sm-2 control-label"})
                <div class="input-group m-b">
                    @Html.TextBoxFor(Function(model) model.Descripcion, New With {.class = "form-control"})
                    <span class="input-group-btn">
                        <input class="btn btn-success" type="submit" value="Guardar!">
                    </span>
                    @Html.ValidationMessageFor(Function(model) model.Descripcion)
                </div>
            </div>
        End Using
    </div>
</div>
<div class="card no-border bg-white">
    <div class="card-block row-equal align-middle">
        <div id="editarGrupo" class="col-md-12">
            <users-selector @@save="save" @@cancel="cancel"
                            users-path="/Grupos/usuarios/@ViewBag.id" only-users="true"></users-selector>
        </div>
    </div>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/noty")
    <script src="https://cdn.jsdelivr.net/npm/vue"></script>

    @Html.Partial("Vuejs/usuariosSelector")

    <script type="text/javascript">
        var app = new Vue({
            el: '#editarGrupo',
            data: {},
            methods: {
                save: function (usuarios, grupos) {
                    console.log("save___ usuarios", usuarios);
                    console.log("save___ grupos", grupos);
                },
                cancel: function () {
                    console.log("cancel");
                    window.location.href = '/Grupos';
                }
            }
        });
    </script>
End Section
