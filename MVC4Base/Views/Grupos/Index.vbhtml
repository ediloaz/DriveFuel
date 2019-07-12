@ModelType IEnumerable(Of MVC4Base.Grupo)

@Code
    ViewData("Title") = "Grupos"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Nuevo Grupo", "Create", Nothing, New With {.class = "btn btn-success"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-striped m-b-0">
                <thead>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th></th>
                    </tr>
                </tfoot>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr>
                            <td>
                                @Html.DisplayFor(Function(modelItem) currentItem.Descripcion)
                            </td>
                            <td>
                                @Html.ActionLink("Editar", "Edit", New With {.id = currentItem.idGrupo}, New With {.class = "btn btn-dark"})
                                &nbsp;&nbsp;
                                @Html.ActionLink("Borrar", "Delete", New With {.id = currentItem.idGrupo}, New With {.class = "btn btn-danger"})
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>
