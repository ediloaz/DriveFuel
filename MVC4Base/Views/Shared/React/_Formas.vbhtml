<script type="text/babel">
    var itemStyle = {
        "borderBottom": "gray 1px solid"
    };
    var cardTitle = {
        "marginTop": "15px"
    };
  class Forma extends React.Component {
    render() {
      return <div><h1>Hello, {this.props.name}</h1> <a className="btn btn-warning">Hola</a></div>;
    }
  }
  class Formas extends React.Component {
    constructor(props) {
      super(props);
      console.log(props);
      this.state = {
        formasLibresShow: [],
        formasSelectedShow: []
      };
      let self = this;
      fetch('/api/ApiFormas').then((response) => response.json().then((data) => {
            self.setState({
              formas: self.mapApiFormas(data)
            });
            console.log(self.state.formas);
            if(idRuta) {
              self.getFormas();
            }
      }));
    }
    mapApiFormas(data) {
      console.log(data);
        return data.map((item) => ({id: parseInt(item['IdForma']), name: item['Descripcion']}));
    }
    getFormas() {
      fetch(`/Rutas/Formas/${idRuta}`).then((res) => res.json().then((data) => {
          this.setState({
            formasLibres: this.state.formas.filter((item) => data.indexOf(item.id) < 0),
            formasSelected: this.state.formas.filter((item) => data.indexOf(item.id) >= 0),
          });
          this.setState({
            querySearch: '',
          });
          this.search('');
        }));
    }
    addForma(usuario) {
      this.setState({
        formasSelected: this.state.formasSelected.concat(usuario),
        formasLibres: this.state.formasLibres.filter((u) => u.id != usuario.id)
      });
      let self = this;
      setTimeout(() => {self.search()}, 150);
    }
    removeForma(usuario) {
      this.setState({
        formasLibres: this.state.formasLibres.concat(usuario),
        formasSelected: this.state.formasSelected.filter((u) => u.id != usuario.id)
      });
      let self = this;
      setTimeout(() => {self.search()}, 150);
    }
    showPreview(item) {
       appPreview.show(item.id);
    }
    search(querySearch) {
        if(typeof(querySearch) == 'undefined')
            querySearch = this.state.querySearch;
        if(querySearch.length > 0) {
            this.setState({
                formasLibresShow: this.state.formasLibres.filter((item) => item.name.toLowerCase().indexOf(querySearch) >= 0),
                formasSelectedShow: this.state.formasSelected.filter((item) => item.name.toLowerCase().indexOf(querySearch) >= 0)
            });
        } else {
            this.setState({
                formasLibresShow: this.state.formasLibres,
                formasSelectedShow: this.state.formasSelected
            });
        }
    }
    handleChange(e) {
        this.setState({ querySearch: e.target.value.toLowerCase() });
        this.search(e.target.value.toLowerCase());
      }
    save() {
      console.log(this.state.formasSelected);
      let ids = this.state.formasSelected.map((item) => item.id);
      console.log(ids);
      fetch(`/Rutas/Formas/${idRuta}`, {method: 'POST', body: JSON.stringify( ids )}).then((res) => res.text().then((data) => {
        noty({
            theme: 'app-noty',
            text: 'Formas asignadas',
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
              <h4 className="card-title">Disponibles (formas)</h4>
              {this.state.formasLibresShow.map((item) => <div key={item.id} className="userSelector text-center">
                <div className="row" style={itemStyle}>
                  <div className="col-sm-2">
                    <i className="icon-notebook" onClick={e => this.showPreview(item)}></i>
                  </div>
                  <div className="col-sm-8 userText">
                    <div>
                      <strong><span>{item.name}</span>  </strong>
                    </div>
                  </div>
                  <div className="col-sm-2">
                    <i className="add-remove-user icon-arrow-right" onClick={(e) => this.addForma(item)}></i>
                  </div>
                </div>
              </div>)}
            </div>
          </div>
          <div className="col-sm-6">
            <div className="card card-block bg-white">
              <h4 className="card-title">Seleccionados (formas)</h4>
              {this.state.formasSelectedShow.map((item) => <div key={item.id} className="userSelector text-center">
                <div className="row" style={itemStyle}>
                  <div className="col-sm-2">
                    <i className="add-remove-user icon-arrow-left" onClick={(e) => this.removeForma(item)}></i>
                  </div>
                  <div className="col-sm-2">
                    <i className="icon-notebook" onClick={e => this.showPreview(item)}></i>
                  </div>
                  <div className="col-sm-8 userText">
                    <div>
                      <strong><span>{item.name}</span>  </strong>
                    </div>
                  </div>
                </div>
              </div>)}
            </div>
          </div>
        </div>
        <div className="row text-right"><span className="btn btn-success" onClick={(e) => this.save()}>Guardar</span></div>
      </div>;
    }
  }

  formasApp = ReactDOM.render(
    <Formas idRuta={idRuta}/>,
    document.getElementById('formas')
  );
</script>