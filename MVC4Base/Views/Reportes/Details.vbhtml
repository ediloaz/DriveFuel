@ModelType MVC4Base.Reporte
@imports MVC4Base

@Code
    If ViewBag.esPdf Then
        Layout = "~/Views/Shared/_LayoutEmpty.vbhtml"
    End If
End Code

@Section Styles
    <style>
        li.bg-grey {
            background-color: #F0F0F0 !important;
        }

        .badge-info {
            background-color: #17A2B8 !important;
        }

        .badge-green {
            background-color: #28A745 !important;
        }

        .badge-red {
            background-color: #DC3545 !important;
        }
    </style>
End Section

<left>
    <h2>Reporte de actividades</h2>
    <p class="" style="font-size:1.2em">Generado por <b>@Model.UsuarioSolicitante</b></p>
    <a href="?excel=1" target="_blank" class="btn btn-lg btn-success bg-green-light text-white" style="border-radius: .25rem">Exportar datos a excel</a>
</left>




<center class="m-t p-t">
    <h3><b>Información General</b></h3>
</center>
<div class="row m-t p-t">
    <ul class="list-group list-group-flush">
        <li class="bg-grey list-group-item">Cliente: <strong> @Model.Cliente </strong></li>
        <li class="bg-grey list-group-item">Producto: <strong> @Model.Producto </strong></li>
        <li class="bg-grey list-group-item">Ruta: <strong> @Model.Ruta </strong></li>
        <li class="bg-grey list-group-item">Periodo: <strong> @Model.FechaInicio - @Model.FechaFinal </strong></li>
    </ul>
</div>




<center class="m-t p-t">
    <h3><b>Datos estadísticos</b></h3>
</center>
<div class="row m-t p-t">
    <ul class="list-group list-group-flush">
        <li class="bg-grey list-group-item d-flex justify-content-between align-items-center">
            Checkpoints
            <span class="badge badge-primary badge-pill badge-info">
                @Model.CantidadCheckPoints
            </span>
        </li>
        <li class="bg-grey list-group-item d-flex justify-content-between align-items-center">
            Promotores
            <span class="badge badge-primary badge-pill badge-info">
                @Model.CantidadPromotores
            </span>
        </li>
        <li class="bg-grey list-group-item d-flex justify-content-between align-items-center">
            Asistencias
            <span class="badge badge-primary badge-pill badge-info">
                @Model.CantidadAsistencias
            </span>
        </li>
        <li class="bg-grey list-group-item d-flex justify-content-between align-items-center">
            Faltas
            <span class="badge badge-primary badge-pill badge-info">
                @Model.CantidadAusencias
            </span>
        </li>
        <li class="bg-grey list-group-item d-flex justify-content-between align-items-center">
            Formularios
            <span class="badge badge-primary badge-pill badge-info">
                @Model.CantidadFormularios
            </span>
        </li>
        <li class="bg-grey list-group-item d-flex justify-content-between align-items-center">
            Imágenes
            <span class="badge badge-primary badge-pill badge-info">
                @Model.CantidadImagenes
            </span>
        </li>
    </ul>
</div>



<center class="m-t p-t">
    <h3><b>Checkpoints</b></h3>
</center>
<div class="row m-t p-t">
    <div class="table-responsive">
        <table class="table table-bordered table-striped datatable m-b-0">
            <thead class="text-white bold" style="background:#01A93A">
                <tr>
                    <th>#</th>
                    <th>Dirección</th>
                    <th>Checkpoint</th>
                </tr>
            </thead>
            <tbody>

                @Code Dim contador = 0 End Code
                @For Each checkpoint In ViewBag.CheckPoints
                    Dim checkPointName As String = checkpoint.Descripcion
                    Dim direccion As String = checkpoint.Lugar
                    contador += 1
                    @<tr>
                        <td>
                            @contador
                        </td>
                        <td>
                            @direccion
                        </td>
                        <td>
                            @checkPointName
                        </td>
                    </tr>
                Next
            </tbody>
        </table>
    </div>
</div>




<center class="m-t p-t">
    <h3><b>Promotores</b></h3>
    <span></span>
