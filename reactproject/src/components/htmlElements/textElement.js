import React from 'react';
import {   Input } from 'reactstrap';
 
import './tooltips.css';

export default class TextElement extends React.Component {

    constructor(props) {
        super(props);
        
        this.requiredMark = this.props.required && this.props.required === 'true' ? "*" : "";
        this.changedValue = this.changedValue.bind(this);
        this.renderDataList = this.renderDataList.bind(this);
        this.addLabel = this.addLabel.bind(this);
        this.state = { value: "" };
    }
    loaded = false;
    newlabel = ""; 
    value = "";
    requiredMark = "";
    datalist = "";
    datalistName = "dl_" + this.props.name;

    componentDidMount() {
        this.loaded = true;
        this.setState ({ value: ('value' in this.props) ? this.props.value : ""})  ;
    }

    changedValue(e) {
        this.value = e.target.value;
        this.props.onChange(this.props.name, e.target.value);
        this.setState({ value: e.target.value })
    }

    renderDataList() {
        if (this.props.datalist)
            return (
                <datalist id={this.datalistName}>
                    {this.props.datalist.map((i,item) => {
                        return (<option key={i} value= {item}  />);
                    }
                    )}
                </datalist> 
                ); 
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
                    <Input
                        type="text"
                        className="form-control"
                        onChange={this.changedValue}
                        aria-describedby={this.props.name + "_label"}
                        required={this.props.required} 
                        value={this.state.value}
                        list={this.datalistName}
                        id={this.props.name + "_id"}
                        placeholder={this.props.placeholder ? this.props.placeholder : ""}
                    />
                    {this.renderDataList()}
                  </span>  
            </div> 
            );
    }
}