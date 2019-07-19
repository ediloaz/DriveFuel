@Code
    ViewData("Title") = "Mensajes"
End Code
<style>
.contacto{
	cursor: pointer;
}
.del-message {
	cursor: pointer;
	padding-left: 10px;
}
.seleccionado{
	background: #9ab93df2;
	cursor:pointer;
}
.seleccionado .text-muted{
	color:white;
}
.iconoSeleccionado{
	color:white;
	font-size: 73px;
}
.iconoContacto{
	font-size: 73px;
}
.chat{
	display: flex;
    flex-direction: column;
    justify-content: flex-end;
}
.MensajeRecibido{
	background: #caeab2;
    border-radius: 60px;
    border-bottom-left-radius: 0px;
    color:black;
    text-align: justify;
}
.MensajeEnviado{
	background: #caeab2;
    border-radius: 60px;
    border-bottom-right-radius: 0px;
    color:black;
    text-align: justify;
}
.btnEnviar{
	padding-bottom: 4px;
    padding-top: 4px;
    padding-right: 10px;
    padding-left: 8px;
    font-size: 20px;
}
.barraEnviar{
	height: 7vh;
	margin-left: 20px;
	margin-top:20px;
	background:#d8d6d6;
}
</style>
<div id="chat">
	<div class="input-group m-b">
	    <span class="input-group-addon" style="background: #76990f; color:white"><i class="fa fa-search"></i></span>
	    <input type="text" class="form-control" placeholder="Username" v-model="searchText">
	    <span class="input-group-addon btn btn-success" @@click="newGroup()"><i class="fa fa-plus-circle"></i></span>
	</div>
	<div v-if="creatingGroup" class="col-md-12" style="height: 100vh">
		<users-selector @@save="createGroupChat" @@cancel="cancelGroup"></users-selector>
	</div>
	<div v-else class="col-md-12" style="height: 100vh"><!--CHAT-->
		<div class="col-md-3" style="height: 100vh"> <!--CONTACTOS-->
	    	<div v-for="conversation in listConversations" @@click="showChat(conversation)" class="col-md-12 card-block bg-white no-border" :class="conversation.selected ? 'seleccionado' : 'contacto'">
	    		<div class="col-md-4 text-center">
	    			<i :class="[conversation.selected ? 'iconoSeleccionado' : 'iconoContacto', conversation.idConversacion ? conversation.Grupal ? 'icon-users' : 'icon-user' : 'icon-user-follow']"></i>
	    		</div>
	    		<div class="col-md-8" style="padding-left: 20px">
					<h5 class="card-text"><b>{{ conversation.Nombre || conversation.Descripcion }}</b></h5>
					<template v-if="conversation.idConversacion">
		    			<small class="text-muted bold">{{ conversation.UltimoFecha }}</small><br>
		    			<small class="text-muted bold">{{ conversation.UltimoMensaje }}</small>
					</template>
	    		</div>
	    	</div>
		</div>
		<div class="col-md-9" style="background: white"><!--MENSAJES-->
			<div class="row" style="padding-top: 10px;background: #f9f7f7;">	
				<h6 class="card-title" style="text-align: center;"><b>{{ conversationActual.Nombre }}</b></h6>
			</div>
			<div class="col-md-11 chat"><!--pantalla-->
				<div class="row" v-for="message in messages"> <!--Mensaje Recibido-->
					<div class="col-md-4" :class="message.idUsuario == idUsuario ? 'MensajeEnviado col-md-offset-8' : 'MensajeRecibido col-md-offset-1'">
						<div class="card-block">
							<p v-if="conversationActual.Grupal && message.idUsuario != idUsuario"><small>{{ message.Usuarios.Nombre }}</small></p>
			                <p class="card-text">{{ message.Mensaje1 }}</p>
			                <p style="text-align: right;"><small>{{ message.FechaCreacion }}</small><i v-if="message.idUsuario == idUsuario" @@click="del(message)" class="fa fa-trash del-message"></i></p>
			            </div>
					</div>
				</div>
			</div>
			<div class="col-md-11 barraEnviar"> <!--Barra enviar-->
	    		<div class="input-group" style="margin-top: 10px">
					<input type="text" class="form-control br0" v-model="text" @@keyup="enter" />
	                <span class="input-group-btn">
	                		<button class="btn btn-success btnEnviar" type="button" @@click="enviar"><i class="icon-paper-plane"></i></button>
	                </span>
	             </div>
			</div>
		</div>
	</div>
</div>

@Section Scripts
@Scripts.Render("~/bundles/noty")
<script src="https://cdn.jsdelivr.net/npm/vue"></script>

@Html.Partial("Vuejs/usuariosSelector")

