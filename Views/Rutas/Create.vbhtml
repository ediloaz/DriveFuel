@Code
    ViewData("Title") = "Creación de Rutas"
    ViewData("Descripcion") = "Ingresa los datos correspondientes en cada una de las pestañas para la creación de la ruta"
    Dim mapApiKey As String = ViewBag.mapApiKey
End Code

@Section Styles
@Styles.Render("~/Content/datepicker")
@Styles.Render("~/Content/clockpicker")
    <style>
        #map {
            height: 500px;
            width: 100%;
        }
        #container-status.new .edit {
            display: none;
        }
        #container-status.edit .new {
            display: none;
        }
        .add-remove-user {
          cursor: pointer;
        }
        .icon-notebook {
            cursor: pointer;
        }
        .userSelector {
          font-size: 25px;
        }
        .userText {
          font-size: 15px;
        }
        .row.options {
            padding: 10px;
        }
    </style>
End Section

@Section Scripts
@Scripts.Render("~/bundles/datepicker")
@Scripts.Render("~/bundles/clockpicker")
@Scripts.Render("~/bundles/noty")
@Scripts.Render("~/bundles/chosen")
@Scripts.Render("~/bundles/card")
@Scripts.Render("~/bundles/jqurey-validation")
@Scripts.Render("~/bundles/checkbo")
@Scripts.Render("~/bundles/twitter-bootstrap-wizard")

<script src="https://unpkg.com/react@16/umd/react.development.js"></script>
<script src="https://unpkg.com/react-dom@16/umd/react-dom.development.js"></script>
<!--<script src="https://cdnjs.cloudflare.com/ajax/libs/mobx/3.4.1/mobx.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/mobx/3.4.1/mobx.umd.js"></script>-->
<script src="https://unpkg.com/babel-standalone@6.15.0/babel.min.js"></script>

