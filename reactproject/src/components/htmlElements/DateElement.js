import React from 'react';
import { Input } from 'reactstrap';

import './tooltips.css';

export default class DateElement extends React.Component {

    constructor(props) {
        super(props);

        if ('showprops' in this.props)
            console.log("DateElement", this.props);

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
        this.setState({ value: ('value' in this.props) ? this.props.value : "" });
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
            <div className="input-group">
                {this.addLabel()}
                <span data-tip={this.props.description}>
                    <Input
                        type="date"
                        className="form-control"
                        onChange={this.changedValue}
                        aria-describedby={this.props.name + "_id"}
                        required={this.props.required}
                        value={this.state.value} 
                        id={this.props.name + "_id"}
                        name={this.props.name}
                        placeholder={this.props.placeHolder ? this.props.placeholder : ""}
                    />
                </span>
            </div>
        );
    }
}