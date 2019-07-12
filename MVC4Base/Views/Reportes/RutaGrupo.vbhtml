@ModelType MVC4Base.Grupo

@Code
    ViewData("Title") = "Reporte grupo"
End Code

<style>
    .cuadro {
        background: #8ba73a;
        height: 100%;
    }

    .contenedor_datos {
        padding: 10px;
        text-align: center;
    }

    .contenedor_usuario {
        margin-top: 4px;
    }

    .fila {
        height: 108px;
    }

    h3 {
        margin-top: 0 !important;
    }

    .bg-ficha {
        background-color: #e9e9ea;
    }
</style>

<div class="main-content">
    @For Each usuario As MVC4Base.Usuarios In Model.Usuarios
        @<a href="@Url.Action("RutaUsuario", New With {.id = ViewBag.idRuta, .id2 = usuario.idUsuario})">
            <div class="card card-block bg-white no-border" style="overflow: hidden;">
                <div class="row fila">
                    <div class="col-md-3 contenedor_usuario">
                        <h3>@usuario.Nombre</h3>
                        <h3>Grupos: @usuario.GruposCount</h3>
                        <h3>Rutas @usuario.RutasCount</h3>
                    </div>
                    <div class="col-md-2">
                        <div class="contenedor_datos bg-ficha">
                            <h3>Checkins</h3>
                            <h3>@usuario.Checkins</h3>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="contenedor_datos bg-ficha">
                            <h3>Faltas</h3>
                            <h3>@usuario.Faltas</h3>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="contenedor_datos bg-ficha">
                            <h3>Cuestionarios</h3>
                            <h3>@usuario.Cuestionarios</h3>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="contenedor_datos bg-ficha">
                            <h3>Multimedia</h3>
                            <h3>@usuario.Multimedia</h3>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="contenedor_datos bg-ficha">
                            <h3>Capacitación</h3>
                            <h3>@usuario.Capacitacion</h3>
                        </div>
                    </div>
                </div>
            </div>
        </a>
    Next
    
</div>