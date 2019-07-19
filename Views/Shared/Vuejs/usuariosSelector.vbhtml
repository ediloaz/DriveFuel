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

<script type="text/x-template" id="users-selector-template">
<div>
	<div class="row">
		<div class="form-group">
			<label class="col-sm-2 control-label">Filtrar</label>
			<div class="col-sm-10">
				<input type="text" class="form-control" v-model="searchText" />
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-sm-6">
			<div class="card card-block bg-white">
				<template v-if="!onlyUsers">
					<h4 class="card-title">Disponibles (grupos)</h4>
					<div v-for="grupo in gruposLibresShow" class="userSelector text-center">
						<div class="row row-item">
							<div class="col-sm-2">
								<i class="icon-user"></i>
							</div>
							<div class="col-sm-8 userText">
								<div>
									<strong><span>{{ grupo.Descripcion }}</span></strong>
								</div>
							</div>
							<div class="col-sm-2">
								<i class="add-remove-user icon-arrow-right" @@click="addGroup(grupo)"></i>
							</div>
						</div>
					</div>
				</template>
				<h4 class="card-title users-title" >Disponibles (usuarios)</h4>
				<div v-for="usuario in usuariosLibresShow" key={usuario.id} class="userSelector text-center">
					<div class="row row-item">
						<div class="col-sm-2">
							<i class="icon-user"></i>
						</div>
						<div class="col-sm-8 userText">
							<div>
								<span>{{ usuario.Nombre }}</span>  
							</div>
							<div>
								<span>{{ usuario.Correo }}</span>  
							</div>
						</div>
						<div class="col-sm-2">
							<i class="add-remove-user icon-arrow-right" @@click="addUser(usuario)"></i>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="row">
		<div class="col-sm-6">
			<div class="card card-block bg-white">
				<template v-if="!onlyUsers">
					<h4 class="card-title">Seleccionados (grupos)</h4>
					<div v-for="grupo in gruposSelectedShow" class="userSelector text-center">
						<div class="row row-item">
							<div class="col-sm-2">
								<i class="add-remove-user icon-arrow-left" @@click="removeGroup(grupo)"></i>
							</div>
							<div class="col-sm-2">
								<i class="icon-user"></i>
							</div>
							<div class="col-sm-8 userText">
								<div>
									<strong><span>{{ grupo.Descripcion }}</span></strong>
								</div>
							</div>
						</div>
					</div>
				</template>
				<h4 class="card-title users-title">Seleccionados (usuarios)</h4>
				<div v-for="usuario in usuariosSelectedShow" key={usuario.id} class="userSelector text-center">
					<div class="row row-item">
						<div class="col-sm-2">
							<i class="add-remove-user icon-arrow-left" @@click="removeUser(usuario)"></i>
						</div>
						<div class="col-sm-2">
							<i class="icon-user"></i>
						</div>
						<div class="col-sm-8 userText">
							<div>
								<span>{{ usuario.Nombre }}</span>  
							</div>
							<div>
								<span>{{ usuario.Correo }}</span>  
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="row text-right">
			<span class="btn btn-danger" @@click="cancel">Cancelar</span>
			&nbsp;&nbsp;&nbsp;&nbsp;
			<span class="btn btn-success" @@click="save">Guardar</span> 
		</div>
	</div>
</div>
</script>

