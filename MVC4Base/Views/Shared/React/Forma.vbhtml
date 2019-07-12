<style>
	.pointer {
		cursor: pointer;
	}
	.pregunta {
		margin: 10px
	}
	.save-question, .remove-question {
		cursor: pointer;
		font-size: 1.5em;
	}
	.row.options {
		padding: 15px;
	}
	.option-container {
		display: inline-block;
		padding: 5px;
	}
	.options-container {
		padding-top: 10px;
		text-align: center;
	}
</style>

<script type="text/babel">
	var FormaApp;
	var tiposForma = ['Seleccionar Tipo', 'Texto', 'Input Númerico', 'Combo', 'Radio Button', 'Checkbox', 'Imagen'];
	var tiposFormaOptions = [false, false, false, true, true, true, false];
	var countOpciones = -999;

  class Opcion extends React.Component {
  	handleChange(e) {
  		FormaApp.updateOpcion({
  			id: this.props.opcion.idFormaPreguntaOpcion,
  			idFormaPregunta: this.props.idFormaPregunta,
  			texto: e.target.value
  		});
  	}
  	removeOption(e) {
		FormaApp.removeOpcion({
  			id: this.props.opcion.idFormaPreguntaOpcion,
  			idFormaPregunta: this.props.idFormaPregunta
  		});
  	}
    render() {
      return <div className="option-container">
      			<div className="col-sm-11">
		      		<input className="form-control" value={this.props.opcion.Opcion} onChange={(e) => this.handleChange(e)} />
	      		</div>
      			<div className="col-sm-1 options-container">
		      		<i className="remove-option icon-trash pointer" onClick={(e) => this.removeOption(e)}></i>
	      		</div>
      </div>;
    }
  }

  class Pregunta extends React.Component {
  	handleChange(e) {
  		FormaApp.editPregunta({
  			id: this.props.pregunta.idFormaPregunta,
  			texto: e.target.value
  		});
  	}
  	handleBlur(e) {
  		FormaApp.updatePregunta({
  			id: this.props.pregunta.idFormaPregunta,
  		});
  	}
    render() {
      return <div className="row pregunta">
      	<div className="form-group">
      		<div className="row">
	      		<div className="col-sm-7">
		      		<input className="form-control" value={this.props.pregunta.Pregunta} onChange={(e) => this.handleChange(e)} onBlur={(e) => this.handleBlur(e)} />
		      	</div>
		      	<div className="col-sm-3">
		      		<label>{tiposForma[this.props.pregunta.Tipo]}</label>
		      	</div>
		      	<div className="col-sm-2">
					<i className="save-question fa fa-save" onClick={(e) => FormaApp.savePregunta(this.props.pregunta.idFormaPregunta)}></i>
					&nbsp;&nbsp;&nbsp;
					<i className="remove-question icon-trash" onClick={(e) => FormaApp.removePregunta(this.props.pregunta.idFormaPregunta)}></i>
				</div>
      		</div>
      		{tiposFormaOptions[this.props.pregunta.Tipo] && 
      		<div className="row options">
      			<div className="col-sm-1 options-container">
		      		<i className="add-option icon-plus pointer" onClick={(e) => FormaApp.nuevaOpcion(this.props.pregunta.idFormaPregunta)}></i>
      			</div>
      			<div className="col-sm-10">
      				{this.props.pregunta.FormaPreguntaOpcion.filter((p) => p.Opcion != '__delete__').map((p, i) => <Opcion key={i} opcion={p} idFormaPregunta={this.props.pregunta.idFormaPregunta}/>)}
      			</div>
      		</div>
	      	}
      	</div>
      </div>;
    }
  }

  class Forma extends React.Component {
    constructor(props) {
      super(props);
      this.state = {
        preguntas: [],
        cont: 0,
        nuevaPregunta: '',
        nuevaPreguntaTipo: 0
      };
      this.getPreguntas();
    }
    getPreguntas() {
      fetch(`/api/ApiFormas//${idForma}`).then((res) => res.json().then((data) => {
          this.setState({
            preguntas: data.FormaPregunta
          });
        }));
      setTimeout(() => appPreview.show(idForma), 1500);
    }
    handleChange(e) {
        this.setState({ querySearch: e.target.value.toLowerCase() });
        this.search(e.target.value.toLowerCase());
      }
  	editPregunta(data) {
  		this.setState({
  			preguntas: this.state.preguntas.map((p) => {
  				if(data.id == p.idFormaPregunta) {
  					p.Pregunta = data.texto;
  					return p;
  				} else {
  					return p;
  				}
  			})
  		});
  	}
  	updateOpcion(data) {
  		this.setState({
  			preguntas: this.state.preguntas.map((p) => {
  				if(data.idFormaPregunta == p.idFormaPregunta) {
  					console.log(p);
  					p.FormaPreguntaOpcion.forEach((o) => {
  						console.log(o);
  						if(data.id == o.idFormaPreguntaOpcion) {
  							o.Opcion = data.texto;
  						}
  					});
  					return p;
  				} else {
  					return p;
  				}
  			})
  		});	
  	}
  	removeOpcion(data) {
  		console.log(data);
  		this.setState({
  			preguntas: this.state.preguntas.map((p) => {
  				if(data.idFormaPregunta == p.idFormaPregunta) {
  				console.log(p);
  					p.FormaPreguntaOpcion.forEach((o) => {
  						if(data.id == o.idFormaPreguntaOpcion) {
  						console.log(o);
  							//o.idFormaPreguntaOpcion *= -1;
  							o.Opcion = '__delete__';
  						}
  					});
  					return p;
  				} else {
  					return p;
  				}
  			})
  		});	
  	}
  	savePregunta(idFormaPregunta) {
  		let data = this.state.preguntas.filter((p) => idFormaPregunta == p.idFormaPregunta)[0];
  		let myHeaders = new Headers();
		myHeaders.append('Content-Type', 'application/json');

  		fetch(`/api/ApiFormaPreguntas/${idFormaPregunta}`, {method: 'PUT', headers: myHeaders, body: JSON.stringify(data)}).then((res) => res.json().then((data) => {
  			noty({
	            theme: 'app-noty',
	            text: 'Pregunta guardada',
	            type: 'success',
	            timeout: 3000,
	            layout: 'bottomRight',
	            closeWith: ['button', 'click'],
	            animation: {
	                open: 'in',
	                close: 'out'
	            },
	        });
  			this.setState({
	  			preguntas: this.state.preguntas.map((p) => {
	  				if(data.idFormaPregunta == p.idFormaPregunta) {
		  				console.log(p);
		  				console.log(data);
	  					return data;
	  				} else {
	  					return p;
	  				}
	  			})
	  		});
        appPreview.show(idForma);
		}));
  	}
  	removePregunta(idFormaPregunta) {
  		fetch(`/api/ApiFormaPreguntas/${idFormaPregunta}`, {method: 'DELETE'}).then((res) => res.json().then((data) => {
  			noty({
	            theme: 'app-noty',
	            text: 'Pregunta eliminada',
	            type: 'success',
	            timeout: 3000,
	            layout: 'bottomRight',
	            closeWith: ['button', 'click'],
	            animation: {
	                open: 'in',
	                close: 'out'
	            },
	        });
  			this.setState({
	  			preguntas: this.state.preguntas.filter((p) => data.idFormaPregunta != p.idFormaPregunta)
	  		});
		}));
  	}
  	nuevaOpcion(idFormaPregunta) {
  		countOpciones += 1;
  		this.setState({
  			preguntas: this.state.preguntas.map((p) => {
  				if(idFormaPregunta == p.idFormaPregunta) {
  					p.FormaPreguntaOpcion.push({
  						idFormaPreguntaOpcion: countOpciones,
  						idFormaPregunta: idFormaPregunta,
  						Opcion: ''
  					});
  					return p;
  				} else {
  					return p;
  				}
  			})
  		});
  	}
  	nuevaPregunta() {
  		console.log(this.state.nuevaPregunta, this.state.nuevaPreguntaTipo);
  		if(this.state.nuevaPregunta.trim() == '') {
  			alert('Se requiere un texto');
  			return;
  		}
  		if(this.state.nuevaPreguntaTipo == 0) {
  			alert('Se requiere un tipo');
  			return;
  		}
  		let data = {
  			idForma: idForma,
  			Pregunta: this.state.nuevaPregunta.trim(),
  			Tipo: this.state.nuevaPreguntaTipo,
  			Orden: this.state.preguntas.length + 1
  		};
  		let myHeaders = new Headers();
		myHeaders.append('Content-Type', 'application/json');
  		fetch('/api/ApiFormaPreguntas', {method: 'POST', headers: myHeaders, body: JSON.stringify(data)}).then((res) => res.json().then((data) => {
  			this.setState({
  				preguntas: this.state.preguntas.concat(data)
  			});
	        noty({
	            theme: 'app-noty',
	            text: 'Pregunta agregada',
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
            <label className="col-sm-2 control-label">Nueva pregunta</label>
            <div className="col-sm-9">
            	<div className="input-group m-b">
        			<div className="col-sm-9">
	              		<input type="text" className="form-control" onChange={(e) => this.setState({'nuevaPregunta': e.target.value})} />
	              	</div>
	              	<div className="col-sm-3">
		              	<select className="form-control col-sm-4" onChange={(e) => this.setState({'nuevaPreguntaTipo': e.target.value})}>
		              	{tiposForma.map((t, i) => <option key={i} value={i}>{t}</option>)}
						</select>
		              </div>
	              <span className="input-group-btn">
	              	<input className="btn btn-success" type="submit" value="Agregar" onClick={(e) => this.nuevaPregunta()} />
	              </span>
            	</div>
            </div>
          </div>
      	</div>
      <br/>
      <div className="row">
          <div className="col-sm-12">
            <div className="card card-block bg-white">
              <h4 className="card-title">Preguntas</h4>
              {this.state.preguntas.map((pregunta) => <Pregunta key={pregunta.idFormaPregunta} pregunta={pregunta} />)}
            </div>
          </div>
        </div>
      </div>;
    }
  }

  FormaApp = ReactDOM.render(
    <Forma idForma={idForma}/>,
    document.getElementById('forma')
  );
</script>