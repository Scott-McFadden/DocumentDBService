import React from 'react';
import { FormGroup, Label, Input, Collapse } from 'reactstrap';
 
import './tooltips.css';

export default class TextElement extends React.Component {

    constructor(props) {
        super(props);
         
        //this.curValue = this.props.value;
        //this.handleValueChange = this.handleValueChange.bind(this);
        this.requiredMark = this.props.required && this.props.required === 'true' ? "*" : "";
       // this.value = this.props.value;
        this.changedValue = this.changedValue.bind(this);

    }
    loaded = false;
    newlabel = "";
    //curValue = ""; 
    value = "";
    requiredMark = "";

    componentDidMount() {
        this.loaded = true;
        this.value = this.props.value;
    }

    changedValue(e) {
        console.log(e);
        console.log(e.target.value);

        this.value = e.target.value;
        console.log(this.value);
        this.props.onChange(this.props.name, this.value);
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
                         
                        value={this.value}
                        id={this.props.name + "_id"}
                        placeholder={this.props.placeHolder ? this.props.placeholder : ""}
                    />
                    
                  </span> 
               
                     
            
            </div>

            );
    }
}