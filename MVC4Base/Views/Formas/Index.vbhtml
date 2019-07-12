@ModelType IEnumerable(Of MVC4Base.Forma)

@Code
    ViewData("Title") = "Formas"
End Code

<div class="card-header">
    Formas &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    @Html.ActionLink("Nueva forma", "Create", Nothing, New With {.class = "btn btn-success"})
</div>

<div class="card-block">
<div class="table-responsive">
  <table class="table table-bordered datatable table-striped m-b-0">
    <thead>
      <tr>
        <th>@Html.DisplayNameFor(Function(model) model.IdForma)</th>
        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
        <th># Preguntas</th>
        <th></th>
      </tr>
    </thead>
    <tfoot>
      <tr>
        <th>@Html.DisplayNameFor(Function(model) model.IdForma)</th>
        <th>@Html.DisplayNameFor(Function(model) model.Descripcion)</th>
        <th># Preguntas</th>
        <th></th>
      </tr>
    </tfoot>
    <tbody>
    @For Each item In Model
        Dim currentItem = item
        @<tr>
            <td>
                @Html.DisplayFor(Function(modelItem) currentItem.IdForma)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) currentItem.Descripcion)
            </td>
            <td>@item.FormaPregunta.Count</td>
            <td>
                @Html.ActionLink("Editar", "Edit", New With {.id = currentItem.IdForma}, New With {.class = "btn btn-success"})
                &nbsp;&nbsp;
                @Html.ActionLink("Borrar", "Delete", New With {.id = currentItem.IdForma}, New With {.class = "btn btn-danger"})
            </td>
        </tr>
    Next
    </tbody>
  </table>
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