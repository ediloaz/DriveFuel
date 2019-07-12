@code
    Layout = Nothing
End Code
@If ViewData.Model Is Nothing Then
    @<span>NA</span>
Else
    @<span class="glyphicon glyphicon-@IIF(ViewData.Model, "check", "unchecked") check" title="@IIf(ViewData.Model, "Si", "No")"></span>
End If