@ModelType MVC4Base.Producto
@Code
    ViewData("Title") = "Borrar Producto " & Model.NombreProducto
    ViewData("Descripcion") = "¿Realmente desea borrar el producto?"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Using Html.BeginForm()
            @Html.AntiForgeryToken()
            @<input value="Borrar Producto" type="submit" class="btn btn-danger" />
            @<a href="@Url.Action("Index", "Productos")" class="btn btn-default">Cancelar</a>
        End Using
    </div>
    <div class="card-block">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-12">
                    @Html.LabelFor(Function(model) model.NombreProducto, New With {.class = "col-md-2 control-label bold"})
                    <div class="col-md-10" style="padding-top:7px">
                        @Html.DisplayFor(Function(model) model.NombreProducto, New With {.type = "text", .class = "col-md-10 form-control"})
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    @Html.LabelFor(Function(model) model.Clave, New With {.class = "col-md-2 control-label bold"})
                    <div class="col-md-10" style="padding-top:7px">
                        @Html.DisplayFor(Function(model) model.Clave, New With {.type = "text", .class = "col-md-10 form-control"})
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    @Html.LabelFor(Function(model) model.Cliente.NombreCliente, New With {.class = "col-md-2 control-label bold"})
                    <div class="col-md-10" style="padding-top:7px">
                        @Html.DisplayFor(Function(model) model.Cliente.NombreCliente, New With {.type = "text", .class = "col-md-10 form-control"})
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    @Html.LabelFor(Function(model) model.Activo, New With {.class = "col-md-2 control-label bold"})
                    <div class="col-md-10" style="padding-top:7px">
                        @Html.DisplayFor(Function(model) model.ActivoTexto, New With {.type = "text", .class = "col-md-10 form-control"})
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>

