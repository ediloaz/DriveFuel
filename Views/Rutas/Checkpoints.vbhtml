@ModelType MVC4Base.RutaCheckpoint()

@Code
    ViewData("Title") = "Checkpoints"
End Code

<div class="card bg-white">
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered table-striped m-b-0">
                <thead>
                    <tr>
                        <th>CheckPoint</th>
                        <th></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>CheckPoint</th>
                        <th></th>
                    </tr>
                </tfoot>
                <tbody>
                    @For Each item In Model
                        @<tr>
                            <td>@item.Descripcion</td>
                            <td>
                                @Html.ActionLink("Checks", "Checks", New With {.id = item.idRutaCheckPoint}, New With {.class = "btn btn-dark"})
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>
