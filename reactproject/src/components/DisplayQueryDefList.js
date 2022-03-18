import React, { Component } from 'react';


export default class DisplayQueryDefList extends Component {

    constructor(props) {
        super(props);

        this.onQueryDefChange = this.onQueryDefChange.bind(this);
        this.renderTable = this.renderTable.bind(this);
        this.state = {
            queryDefList: [],
            loading: true,
            currentEntry: { name: "" },
            fieldList: []
        }
    }

    onQueryDefChange(e) {
        console.log(e);
        var rowstate = [];
        for (var i in this.state.queryDefList) {
            rowstate[i.id] = "";
        }
        rowstate[e.id] = "table-active";

        this.setState({ currentEntry: e, rowStateClass: rowstate });
        this.props.onQueryDefChange(e);
        this.populateFields(e.name);
    }
    renderTable() {


        return (
            <div className="container">
               
                <div className="row align-items-start " >
            <div className='col-7 border'>
            <table className='table  table-hover' aria-labelledby='tableLable'>
                <thead>
                    <tr>
                        <th scope="col">Query Def Name</th>
                        <th scope="col">Description </th>

                    </tr>
                </thead>
                <tbody>
                    {this.state.queryDefList.map(entry =>
                        <tr
                            key={entry.id}
                            onClick={() => { this.onQueryDefChange(entry) }}
                            className={this.state.rowStateClass[entry.id]}
                            >
                            <td > {entry.name}</td>
                        <td>{entry.description} </td>
                        </tr>
                        )} 
                </tbody>
                    </table></div>
                    <div className='col-4  border' style={{ marginLeft: '10px',paddingTop:'10px', paddingLeft: '10px' }}>
                        <h5>Selected:   {this.state.currentEntry.name}</h5>
                        <h5>Fields</h5>
                        <ol>
                            {this.state.fieldList.map(entry => (<li>{entry}</li>))}
                        </ol>
                    </div>
                    </div>

            </div>
            );
    }
    componentDidMount() {
        this.populateQueryDefs();
    }

    async populateFields(qdef) {
        if (qdef === undefined)
            return;
        const response = await fetch(`/api/ControlData/ControlData/${qdef}/GetFields`, { mode: 'cors' });
        const data = await response.json();
        this.setState({ fieldList: data });
    }
    async populateQueryDefs() {
        const response = await fetch('/api/QueryDef', { mode: 'cors' });
        const data = await response.json();
        var rowstate = [];
        for (var i in data) {
            rowstate[i.id] = "";
        }
        this.setState({ queryDefList: data, loading: false, rowStateClass: rowstate});

    }
    render() {

        let contents = this.state.loading ?
            <span> <em> Loading Available Query Definitions </em></span>
            :
            this.renderTable();

        return (
            <div id='displayQueryDefList' className="container-lg border border-primary">
                {contents}
            </div>


        );
    }
}