<script type="text/javascript">
	var url = '/api/ApiConversaciones/';
	var urlMensajes = '/api/ApiMensajes/';
	var appChat = new Vue({
		el: '#chat',
		data: {
			idUsuario: @ViewBag.idUsuario,
			conversations: [],
			conversationActual: {},
			messages: [],
			idConversation: 0,
			text: '',
			usuarios: [],
			grupos: [],
			searchText: '',
			creatingGroup: false
			//listConversations: []
		},
		created: function() {
			let self = this;
			fetch(url, {credentials: 'same-origin'}).then(r => r.json().then(data => {
				self.conversations = data.conversations;
			}));
			Promise.all([fetch('/api/ApiUsuarios'), fetch('/api/ApiGrupos')]).then((responses) => {
	        Promise.all([responses[0].json(), responses[1].json()]).then((datas) => {
	        	self.usuarios = datas[0];
	        	//self.grupos = datas[1];
	        	console.log(self.usuarios, self.grupos);
	        });
	      })
		},
		computed: {
			listConversations: function() {
				let searchText = this.searchText.trim().toLowerCase();
				let conversations = this.conversations;
				let usuarios = this.usuarios;
				let grupos = this.grupos;
				console.log(searchText);
				if(searchText.length == 0) {
					return conversations;
				} else {
					let self = this;
					let conversations = this.conversations.filter(c => {
						return (c.Nombre || '').toLowerCase().indexOf(searchText) >= 0;
					});
					conversations = conversations.concat(this.usuarios.filter(u => {
						return (self.conversations.filter(c => c.idUsuarioInvitado == u.idUsuario).length == 0 && 
								u.idUsuario != self.idUsuario) &&
							( u.Nombre.toLowerCase().indexOf(searchText) >= 0 || 
							u.Correo.toLowerCase().indexOf(searchText) >= 0 );
					})).concat(this.grupos.filter(u => {
						return u.Descripcion.toLowerCase().indexOf(searchText) >= 0;
					}));
					return conversations;
				}
			}
		},
		methods: {
			showChat(conversation) {
				console.log(conversation);
				console.log(this.conversationActual);
				if(this.conversationActual) {
					this.$set(this.conversationActual, 'selected', false);
				}
				this.conversationActual = conversation;
				this.$set(this.conversationActual, 'selected', true);
				this.idConversation = conversation.idConversacion;
				let self = this;
				fetch(url + conversation.idConversacion, {credentials: 'same-origin'})
					.then(r => r.json().then(data => {
						console.log(data);
						self.messages = data.Mensaje;
						self.conversationActual.Mensaje = data.Mensaje;
					}));
			},
			enter: function(e) {
				console.log(e);
				if(e.key == 'Enter') {
					this.enviar();
				}
			},
			enviar: function() {
				if(this.text.trim().length == 0) {
					return;
				}
				console.log(this.idConversation, this.text);
				if(!this.idConversation) {
					this.newChat(this.text, this.conversationActual.idUsuario, this.idGrupo);
					return;
				}
				let data = JSON.stringify({idConversacion: this.idConversation, Mensaje1: this.text});
				let myHeaders = new Headers();
				myHeaders.append('Content-Type', 'application/json');
				let options = {credentials: 'same-origin', method: 'POST', body: data, headers: myHeaders};
				let self = this;
				fetch(urlMensajes, options).then(r => r.json().then(data => {
					console.log(data);
					self.conversationActual.Mensaje.push(data);
					self.text = '';
					self.searchText = '';
				}));
			},
			newChat: function(text, idUsuarioInvitado, idGrupo) {
				let data;
				if(idGrupo) {
					data = JSON.stringify({idGrupo: idGrupo});
				} else {
					data = JSON.stringify({
						idUsuarioInvitado: idUsuarioInvitado,
						/*Usuarios: [],
						Grupos: []*/
					});
				}
				this._newChat(data);
			},
			_newChat: function(data) {
				let myHeaders = new Headers();
				myHeaders.append('Content-Type', 'application/json');
				let options = {credentials: 'same-origin', method: 'POST', body: data, headers: myHeaders};
				let self = this;
				fetch(url, options).then(r => r.json().then(data => {
					self.idConversation = data.idConversacion;
					self.conversations.push(data);
					self.conversationActual = data;
					self.searchText = '';
					if(!data.Grupal) {
						self.enviar();
					} else {
						noty({
				            theme: 'app-noty',
				            text: 'Grupo creado',
				            type: 'success',
				            timeout: 3000,
				            layout: 'bottomRight',
				            closeWith: ['button', 'click'],
				            animation: {
				                open: 'in',
				                close: 'out'
				            },
				        });
					}
					self.showChat(self.conversationActual);
					self.creatingGroup = false;
				}));
			},
			newGroup: function() {
				console.log('new grupo');
				if(this.searchText.trim().length == 0) {
					return;
				}
				this.creatingGroup = true;
			},
			cancelGroup: function() {
				console.log('cancel grupo');
				this.creatingGroup = false;
			},
			createGroupChat: function(usuarios, grupos) {
				let name = this.searchText.trim();
				console.log('createGroupChat', name, usuarios, grupos);
				data = JSON.stringify({
					Nombre: name,
					Usuarios: usuarios,
					Grupos: grupos
				});
				this._newChat(data);
			},
			del: function(message) {
				console.log(message);
				if(confirm("¿Estas seguro de borrar este mensaje?")) {
					//let data = JSON.stringify({idConversacion: this.idConversation});
					let myHeaders = new Headers();
					myHeaders.append('Content-Type', 'application/json');
					let options = {credentials: 'same-origin', method: 'DELETE', headers: myHeaders};
					let self = this;
					fetch(urlMensajes + '/' + message.idMensaje, options).then(r => r.json().then(data => {
						self.$set(self.conversationActual, 'Mensaje', self.conversationActual.Mensaje.filter(m => m.idMensaje != message.idMensaje));
						self.messages = self.messages.filter(m => m.idMensaje != message.idMensaje);
						noty({
				            theme: 'app-noty',
				            text: 'Mensaje borrado',
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
				}
			}
		}
	});
</script>
End Section