@ModelType  MVC4Base.CapacitacionArchivos

@Code
    ViewData("Title") = "Tema:" & Model.CapacitacionTema.Titulo
    ViewData("Descripcion") = "Archivo:" & Model.Titulo
End Code

<div class="card bg-white">
    <div class="card-header">
        @Html.ActionLink("Visitas", "Visitas", Nothing, New With {.class = "btn btn-info"})
    </div>
    <div class="card-block">
        <div class="text-center m-b">
            <p class="m-t"><h3>@Model.Titulo</h3></p>
            <p class="m-t">@Model.Descripcion</p>
        </div>
        @If Model.URLCompleta IsNot Nothing AndAlso Model.URL <> String.Empty Then
            @<div class="form-group m-l-lg">
                @If Model.TipoArchivo = MVC4Base.EnumTipoArchivo.EsImagen Then
                    @<img src="@Model.URLCompleta" alt="@Model.Titulo" style="max-width: 400px;margin-left: 2em;">
                ElseIf Model.TipoArchivo = MVC4Base.EnumTipoArchivo.EsVideo Then
                    @<video id="sampleMovie" width="400" height="400" controls="">
                         <source src="@Model.URLCompleta" type="video/mp4">
                    </video>
                Else
                    @<a href="@Model.URLCompleta" class="btn-info" target="_blank"></a>
                End If
                
            </div>    
        End If
        
        
    </div>
</div>


@Section Scripts
   
    <script>
        $(document).ready(function () {
           
        });

    </script>
End Section



