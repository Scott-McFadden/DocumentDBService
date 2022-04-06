import React from 'react';
import {   Input } from 'reactstrap';
 
import './tooltips.css';

export default class TextAreaElement extends React.Component {

    constructor(props) {
        super(props);
        if ('showprops' in this.props)
            console.log("TextAreaElement", this.props);

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
                    <textarea
                        type="text"
                        className="form-control"
                        onChange={this.changedValue}
                        aria-describedby={this.props.name + "_label"}
                        required={this.props.required} 
                        wrap='soft' 
                        id={this.props.name + "_id"}
                        name={this.props.name}
                        rows={(rows in this.props) ? this.rows.props : "5"}
                        cols={(cols in this.props) ? this.cols.props : "50"}
                        autocapitalize='sentences'
                        autocomplete='off'
                        placeholder={this.props.placeholder ? this.props.placeholder : ""}
                    >{this.state.value}</textarea>
                   
                  </span>  
            </div> 
            );
    }
}