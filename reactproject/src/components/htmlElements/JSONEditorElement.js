import React from 'react';
import {   Input } from 'reactstrap';
import { JsonEditor as Editor } from 'jsoneditor-react';
import 'jsoneditor-react/es/editor.min.css';
import './tooltips.css';
import ace from 'brace';
import 'brace/mode/json';
import 'brace/theme/github';


export default class JSONEditorElement extends React.Component {

    constructor(props) {
        super(props);
        if ('showprops' in this.props)
            console.log("JSONEditorElement", this.props);

        this.requiredMark = this.props.required && this.props.required === 'true' ? "*" : "";
        this.changedValue = this.changedValue.bind(this); 
        this.addLabel = this.addLabel.bind(this);
        this.state = { value: "" };
    }

    loaded = false;
    newlabel = ""; 
    value = "";
    requiredMark = "";
   

    componentDidMount() {
        this.loaded = true;
        this.setState ({ value: ('value' in this.props) ? this.props.value : ""})  ;
    }

    changedValue(e) {
        this.value = e.target.value;
        this.props.onChange({ name: this.props.name, value: e.target.value });
        this.setState({ value: e.target.value })
    }

    addLabel() {
        if ("label" in this.props)
            return (
                <span
                    className="input-group-text"
                    id={this.props.name + "_label"}>
                    <span style={{ color: "red" }}>{this.requiredMark}</span>{this.props.label}
                </span>
            );
        return ("");
    }

    render() { 
        return ( 
            <div className="input-group ">
                {this.addLabel()}
                <span data-tip={ this.props.description }>
                    <Editor
                        type="text"
                        className="form-control"
                        value={this.state.value}
                        onChange={this.changedValue}
                        aria-describedby={this.props.name + "_label"}
                        required={this.props.required}
                        ace={ace}
                        id={this.props.name + "_id"}
                        name={this.props.name}
                        theme={(theme in this.props) ? this.props.theme : 'ace/theme/github'}
                        schema={(schema in this.props) ? this.props.schema : undefined   } />
                   
                  </span>  
            </div> 
            );
    }
}