import React from 'react';
import { Input } from 'reactstrap';

import './tooltips.css';

export default class VersionElement extends React.Component {

    constructor(props) {
        super(props);
        if ('showprops' in this.props)
            console.log("buttonelement", this.props);

        this.requiredMark = this.props.required && this.props.required === 'true' ? "*" : "";
        this.changedValue = this.changedValue.bind(this); 
        this.addLabel = this.addLabel.bind(this);
        this.value = ('value' in this.props) ? (this.props.value !== undefined ? this.props.value : " ") : "";
        let valuepart = this.value.split('.');
        this.valueParts["p1"] = (valuepart[0] != undefined ? valuepart[0] : "");
        this.valueParts["p2"] = (valuepart[1] != undefined ? valuepart[1] : "");
        this.valueParts["p3"] = (valuepart[2] != undefined ? valuepart[2] : "");

        this.state = {
            
            p1: this.valueParts["p1"],
            p2: this.valueParts["p2"],
            p3: this.valueParts["p3"],
            value: this.valueParts["p1"] + "." + this.valueParts["p2"] + "." + this.valueParts["p3"] ,
        };
    }
    valueParts = {};
    loaded = false;
    newlabel = "";
    value = "";
    requiredMark = "";
    datalist = "";
     

    componentDidMount() {
        this.loaded = true;
        this.setState({ value: ('value' in this.props) ? this.props.value : "" });
    }

    changedValue(name, e) {
        this.valueParts[name] = e.target.value;

        this.setState({
            p1: this.valueParts["p1"],
            p2: this.valueParts["p2"],
            p3: this.valueParts["p3"],
            value: this.valueParts["p1"] + "." + this.valueParts["p2"] + "." + this.valueParts["p3"]
        });
        
        this.props.onChange({ name: this.props.name, value: this.state.value });
        
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
                <span data-tip={this.props.description}>
                    <span className="form-control" style={{ display: "flex"}} >
                    <Input
                        type="text"
                            width='3' style={{ width: "50px", display:"flex"}}
                        onChange={(e) => this.changedValue("p1", e)}
                        aria-describedby={this.props.name + "_label"}
                        required={this.props.required}
                        value={this.state.p1}
                        list={this.datalistName}
                        id={this.props.name + "p1_id"}
                         
                         
                        />&nbsp; .&nbsp;
                    <Input
                        type="text"
                        className="form-control"
                        onChange={(e) => this.changedValue("p2", e)}
                        aria-describedby={this.props.name + "_label"}
                        required={this.props.required}
                        value={this.state.p2}
                            list={this.datalistName}
                            width='3' style={{ width: "50px", display: "flex" }}
                        id={this.props.name + "p2_id"}
                        

                        />&nbsp; . &nbsp;
                    <Input
                        type="text"
                        className="form-control"
                        onChange={(e) => this.changedValue("p3", e)}
                        aria-describedby={this.props.name + "_label"}
                        required={this.props.required}
                            value={this.state.p3}
                            width='3' style={{ width: "50px", display: "flex" }}
                        list={this.datalistName}
                        id={this.props.name + "p3_id"}
                         

                        />
                        <input type="hidden" name={this.props.name} value={this.state.value} />
                    </span>
                </span>
            </div>
        );
    }
}