@ModelType MVC4Base.Cliente

@Code
    ViewData("Title") = "Borrar Cliente " & Model.NombreCliente
    ViewData("Descripcion") = "¿Realmente desea borrar el cliente?"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Using Html.BeginForm()
        @Html.AntiForgeryToken()
                @<input value="Borrar Cliente" type="submit" class="btn btn-danger"/>
                @<a href="@Url.Action("Index","Clientes")" class="btn btn-default">Cancelar</a>
        End Using
    </div>
    <div class="card-block">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-12">
                   @Html.LabelFor(Function(model) model.NombreCliente, New With {.class = "col-md-2 control-label bold"})
                    <div class="col-md-10" style="padding-top:7px">
                        @Html.DisplayFor(Function(model) model.NombreCliente, New With {.type = "text", .class = "col-md-10 form-control"})
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

