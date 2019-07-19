<script type="text/template" id='templateNuevoTema'>
    <li id="tema-<%= notema %>" data-item="<%= notema %>" class="item-tema">
        <input type="text" placeholder="Tema <%= notema %>" id="Tema-<%= notema %>">
        <a class="icon-plus nuevoArchivo" data-item="<%= notema %>"></a>
        <a class="icon-trash borrarTema" data-item="<%= notema %>"></a>
        <ul></ul>
    </li>
</script>

<script type="text/template" id='templateArchivoNuevo'>
    <li id="archivo-<%= noarchivo %>" data-idpadre="<%= notema %>" data-item="<%= noarchivo %>" class="item-archivo">
        <div style="display:inline-block">
            <input name="temas[<%= noarchivo %>].MultimediaFile" id="MultimediaFile-<%= noarchivo %>" type="file" />
            <input name="temas[<%= noarchivo %>].TituloArchivo" type="text" data-idpadre="<%= notema %>" data-item="<%= noarchivo %>" placeholder="Titulo de Contenido" id="TituloArchivo-<%= noarchivo %>">
            <input name="temas[<%= noarchivo %>].DescripcionArchivo" type="text" class="descripcionContenido" data-idpadre="<%= notema %>" data-item="<%= noarchivo %>" placeholder="Descripción de Contenido" id="DescripcionArchivo-<%= noarchivo %>">
            <input name="temas[<%= noarchivo %>].TituloTema" id="TituloTema-<%= noarchivo %>" type="hidden">
            <input name="temas[<%= noarchivo %>].DescripcionTema" id="DescripcionTema-<%= noarchivo %>" type="hidden">
            <input name="temas[<%= noarchivo %>].OrdenTema" id="OrdenTema-<%= noarchivo %>" type="hidden">
            <input name="temas[<%= noarchivo %>].OrdenArchivo" id="OrdenArchivo-<%= noarchivo %>" type="hidden">
            <input name="temas[<%= noarchivo %>].idTemaCapacitacion" id="idTemaCapacitacion-<%= noarchivo %>" type="hidden">
            <input name="temas[<%= noarchivo %>].idCapacitacionArchivos" id="idCapacitacionArchivos-<%= noarchivo %>" type="hidden">
        </div>
        <a class="icon-trash borraArchivo" data-idpadre="<%= notema %>" data-item="<%= noarchivo %>"></a>
    </li>
</script>

<script type="text/template" id='CapacitacionTemasHidden'>
    <div class="row">
        <div><input type="hidden" name="temas.idTemaCapacitacion" value="<%= contador %>" /></div>
        <div><input type="hidden" name="temas[<%= contador %>].TituloTema" value="<%= TituloTema %>" /></div>
        <div><input type="hidden" name="temas[<%= contador %>].TituloTema" value="<%= TituloTema %>" /></div>
        <div><input type="hidden" name="temas[<%= contador %>].TituloTema" value="<%= detalle.ImporteExentoPercepcion %>" /></div>
    </div>
</script>