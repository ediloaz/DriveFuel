@ModelType MVC4Base.DashboardViewModel

@Code
    ViewData("Title") = "Home Dashboard"
End Code

<div class="row">
    <div class="row">
        <div class="col-sm-12">
            <div class="card bg-white" @*style="background-color:#76990f"*@>
                <div id="legend-container" style="float:right"></div>
                <div class="card-block text-center">
                    <div class="chart line" style="height: 320px; padding: 0px; position: relative;"></div>
                </div>
            </div>
        </div>
    </div>
</div>

@Code
    If (Model.Role.ToString() = "Cliente") Then
        @Html.Partial("_DashboardCliente")
    Else
        @Html.Partial("_DashboardAdmin")
    End If
End Code






@Section styles

    <style>
        .round-i {
            display: inline-block;
            border-radius: 20px;
            padding: 7px 7px;
            color: white;
        }

        #legend-container {
            margin: 0.3em 2em 0em 0em !important;
            padding: 2px;
            display: inline-block;
            margin: 0 auto;
            color: black;
        }

        .legendLabel {
            color: gray;
        }
    </style>

End Section

@Section Scripts
    @Scripts.Render("~/bundles/noty")
    @Scripts.Render("~/Content/vendor/flot")

    <script>
        var datosGrafica = @Html.Raw(Json.Encode(Model.ChecksRecientes));
        console.log(datosGrafica)
    </script>
    <script src="~/Scripts/charts/flot.js"></script>
    <script>
        var Imagenes = @Html.Raw(Json.Encode(Model.Imagenes));
        var idImageSelect = 0;
        var idGrupoSelect = 0;

        function BlinkImage() {

            if (Imagenes.length >= 1) {
                $("#img_display").css("background-image", "url(" + Imagenes[idImageSelect] + ")");
                $("#img_display").css("background-position", "center");
                $("#img_display").css("background-repeat", "no-repeat");
                $("#img_display").css("background-size", "100% auto");
                setTimeout("BlinkImage()", 6000); //1000 ms = 1 second.

                if (idImageSelect < Imagenes.length - 1) {
                    idImageSelect = idImageSelect + 1;
                } else {
                    idImageSelect = 0;
                }
            }
        }

        $("#notifica_envio").click(function() {

            var mensaje = $("#mensaje_notificacion").val();
            var urlEnviaMensaje = "/api/ApiNotificacion";

            if (mensaje == ''){
                return 0;
            }

            if(idGrupoSelect == 0){
                noty({
                    theme: 'app-noty',
                    text: 'Seleccione un grupo para enviar mensaje',
                    type: 'error',
                    timeout: 5000,
                    layout: 'topRight',
                    closeWith: ['button', 'click'],
                    animation: {
                        open: 'in',
                        close: 'out'
                    },
                });
                return 0;
            }

            $.ajax({
                cache: false,
                type: "POST",
                url: "/api/ApiNotificacion",
                data: {"mensaje":mensaje, "idGrupo":idGrupoSelect},
                success: function (data) {
                    var t = JSON.parse(data);
                    noty({
                        theme: 'app-noty',
                        text: 'Notificación enviada Exitosamente a los ' + t['recipients'] + ' promoteres activos',
                        type: 'success',
                        timeout: 5000,
                        layout: 'topRight',
                        closeWith: ['button', 'click'],
                        animation: {
                            open: 'in',
                            close: 'out'
                        },
                    });
                    $("#mensaje_notificacion").val('');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    noty({
                        theme: 'app-noty',
                        text: xhr.responseText,
                        type: 'error',
                        timeout: 5000,
                        layout: 'topRight',
                        closeWith: ['button', 'click'],
                        animation: {
                            open: 'in',
                            close: 'out'
                        },
                    });
                }
            });
        });

        $(".opcionesGrupo").click(function() {
            $("#textoGrupo").html($(this).data('item'));
            idGrupoSelect = $(this).data('idgrupo')
        });

        $(document).ready(function () {
            BlinkImage();
            jQuery.event.props.push('dataTransfer');
        });

    </script>

End Section




