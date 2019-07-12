@ModelType MVC4Base.Ruta

@Code
    ViewData("Title") = "Delete"
End Code

<div class="card bg-white">
    <div class="card-header">
        Borrar ruta
    </div>

    <h3>¿Estas seguro de borrar esta ruta?</h3>
    <div class="card-block">
        <div class="display-field">
            @Html.DisplayNameFor(Function(model) model.Descripcion) : @Html.DisplayFor(Function(model) model.Descripcion)
        </div>
        <div class="display-field">
            Checkpoints: @Model.RutaCheckpoint.Count
        </div>
        <br/>
    @Using Html.BeginForm()
        @Html.AntiForgeryToken()
        @<p>
            <input type="submit" value="Borrar" class="btn btn-danger" />
            @Html.ActionLink("Cancelar", "Index", Nothing, New With {.class = "btn btn-success"})
        </p>
    End Using
    </div>
</div>