@Code
    ViewData("Title") = "Formas"
    ViewData("Descripcion") = "Formas de ruta"
End Code

<div class="card bg-white">
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered datatable table-striped m-b-0">
                <thead>
                    <tr>
                        <th>Checkpoint</th>
                        <th>Forma</th>
                        <th></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>Checkpoint</th>
                        <th>Forma</th>
                        <th></th>
                    </tr>
                </tfoot>
                <tbody>
                    @For Each item In Model
                        Dim currentItem = item
                        @<tr>
                            <td>@currentItem.checkPoint</td>
                            <td>@currentItem.forma</td>
                            <td>
                                @Html.ActionLink("Respuestas", "Respuestas", New With {.id = currentItem.idRutaCheckpoint, .id2 = currentItem.idForma}, New With {.class = "btn btn-dark"})
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
    <script>
        $(document).ready(function () {
            $('.datatable').dataTable();
        });
    </script>
End Section