<script>
    $("#idCliente").change(function () {
        var ddlProducts = $("#idProducto");
        var selectedItemValue = $(this).val();
        if(selectedItemValue == ""){
            ddlProducts.html('');
            return 0;
        }
       
        $.ajax({
            cache: false,
            type: "GET",
            url: '@Url.Action("ObtenerProductos", "Productos")',
            data: { "idCliente": selectedItemValue },
            success: function (data) {
                if (data.length == 0){
                    alert('No tiene permiso para visualizar productos');
                    return 0;
                }

                ddlProducts.html('');
                $.each(data, function (id, option) {
                    ddlProducts.append($('<option></option>').val(option.idProducto).html(option.NombreProducto));
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Error al cargar producto.');
            }
        });
    });
</script>

End Section

<div class="card">
  <div class="card-block p-a-0">
    <div class="box-tab m-b-0" id="rootwizard">
      <ul class="wizard-tabs">
        <li class="active"><a href="#tab1" data-toggle="tab" aria-expanded="true">Ruta</a></li>
        <li><a href="#tab2" data-toggle="tab">Usuarios</a></li>
        <li><a href="#tab3" data-toggle="tab">Formas</a></li>
      </ul>
      <div class="tab-content">
        <div class="tab-pane active" id="tab1">
          <div id="container-status" class="@(If(ViewBag.idRuta isnot Nothing, "edit","new"))">
            <div class="row edit">
              <div class="form-group">
                <label class="col-sm-12 control-label">Buscar</label>
                <input type="text" class="form-control" id="autocomplete">
              </div>
            </div>
            <div class="row edit">
                <div class="col-md-3">
                    <div class="form-group">
                        <label class="col-sm-12 control-label">Lugar</label>
                        <div class="col-sm-12">
                            <input type="text" class="form-control" id="lugar">
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12 control-label">Descripción</label>
                        <div class="col-sm-12">
                            <input type="text" class="form-control" id="descripcion">
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12 control-label">Llegada</label>
                        <div class="col-sm-12">
                            <input type="text" class="form-control m-b" data-provide="datepicker" placeholder="Llegada" id="llegada">
                        </div>
                        <div class="input-group input-group-lg clockpicker w200">
                            <input type="text" class="form-control" value="10:10" id="llegadaHora">
                            <span class="input-group-addon">
                                <span class="fa fa-clock-o"></span>
                            </span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12 control-label">Salida</label>
                        <div class="col-sm-12">
                            <input type="text" class="form-control m-b" data-provide="datepicker" placeholder="Salida" id="salida">
                        </div>
                        <div class="input-group input-group-lg clockpicker w200">
                            <input type="text" class="form-control" value="10:10" id="salidaHora">
                            <span class="input-group-addon">
                                <span class="fa fa-clock-o"></span>
                            </span>
                        </div>
                    </div>
                    <button type="button" class="btn btn-success" id="updatePoint" onclick="updatePoint()">Actualizar</button>
                    <button type="button" class="btn btn-warning" id="deletePoint" onclick="deletePoint()">Borrar</button>
                </div>
                <div class="col-md-9">
                    <div id="map"></div>
                </div>
            </div>

            <br />
              <div class="form-group">
                  <label class="col-sm-2 control-label">Cliente</label>
                  @Html.DropDownList("idCliente", Nothing, String.Empty, New With {.class = "form-control"})
              </div>
              <div class="form-group">
                  <label class="col-sm-2 control-label">Producto</label>
                  @Html.DropDownList("idProducto", Nothing, String.Empty, New With {.class = "form-control"})
              </div>
              <div class="form-group">
                  <label class="col-sm-2 control-label">Descripción</label>
                  <div class="input-group m-b">
                      <input type="text" class="form-control" id="nombre_ruta">
                      <span class="input-group-btn">
                          <button class="btn btn-success" type="button" onclick="guardar()">Guardar!</button>
                      </span>
                  </div>
              </div>
            </div>
        </div>
        <div class="tab-pane p-x-lg" id="tab2">
          <div id="usuarios"></div>
        </div>
        <div class="tab-pane p-x-lg" id="tab3">
            <div class="row">
                <div class="col-lg-8">
                    <div id="formas"></div>
                </div>
                <div class="col-lg-4">
                    <div id="preview">
                      <forma-preview :id-forma="idForma"></forma-preview>
                    </div>
                </div>
            </div>
        </div>
        <div class="wizard-pager">
        </div>
      </div>
    </div>
  </div>
</div>

<script src="https://cdn.jsdelivr.net/npm/vue"></script>

@If ViewBag.idRuta IsNot Nothing Then
@<script>var idRuta = @ViewBag.idRuta;</script>
Else
@<script>var idRuta;</script>
End If

<script>
    var map;
    var placesService;
    var coordInfoWindow;
    var points = [];
    var actualPoint;
    var urlRutas = '/api/ApiRutas';
    var urlRutasCheckpoint = '/api/ApiRutaCheckpoint';
    var propsIgnoreInput = ['idRutaCheckPoint', 'latitud', 'longitud'];
    var usuariosApp;
    var formasApp;
    var DOMContentLoaded = false;

    document.addEventListener('DOMContentLoaded', function () {
        DOMContentLoaded = true;
        $.fn.datepicker.defaults.autoclose = true;
        $('.clockpicker').clockpicker({donetext: 'Done', autoclose: true});
        if(!map) initMap();
    });

    function initMap() {
        if(!idRuta || !DOMContentLoaded || map) return;
        var uluru = { lat: 19.4341234, lng: -99.1363569 };
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 16,
            center: uluru
        });
        placesService = new google.maps.places.PlacesService( map );
        var input = document.getElementById('autocomplete');
        var options = {
          types: ['geocode']
        };

        autocomplete = new google.maps.places.Autocomplete(input, options);
        map.addListener('click', function (e) {
            var marker = new google.maps.Marker({
                position: e.latLng,
                map: map,
                draggable: true
            });
            var point = {marker: marker, data: {
                idRutaCheckPoint: 0,
                latitud: e.latLng.lat(),
                longitud: e.latLng.lng(),
                lugar: '',
                descripcion: '',
                llegada: '',
                llegadaHora: '',
                salida: '',
                salidaHora: ''
            }};
            if(e.placeId) {
              placesService.getDetails({placeId: e.placeId}, (place, status) => {
                console.log(place);
                console.log(status);
                point.data.lugar = place.name;
                viewMarker(point);
              });
            } else {
              viewMarker(point);
            }
            marker.addListener('click', (e) => viewMarker(point));
            marker.addListener('dragend', (e) => updateMarkerLatLng(e, point));
            points.push(point);
        });
        autocomplete.addListener('place_changed', (e) => {
          var place = autocomplete.getPlace();
          map.setCenter(place.geometry.location);
          var marker = new google.maps.Marker({
                position: place.geometry.location,
                map: map,
                draggable: true
            });
          var point = {marker: marker, data: {
                idRutaCheckPoint: 0,
                latitud: place.geometry.location.lat(),
                longitud: place.geometry.location.lng(),
                lugar: place.name,
                descripcion: '',
                llegada: '',
                llegadaHora: '',
                salida: '',
                salidaHora: ''
            }};
            marker.addListener('click', (e) => viewMarker(point));
            marker.addListener('dragend', (e) => updateMarkerLatLng(e, point));
            points.push(point);
            viewMarker(point);
        });
        initPoints();
        coordInfoWindow = new google.maps.InfoWindow();
    }
    function formatDateBD(date) {
        let parts = date.split('-');
        return `${parts[1]}/${parts[2]}/${parts[0]}`;
    }
    function initPoints() {
        if(idRuta) {
            $.get(`${urlRutas}/${idRuta}`, (data) => {
                document.getElementById('nombre_ruta').value = data.Descripcion;
                document.getElementById('idCliente').value = data.idCliente;
                document.getElementById('idProducto').value = data.idProducto;
                if(data.RutaCheckpoint.length > 0) {
                    map.setCenter({
                        lat: data.RutaCheckpoint[0].Latitud,
                        lng: data.RutaCheckpoint[0].Longitud
                    });
                }
                for(var i=0; i < data.RutaCheckpoint.length; i++) {
                    let point = {data: {
                        idRutaCheckPoint: data.RutaCheckpoint[i].idRutaCheckPoint,
                        latitud: data.RutaCheckpoint[i].Latitud,
                        longitud: data.RutaCheckpoint[i].Longitud,
                        lugar: data.RutaCheckpoint[i].Lugar,
                        descripcion: data.RutaCheckpoint[i].Descripcion,
                        llegada: formatDateBD(data.RutaCheckpoint[i].Llegada.split('T')[0]),
                        llegadaHora: data.RutaCheckpoint[i].Llegada.split('T')[1].slice(0,5),
                        salida: formatDateBD(data.RutaCheckpoint[i].Salida.split('T')[0]),
                        salidaHora: data.RutaCheckpoint[i].Salida.split('T')[1].slice(0,5)
                    }};
                    //delete point.data['$id'];
                    //delete point.data['Ruta'];
                    console.log({lat: point.data.latitud, lng: point.data.longitud});
                    point.marker = new google.maps.Marker({
                        position: {lat: point.data.latitud, lng: point.data.longitud},
                        map: map,
                        draggable: true
                    });
                    point.marker.addListener('click', (e) => viewMarker(point));
                    point.marker.addListener('dragend', (e) => updateMarkerLatLng(e, point));
                    points.push(point);
                }
            });
        }
    }
    function viewMarker(point) {
        actualPoint = point;
        for(var prop in actualPoint.data) {
            if(propsIgnoreInput.indexOf(prop) >= 0) continue;
            document.getElementById(prop).value = actualPoint.data[prop];
        }
        document.getElementById('updatePoint').classList.remove('disabled');
        coordInfoWindow.setContent(point.data.lugar);
        coordInfoWindow.setPosition({lat: point.data.latitud, lng: point.data.longitud});
        coordInfoWindow.open(map);
    }
    function updateMarkerLatLng(e, point) {
      point.data.latitud = e.latLng.lat();
      point.data.longitud = e.latLng.lng();
      viewMarker(point);
    }
    function updatePoint(e) {
        for(var prop in actualPoint.data) {
            if(propsIgnoreInput.indexOf(prop) >= 0) continue;
            actualPoint.data[prop] = document.getElementById(prop).value;
        }
        var checkPointData = JSON.parse(JSON.stringify(actualPoint.data));
        checkPointData.idRuta = idRuta;
        checkPointData.llegada += ' ' + checkPointData.llegadaHora;
        checkPointData.salida += ' ' + checkPointData.salidaHora;
        delete checkPointData['llegadaHora'];
        delete checkPointData['salidaHora'];
        if(!actualPoint.data.idRutaCheckPoint) {
            $.post(urlRutasCheckpoint, checkPointData, (data) => {
                actualPoint.data.idRutaCheckPoint = data.idRutaCheckPoint;
                cleanCheckPointForm();
                noty({
                  theme: 'app-noty',
                  text: 'Checkpoint guardado',
                  type: 'success',
                  timeout: 3000,
                  layout: 'bottomRight',
                  closeWith: ['button', 'click'],
                  animation: {
                    open: 'in',
                    close: 'out'
                  },
                });
            }).fail((e) => {
                noty({
                  theme: 'app-noty',
                  text: e.responseJSON.Message,
                  type: 'success',
                  timeout: 3000,
                  layout: 'bottomRight',
                  closeWith: ['button', 'click'],
                  animation: {
                    open: 'in',
                    close: 'out'
                  },
                });
            });
        } else {
            checkPointData.id = actualPoint.data.idRutaCheckPoint;
            $.ajax({
                url: `${urlRutasCheckpoint}/${actualPoint.data.idRutaCheckPoint}`,
                type: 'PUT',
                data: JSON.stringify(checkPointData),
                contentType: "application/json; charset=utf-8",
                dataType: "text"
            }).success((result) =>  {
                noty({
                  theme: 'app-noty',
                  text: 'Ruta guardada',
                  type: 'success',
                  timeout: 3000,
                  layout: 'bottomRight',
                  closeWith: ['button', 'click'],
                  animation: {
                    open: 'in',
                    close: 'out'
                  },
                });
                cleanCheckPointForm();
            }).error((e) => {
                noty({
                  theme: 'app-noty',
                  text: e,
                  type: 'error',
                  timeout: 3000,
                  layout: 'bottomRight',
                  closeWith: ['button', 'click'],
                  animation: {
                    open: 'in',
                    close: 'out'
                  },
                });
            });
        }
    }
    function deletePoint(e) {
        var index = points.indexOf(actualPoint);
        if(!actualPoint.data.idRutaCheckPoint) {
            actualPoint.marker.setMap(null);
            points.slice(index, 1);
            noty({
              theme: 'app-noty',
              text: 'Ruta borrada',
              type: 'success',
              timeout: 3000,
              layout: 'bottomRight',
              closeWith: ['button', 'click'],
              animation: {
                open: 'in',
                close: 'out'
              },
            });
        } else {
            $.ajax({
                url: `${urlRutasCheckpoint}/${actualPoint.data.idRutaCheckPoint}`,
                type: 'DELETE',
                success: (result) =>  {
                    points.slice(index, 1);
                    noty({
                      theme: 'app-noty',
                      text: 'Ruta borrada',
                      type: 'success',
                      timeout: 3000,
                      layout: 'bottomRight',
                      closeWith: ['button', 'click'],
                      animation: {
                        open: 'in',
                        close: 'out'
                      },
                    });
                    cleanCheckPointForm();
                },
                failure: (e) => {
                    alert(e);
                }
            });
        }
    }
    function cleanCheckPointForm() {
        document.getElementById('updatePoint').classList.add('disabled');
        for(var prop in actualPoint.data) {
            if(propsIgnoreInput.indexOf(prop) >= 0) continue;
            document.getElementById(prop).value = '';
        }
    }
    function guardar() {
        var nombreRuta = document.getElementById('nombre_ruta').value;
        var idCliente = document.getElementById('idCliente').value;
        var idProducto = document.getElementById('idProducto').value;
        if(!idRuta) {
            $.post(urlRutas, {descripcion: nombreRuta, idCliente: idCliente, idProducto: idProducto}, (data) => {
                console.log(data);
                idRuta = data.idRuta;
                usuariosApp.getUsers(idRuta);
                document.getElementById('container-status').classList.remove('new');
                document.getElementById('container-status').classList.remove('edit');
                initMap();
                formasApp.getFormas()
                noty({
                      theme: 'app-noty',
                      text: 'Ruta creada',
                      type: 'success',
                      timeout: 3000,
                      layout: 'bottomRight',
                      closeWith: ['button', 'click'],
                      animation: {
                        open: 'in',
                        close: 'out'
                      },
                    });
            }).fail((e) => {
                noty({
                      theme: 'app-noty',
                      text: e.responseJSON.Message,
                      type: 'error',
                      timeout: 3000,
                      layout: 'bottomRight',
                      closeWith: ['button', 'click'],
                      animation: {
                        open: 'in',
                        close: 'out'
                      },
                    });
            });
        } else {
            $.ajax({
                url: `${urlRutas}/${idRuta}`,
                type: 'PUT',
                data: JSON.stringify({idRuta: idRuta,descripcion: nombreRuta, idCliente: idCliente, idProducto: idProducto}),
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: (result) =>  {
                    noty({
                      theme: 'app-noty',
                      text: 'Ruta guardada',
                      type: 'success',
                      timeout: 3000,
                      layout: 'bottomRight',
                      closeWith: ['button', 'click'],
                      animation: {
                        open: 'in',
                        close: 'out'
                      },
                    });
                },
                failure: (e) => {
                    noty({
                      theme: 'app-noty',
                      text: e,
                      type: 'error',
                      timeout: 3000,
                      layout: 'bottomRight',
                      closeWith: ['button', 'click'],
                      animation: {
                        open: 'in',
                        close: 'out'
                      },
                    });
                }
            });
        }
    }
</script>

@Html.Partial("React/_Usuarios")
@Html.Partial("React/_Formas")
@Html.Partial("Vuejs/formaPreview")


<script async defer
        src="https://maps.googleapis.com/maps/api/js?libraries=places&key=@(mapApiKey)&callback=initMap">
</script>