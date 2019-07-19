@ModelType MVC4Base.UsuariosAcceso

@Code
    ViewData("Title") = "Acceso a Usuarios"
    ViewData("Descripcion") = "Defina el cliente y/o producto a los que tienen acceso los usuarios Administradores"
End Code

<div class="card no-border bg-white">
    <div class="card-block row-equal align-middle">

        @Using Html.BeginForm()
            @Html.AntiForgeryToken()
            @Html.ValidationSummary(True)
            @Html.HiddenFor(Function(model) model.idUser)
            @<div class="form-group">
                <label class="col-sm-2 control-label">Cliente</label>
                @Html.DropDownList("idCliente", Nothing, String.Empty, New With {.class = "form-control"})
            </div>
            @<div class="form-group">
                <label class="col-sm-2 control-label">Producto</label>
                @Html.DropDownList("idProducto", Nothing, String.Empty, New With {.class = "form-control"})
            </div>
            
            @<div class="row">
                <div class="col-md-10">
                    <input type="submit" value="Agregar" class="btn btn-primary" style="margin-top: 1em"/>
                </div>
            </div>
        End Using
    </div>
</div>

<div class="card no-border bg-white">
    <div class="card-block row-equal align-middle">
        <div class="table-responsive">
            <table class="table table-bordered table-condensed table-striped datatable m-b-0" id="tablaAccesos">
                <thead>
                    <tr>
                        <th>CLIENTE</th>
                        <th>PRODUCTO</th>
                        <th>FECHA AUTORIZACIÓN</th>
                        <th>ATORIZADO POR</th>
                        <th></th>
                    </tr>
                </thead>

                <tbody>
                    @For Each item As MVC4Base.UsuariosAcceso In ViewBag.Accesos
                        Dim currentItem = item
                        @<tr>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.Cliente.NombreCliente)</td>
                            @If currentItem.Producto Is Nothing Then
                                @<td>Todos</td>
                            Else
                                @<td>@Html.DisplayFor(Function(modelItem) currentItem.Producto.NombreProducto)</td>
                            End If
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.fechaAutorización)</td>
                            <td>@Html.DisplayFor(Function(modelItem) currentItem.UsuarioAutoriza.Nombre)</td>
                            <td>
                                @Html.ActionLink(" ", "Delete", "UsuariosAcceso", New With {.idUser = currentItem.idUser, .idAcceso = currentItem.idUsuariosAcceso}, New With {.class = "icon-trash"})
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
     
    </div>
</div>

@Section Styles
    @Styles.Render("~/Content/datatable")
End Section

@Section Scripts
   
    @Scripts.Render("~/bundles/datatable")
    @Scripts.Render("~/bundles/jqueryval")
    
    <script>
        $(document).ready(function () {
            $("#tablaAccesos").DataTable({ responsive: true, "language": DataTables.languaje.es, "order": [[0, "desc"]] });
        });

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
                url: '@Url.Action("ObtenerProductos", "UsuariosAcceso")',
                data: { "idCliente": selectedItemValue },
                success: function (data) {
                    if (data.length == 0){
                        alert('No tiene permiso para visualizar productos');
                        return 0;
                    }

                    ddlProducts.html('');
                    ddlProducts.append($('<option></option>').val(0).html(""))
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
End Section