</center>
<div class="row m-t p-t">
    <div class="table-responsive">
        <table class="table table-bordered table-striped datatable m-b-0">
            <thead class="text-white bold" style="background:#01A93A">
                <tr>
                    <th>Promotor</th>
                    <th>Grupo</th>
                    <th>Check-In</th>
                    <th>Faltas</th>
                </tr>
            </thead>
            <tbody>

                @For Each usuario In Model.Promotores
                    Dim promotor_nombre As String = usuario.Nombre
                    Dim Asistencias As String = usuario.Checkins
                    Dim Faltas As String = usuario.Faltas
                    @<tr>
                        <td>
                            @promotor_nombre
                        </td>
                        <td>
                            @usuario.Correo
                        </td>
                        <td>
                            <span class="badge badge-green badge-pill ">
                                @usuario.Checkins
                            </span>
                        </td>
                        <td>
                            <span class="badge badge-red badge-pill ">
                                @usuario.Faltas
                            </span>
                        </td>
                    </tr>
                Next
            </tbody>
        </table>
    </div>
</div>





<center class="m-t p-t">
    <h3><b>Asistencias</b></h3>
    <span>Usuarios que han realizado su check-in</span>
</center>
<div class="row m-t p-t">
    <div class="table-responsive">
        <table class="table table-bordered table-striped datatable m-b-0">
            <thead class="text-white bold" style="background:#01A93A">
                <tr>
                    <th>Promotor</th>
                    <th>Grupo</th>
                    <th>Check-In</th>
                </tr>
            </thead>
            <tbody>

                @For Each usuario In Model.Promotores
                    Dim promotor_nombre As String = usuario.Nombre
                    Dim Asistencias As String = usuario.Checkins
                    Dim Faltas As String = usuario.Faltas
                    If Asistencias <> "0" Then
                        @<tr>
                            <td>
                                @promotor_nombre
                            </td>
                            <td>
                                @usuario.Correo
                            </td>
                            <td style="width:10%">
                                <center>
                                    <i class="icon-check text-green"></i>
                                </center>
                            </td>
                        </tr>
                    End If

                Next
            </tbody>
        </table>
    </div>
</div>





<center class="m-t p-t">
    <h3><b>Ausencias</b></h3>
    <span>Usuarios que <b>no</b> han realizado su check-in</span>
</center>
<div class="row m-t p-t">
    <div class="table-responsive">
        <table class="table table-bordered table-striped datatable m-b-0">
            <thead class="text-white bold" style="background:#DC3545">
                <tr>
                    <th>Promotor</th>
                    <th>Grupo</th>
                    <th>Check-In</th>
                </tr>
            </thead>
            <tbody>

                @For Each usuario In Model.Promotores
                    Dim promotor_nombre As String = usuario.Nombre
                    Dim Asistencias As String = usuario.Checkins
                    Dim Faltas As String = usuario.Faltas
                    If Faltas <> "0" Then
                        @<tr>
                            <td>
                                @promotor_nombre
                            </td>
                            <td>
                                @usuario.Correo
                            </td>
                            <td style="width:10%">
                                <center>
                                    <i class="icon-close text-red"></i>
                                </center>
                            </td>
                        </tr>
                    End If

                Next
            </tbody>
        </table>
    </div>
</div>







<center class="m-t p-t">
    <h3><b>Fotografías Cargadas Recientemente</b></h3>
    <span></span>
</center>
<div class="row m-t p-t">
    @For Each foto In Model.Imagenes
        @<div class="col-sm-12 col-md-4" style="padding:0 50px">
             <center>
                 <img style="max-height:220px" class="img-thumbnail img-responsive" style="width:96% ; padding:10px 2%" src="@foto" />
             </center>
         </div>
    Next

</div>





<center class="m-t p-t">
    <h3> <b> Formularios</b></h3>
    <span> Últimos cargados por <b>@Model.UsuarioSolicitante</b></span>
</center>
<div class="row m-t p-t">
    <div class="row">
        @For Each cuestionario In Model.Cuestionarios
            @<div class="col-sm-12 col-md-4">
                <div class="card" style="    padding: 20px 40px;">
                    <div class="card-body">
                        <h4 class="card-title"><b>@cuestionario.Nombre</b></h4>
                        <h5 class="card-subtitle mb-2 text-muted">@cuestionario.Fecha</h5>
                    </div>
                </div>
            </div>
        Next
    </div>
</div>

