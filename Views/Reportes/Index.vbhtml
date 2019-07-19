﻿@ModelType IEnumerable(Of MVC4Base.Ruta)

@Code
    ViewData("Title") = "Reportes"
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Nueva Ruta", "Create", Nothing, New With {.class = "btn btn-success"})
    </div>
    <div class="card-block">
        <div class="table-responsive">
            <table class="table table-bordered datatable table-striped m-b-0">
                <thead>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th># Checkpoints</th>
                        <th></th>
                    </tr>
                </thead>
                <tfoot>
                    <tr>
                        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
                        <th># Checkpoints</th>
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
                            <td>@item.RutaCheckpoint.Count</td>
                            <td>
                                @Html.ActionLink("Ver", "Details", New With {.id = currentItem.idRuta}, New With {.class = "btn btn-dark"})
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
