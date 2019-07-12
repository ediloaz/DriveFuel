@ModelType IEnumerable(Of MVC4Base.webpages_Roles)

@Code
    ViewData("Title") = "Index"
End Code

<h2>Index</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table>
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.RoleId)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.RoleName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Description)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    Dim currentItem = item
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.RoleId)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.RoleName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Description)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = currentItem.RoleId}) |
            @Html.ActionLink("Details", "Details", New With {.id = currentItem.RoleId}) |
            @*@Html.ActionLink("Delete", "Delete", New With {.id = currentItem.RoleId})*@
        </td>
    </tr>
Next

</table>
