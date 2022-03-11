import React from 'react';
import {   Input } from 'reactstrap';
 
import './tooltips.css';

export default class TextElement extends React.Component {

    constructor(props) {
        super(props);
        
        this.requiredMark = this.props.required && this.props.required === 'true' ? "*" : "";
        this.changedValue = this.changedValue.bind(this);
        this.renderDataList = this.renderDataList.bind(this);
          
    }
    loaded = false;
    newlabel = ""; 
    value = "";
    requiredMark = "";
    datalist = "";
    datalistName = "dl_" + this.props.name;

    componentDidMount() {
        this.loaded = true;
        this.value = this.props.value;
    }

    changedValue(e) {
        this.value = e.target.value;
        this.props.onChange(this.props.name, this.value);
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
    render() { 
        return ( 
            <div className="input-group mb-3">
                <span
                    className="input-group-text"
                    id={this.props.name + "_id"}>
                    <span style={{ color: "red" }}>{this.requiredMark}</span>{this.props.label}
                </span>
                <span data-tip={ this.props.description }>
                    <Input
                        type="text"
                        className="form-control"
                        onChange={this.changedValue}
                        aria-describedby={this.props.name + "_id"}
                        required={this.props.required} 
                        value={this.value}
                        list={this.datalistName}
                        id={this.props.name + "_id"}
                        placeholder={this.props.placeHolder ? this.props.placeholder : ""}
                    />
                    {this.renderDataList()}
                  </span>  
            </div> 
            );
    }
}