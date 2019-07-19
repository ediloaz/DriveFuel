<script type="text/babel">
    var itemStyle = {
        "borderBottom": "gray 1px solid"
    };
    var cardTitle = {
        "marginTop": "15px"
    };
  class Usuario extends React.Component {
    render() {
      return <div><h1>Hello, {this.props.name}</h1> <a className="btn btn-warning">Hola</a></div>;
    }
  }
  class Usuarios extends React.Component {
    constructor(props) {
      super(props);
      console.log(props);
      this.state = {
        usuariosLibresShow: [],
        usuariosSelectedShow: [],
        gruposLibresShow: [],
        gruposSelectedShow: []
      };
      let self = this;
      Promise.all([fetch('/api/ApiUsuarios'), fetch('/api/ApiGrupos')]).then((responses) => {
        Promise.all([responses[0].json(), responses[1].json()]).then((datas) => {
            self.setState({
              usuarios: self.mapApiUsers(datas[0]),
              grupos: self.mapApiGroups(datas[1])
            });
            console.log(self.state.usuarios);
            console.log(self.state.grupos);
            if(idRuta) {
              self.getUsers();
            }
        });
      })
    }
    mapApiUsers(data) {
        return data.map((item) => ({id: parseInt(item['idUsuario']), nombre: item['Nombre'], correo: item['Correo']}));
    }
    mapApiGroups(data) {
        return data.map((item) => ({id: parseInt(item['idGrupo']), descripcion: item['Descripcion'], "usuarios": this.mapApiUsers(item['Usuarios'])}));   
    }
    getUsers() {
      fetch(`/Rutas/Usuarios/${idRuta}`).then((res) => res.json().then((data) => {
          this.setState({
            usuariosLibres: this.state.usuarios.filter((item) => data.usuarios.indexOf(item.id) < 0),
            usuariosSelected: this.state.usuarios.filter((item) => data.usuarios.indexOf(item.id) >= 0),
            gruposLibres: this.state.grupos.filter((item) => data.grupos.indexOf(item.id) < 0),
            gruposSelected: this.state.grupos.filter((item) => data.grupos.indexOf(item.id) >= 0)
          });
          this.setState({
            querySearch: '',
          });
          this.search('');
        }));
    }
    // Disculpa mi spanglish :( jaja
    addUser(usuario) {
      this.setState({
        usuariosSelected: this.state.usuariosSelected.concat(usuario),
        usuariosLibres: this.state.usuariosLibres.filter((u) => u.id != usuario.id)
      });
      let self = this;
      setTimeout(() => {self.search()}, 150);
    }
    removeUser(usuario) {
      this.setState({
        usuariosLibres: this.state.usuariosLibres.concat(usuario),
        usuariosSelected: this.state.usuariosSelected.filter((u) => u.id != usuario.id)
      });
      let self = this;
      setTimeout(() => {self.search()}, 150);
    }
    addGroup(grupo) {
      this.setState({
        gruposSelected: this.state.gruposSelected.concat(grupo),
        gruposLibres: this.state.gruposLibres.filter((u) => u.id != grupo.id)
      });
      let self = this;
      setTimeout(() => {self.search()}, 150);
    }
    removeGroup(grupo) {
      this.setState({
        gruposLibres: this.state.gruposLibres.concat(grupo),
        gruposSelected: this.state.gruposSelected.filter((u) => u.id != grupo.id)
      });
      let self = this;
      setTimeout(() => {self.search()}, 150);
    }
    search(querySearch) {
        if(typeof(querySearch) == 'undefined')
            querySearch = this.state.querySearch;
        if(querySearch.length > 0) {
            this.setState({
                usuariosLibresShow: this.state.usuariosLibres.filter((item) => {
                    return item.nombre.toLowerCase().indexOf(querySearch) >= 0
                        || item.correo.toLowerCase().indexOf(querySearch) >= 0;
                }),
                usuariosSelectedShow: this.state.usuariosSelected.filter((item) => {
                    return item.nombre.toLowerCase().indexOf(querySearch) >= 0
                        || item.correo.toLowerCase().indexOf(querySearch) >= 0;
                }),
                gruposLibresShow: this.state.gruposLibres.filter((item) => item.descripcion.toLowerCase().indexOf(querySearch) >= 0),
                gruposSelectedShow: this.state.gruposSelected.filter((item) => item.descripcion.toLowerCase().indexOf(querySearch) >= 0)
            });
        } else {
            this.setState({
                usuariosLibresShow: this.state.usuariosLibres,
                usuariosSelectedShow: this.state.usuariosSelected,
                gruposLibresShow: this.state.gruposLibres,
                gruposSelectedShow: this.state.gruposSelected
            });
        }
    }
    handleChange(e) {
        this.setState({ querySearch: e.target.value.toLowerCase() });
        this.search(e.target.value.toLowerCase());
      }
    save() {
      console.log(this.state.usuariosSelected);
      let usuarios = this.state.usuariosSelected.map((item) => item.id);
      let grupos = this.state.gruposSelected.map((item) => item.id);
      console.log(usuarios, grupos);
      fetch(`/Rutas/Usuarios/${idRuta}`, {method: 'POST', body: JSON.stringify( {usuarios: usuarios, grupos: grupos} )}).then((res) => res.text().then((data) => {
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
    }
    render() {
      return <div>
        <div className="row">
          <div className="form-group">
            <label className="col-sm-2 control-label">Filtrar</label>
            <div className="col-sm-10">
              <input type="text" className="form-control" onChange={(e) => this.handleChange(e)} />
            </div>
          </div>
      </div>
      <br/>
      <div className="row">
          <div className="col-sm-6">
            <div className="card card-block bg-white">
              <h4 className="card-title">Disponibles (grupos)</h4>
              {this.state.gruposLibresShow.map((grupo) => <div key={grupo.id} className="userSelector text-center">
                <div className="row" style={itemStyle}>
                  <div className="col-sm-2">
                    <i className="icon-users"></i>
                  </div>
                  <div className="col-sm-8 userText">
                    <div>
                      <strong><span>{grupo.descripcion}</span>  </strong>
                    </div>
                  </div>
                  <div className="col-sm-2">
                    <i className="add-remove-user icon-arrow-right" onClick={(e) => this.addGroup(grupo)}></i>
                  </div>
                </div>
              </div>)}
              {/*
              <h4 className="card-title" style={cardTitle}>Disponibles (usuarios)</h4>
              {this.state.usuariosLibresShow.map((usuario) => <div key={usuario.id} className="userSelector text-center">
                <div className="row" style={itemStyle}>
                  <div className="col-sm-2">
                    <i className="icon-user"></i>
                  </div>
                  <div className="col-sm-8 userText">
                    <div>
                      <span>{usuario.nombre}</span>  
                    </div>
                    <div>
                      <span>{usuario.correo}</span>  
                    </div>
                  </div>
                  <div className="col-sm-2">
                    <i className="add-remove-user icon-arrow-right" onClick={(e) => this.addUser(usuario)}></i>
                  </div>
                </div>
              </div>)}
            */}
            </div>
          </div>
          <div className="col-sm-6">
            <div className="card card-block bg-white">
              <h4 className="card-title">Seleccionados (grupos)</h4>
              {this.state.gruposSelectedShow.map((grupo) => <div key={grupo.id} className="userSelector text-center">
                <div className="row" style={itemStyle}>
                  <div className="col-sm-2">
                    <i className="add-remove-user icon-arrow-right" onClick={(e) => this.removeGroup(grupo)}></i>
                  </div>
                  <div className="col-sm-2">
                    <i className="icon-users"></i>
                  </div>
                  <div className="col-sm-8 userText">
                    <div>
                      <strong><span>{grupo.descripcion}</span>  </strong>
                    </div>
                  </div>
                </div>
              </div>)}
              {/*
              <h4 className="card-title">Seleccionados (usuarios)</h4>
              {this.state.usuariosSelectedShow.map((usuario) => <div key={usuario.id} className="userSelector text-center">
                <div className="row" style={itemStyle}>
                  <div className="col-sm-2">
                    <i className="add-remove-user icon-arrow-left" onClick={(e) => this.removeUser(usuario)}></i>
                  </div>
                  <div className="col-sm-2">
                    <i className="icon-user"></i>
                  </div>
                  <div className="col-sm-8 userText">
                    <div>
                      <span>{usuario.nombre}</span>  
                    </div>
                    <div>
                      <span>{usuario.correo}</span>  
                    </div>
                  </div>
                </div>
              </div>)}
            */}
            </div>
          </div>
        </div>
        <div className="row text-right"><span className="btn btn-success" onClick={(e) => this.save()}>Guardar</span></div>
      </div>;
    }
  }

  usuariosApp = ReactDOM.render(
    <Usuarios idRuta={idRuta}/>,
    document.getElementById('usuarios')
  );
</script>