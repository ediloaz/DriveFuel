@ModelType MVC4Base.Forma

@Code
    ViewData("Title") = "Editar forma " & Model.Descripcion
End Code

<div class="card no-border bg-white">
    <div class="card-block row-equal align-middle">
        <h2>Nueva forma</h2>
        <div class="row">
            <div class="col-lg-8">
                <div id="forma"></div>        
            </div>
            <div class="col-lg-4">
                <div id="preview">
                    <forma-preview :id-forma="idForma"></forma-preview>
                </div>
            </div>
        </div>
       <div class="form-group">
            <label class="col-sm-2 control-label">Cliente</label>
            @Html.DropDownList("idCliente", Nothing, String.Empty, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label">Producto</label>
            @Html.DropDownList("idProducto", Nothing, String.Empty, New With {.class = "form-control"})
        </div>
        <div class="form-group">
            @Html.LabelFor(Function(model) model.Descripcion, New With {.class = "col-sm-2 control-label"})
            <div class="input-group m-b">
                @Html.TextBoxFor(Function(model) model.Descripcion, New With {.class = "form-control"})
                <span class="input-group-btn">
                    <input class="btn btn-success" type="submit" value="Guardar!">
                </span>
                @Html.ValidationMessageFor(Function(model) model.Descripcion)
            </div>
        </div>
    </div>
</div>

<script>var idForma = @Model.IdForma;</script>

@Section Scripts
    @Scripts.Render("~/bundles/noty")
    
    <script src="https://unpkg.com/react@16/umd/react.development.js"></script>
    <script src="https://unpkg.com/react-dom@16/umd/react-dom.development.js"></script>
    <!--<script src="https://cdnjs.cloudflare.com/ajax/libs/mobx/3.4.1/mobx.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/mobx/3.4.1/mobx.umd.js"></script>-->
    <script src="https://unpkg.com/babel-standalone@6.15.0/babel.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/vue"></script>
    @Html.Partial("Vuejs/formaPreview")
End Section

@Html.Partial("React/Forma")