<script type="text/javascript">
	Vue.component('users-selector', {
		template: '#users-selector-template',
		props: {
			usersPath: String,
			onlyUsers: {
				type: Boolean,
				default: false
			}
		},
		data: function() {
			return {
				searchText: '',
				usuariosLibresShow: [],
				usuariosSelectedShow: [],
				gruposLibresShow: [],
				gruposSelectedShow: [],
				usuariosLibres: [],
				usuariosSelected: [],
				gruposLibres: [],
				gruposSelected: []
			};
		},
		created: function() {
			let self = this;
			Promise.all([fetch('/api/ApiUsuarios'), fetch('/api/ApiGrupos')]).then((responses) => {
			    Promise.all([responses[0].json(), responses[1].json()]).then((datas) => {
			    	self.usuarios = datas[0];
			    	self.grupos = datas[1];
			    	self.getUsers();
			    	self.search();
			    });
			  })
		},
		watch: {
			searchText: function(val) {
				this.search(val);
			}
		},
		methods: {
		    getUsers: function() {
		    	if(this.usersPath) {
			    	let self = this;
			      	fetch(this.usersPath).then((res) => res.json().then((data) => {
				      	self.usuariosLibres = self.usuarios.filter(item => data.usuarios.indexOf(item.idUsuario) < 0);
				      	self.usuariosSelected = self.usuarios.filter(item => data.usuarios.indexOf(item.idUsuario) >= 0);
				      	if(data.grupos != undefined) {
					      	self.gruposLibres = self.grupos.filter(item => data.grupos.indexOf(item.idGrupo) < 0);
					      	self.gruposSelected = self.grupos.filter(item => data.grupos.indexOf(item.idGrupo) >= 0);
				      	}
			          	self.search('');
				        }));
		    	} else {
		    		this.usuariosLibres = this.usuarios;
			      	this.usuariosSelected = [];
			      	this.gruposLibres = this.grupos;
			      	this.gruposSelected = [];
		    	}
		    },
		    addUser: function(usuario) {
		    	this.usuariosSelected = this.usuariosSelected.concat(usuario);
		    	this.usuariosLibres = this.usuariosLibres.filter(u => u.idUsuario != usuario.idUsuario);
		    	this.search();
		    },
		    removeUser: function(usuario) {
		    	this.usuariosLibres = this.usuariosLibres.concat(usuario);
		    	this.usuariosSelected = this.usuariosSelected.filter(u => u.idUsuario != usuario.idUsuario);
		    	this.search();
		    },
		    addGroup: function(grupo) {
		    	this.gruposSelected = this.gruposSelected.concat(grupo);
		    	this.gruposLibres = this.gruposLibres.filter(u => u.idGrupo != grupo.idGrupo);
		    	// __todo__
		    	this.search();
		    },
		    removeGroup: function(grupo) {
		    	this.gruposLibres = this.gruposLibres.concat(grupo);
		    	this.gruposSelected = this.gruposSelected.filter(u => u.idGrupo != grupo.idGrupo);
		    	// __todo__
		    	this.search();
		    },
		    search: function(querySearch) {
		        if(typeof(querySearch) == 'undefined')
		            querySearch = this.searchText;
	        	querySearch = querySearch.trim();
		        if(querySearch.length > 0) {
		        	this.usuariosLibresShow = this.usuariosLibres.filter(item => {
		        		return item.Nombre.toLowerCase().indexOf(querySearch) >= 0
		                        || item.Correo.toLowerCase().indexOf(querySearch) >= 0;
		        	});
		        	this.usuariosSelectedShow = this.usuariosSelected.filter(item => {
		        		return item.Nombre.toLowerCase().indexOf(querySearch) >= 0
		                        || item.Correo.toLowerCase().indexOf(querySearch) >= 0;
		        	});
		        	this.gruposLibresShow = this.gruposLibres.filter(item => item.Descripcion.toLowerCase().indexOf(querySearch) >= 0);
		        	this.gruposSelectedShow = this.gruposSelected.filter(item => item.Descripcion.toLowerCase().indexOf(querySearch) >= 0);
		        } else {
		        	this.usuariosLibresShow = this.usuariosLibres;
		        	this.usuariosSelectedShow = this.usuariosSelected;
		        	this.gruposLibresShow = this.gruposLibres;
		        	this.gruposSelectedShow = this.gruposSelected;
		        }
		    },
		    save: function() {
		      console.log(this.usuariosSelected);
		      let usuarios = this.usuariosSelected.map((item) => item.idUsuario);
		      let grupos = this.gruposSelected.map((item) => item.idGrupo);
		      console.log('save', usuarios, grupos);
		      this.$emit('save', usuarios, grupos);
		      console.log(usuarios, grupos);
		      if(!this.usersPath) {
		      	return;
		      }
		      fetch(this.usersPath, {method: 'POST', body: JSON.stringify( {usuarios: usuarios, grupos: grupos} )}).then((res) => res.text().then((data) => {
		        noty({
		            theme: 'app-noty',
		            text: 'Usuarios asignados',
		            type: 'success',
		            timeout: 3000,
		            layout: 'bottomRight',
		            closeWith: ['button', 'click'],
		            animation: {
		                open: 'in',
		                close: 'out'
		            },
		        });
		      }));
		    },
		    cancel: function() {
		    	this.$emit('cancel');
		    	this.getUsers();
		    }
		}
	})
</script>