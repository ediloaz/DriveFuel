<style type="text/css">
	.add-remove-user {
		cursor: pointer;
	}
	.row-item {
		border-bottom: 1px solid gray;
	}
	.userSelector {
		font-size: 25px;
	}
	.userText {
		font-size: 15px;
	}
	.users-title {
		margin-top: 15px;
	}
</style>

<script type="text/x-template" id="forma-preview-template">
<div class="row">
  <div class="col-sm-12">
    <div class="card card-block bg-white">
      <h4 class="card-title">Preguntas</h4>
      <div class="row pregunta" v-for="pregunta in forma.FormaPregunta">
        <div class="form-group">
            <div class="row">
                <div class="col-sm-7">
                    <input class="form-control" :value="pregunta.Pregunta" readonly="" disabled="" />
                </div>
                <div class="col-sm-3">
                    <label>{{ tiposForma[pregunta.Tipo] }}</label>
                </div>
            </div>
            <div class="row options" v-if="pregunta.Tipo == 6">
                <input type="file" />
            </div>
            <div class="row options" v-if="tiposFormaOptions[pregunta.Tipo]">
                <template v-if="pregunta.Tipo == 3">
                    <select class="form-control">
                        <option v-for="opcion in pregunta.FormaPreguntaOpcion">
                            {{ opcion.Opcion }}
                        </option>
                    </select>
                </template>
                <template v-else-if="pregunta.Tipo == 4">
                    <template v-for="opcion in pregunta.FormaPreguntaOpcion">
                        <input type="radio" name="previewradio" />
                        {{ opcion.Opcion }} &nbsp;&nbsp;
                    </template>
                </template>
                <template v-else-if="pregunta.Tipo == 5">
                    <template v-for="opcion in pregunta.FormaPreguntaOpcion">
                        <input type="checkbox" />
                        {{ opcion.Opcion }} &nbsp;&nbsp;
                    </template>
                </template>
            </div>
        </div>
      </div>
    </div>
  </div>
</div>
</script>

<script type="text/javascript">
	Vue.component('forma-preview', {
		template: '#forma-preview-template',
		props: ['idForma'],
		data: function() {
			return {
	            forma: {FormaPregunta: []},
	            tiposForma: ['Seleccionar Tipo', 'Texto', 'Input Númerico', 'Combo', 'Radio Button', 'Checkbox'],
	            tiposFormaOptions: [false, false, false, true, true, true]
	        };
		},
		methods: {
			getForma: function(id) {
				if(id <= 0 ) {
					this.forma = {FormaPregunta: []};
					return;
				}
				let self = this;
                fetch(`/api/ApiFormas//${id}`).then((res) => res.json().then((data) => {
                    self.forma = data;
                }));
			}
		},
		watch: {
			idForma: function(val) {
                console.log('change idforma', val);
				this.getForma(val);
			}
		}
	});

    var appPreview = new Vue({
        el: '#preview',
        data: { idForma: 0 },
        methods: {
            show(id) {
                console.log('show', id);
                this.idForma = 0;
                let self = this;
                //this.idForma = id;
                setTimeout(() => self.idForma = id, 500);
            }
        }
    });
</script>