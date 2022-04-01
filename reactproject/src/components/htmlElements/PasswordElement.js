import React from 'react';  
import './tooltips.css'; 
import "bootstrap/dist/css/bootstrap.css";
import { Input } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { fas, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons'
import { library } from '@fortawesome/fontawesome-svg-core'
 
/*  usage:
 *
 * <ButtonElement 
    buttonType='primary'     // standard button types primary, secondary, successs, info, warning, danger, link
    description='button hover works' 
    disabled={false}   // true / false
    name="thing1" 
    size='sm' //   sm or lg  
    showprops={true}  // if included, will dump props to console.
    outline={false}   // sets outline of button
    active={false}    // sets the button's active state - in the case of outline, turns it solid.
    onClick={(t) => this.onClickMeClick(t)} >clickme</ButtonElement>
 *
 * */

export default class PasswordElement extends React.Component {
      
    constructor(props) {
        super(props);

        if ('showprops' in this.props)
            console.log("PasswordElement", this.props);

        library.add(fas, faEye, faEyeSlash);

        if ('showprops' in this.props)
            console.log("PasswordElement", this.props);

        var act = ('active' in this.props) ? this.props.active : false;
        var classes = ("classes" in this.props) ? this.props.classes : "";
        var required = ("required" in this.props) ? this.props.required : false;
        var dis = ("disabled" in this.props) ? this.props.disabled : false;
        var sz = ("size" in this.props) ? this.props.size : 20;
        this.state = {
            active: act,
            classes: classes,
            required: required,
            disabled: dis,
            size : sz,
            value: "",
            type: "password",
            eyeIcon: "faEyeSlash"
        }

        this.addLabel = this.addLabel.bind(this);
        this.changedValue = this.changedValue.bind(this);
        this.toggleType = this.toggleType.bind(this);
        this.eyeIcon = this.eyeIcon.bind(this);
    }
    
    loaded = false;
    active = false;
    faStyle = {
        top: '2px',
        'fontSize': '1.5em',
        position: 'absolute',
        cursor: 'pointer'
    }
     
    componentDidMount() {
        this.loaded = true;
        this.setState({ value: ('value' in this.props) ? this.props.value : "" });
    }

    changedValue(e) {
        this.value = e.target.value;
        this.props.onChange({ name: this.props.name, value: e.target.value });
        this.setState({ value: e.target.value })
    }

    toggleType()   {
        if (this.state.type === 'password')
            this.setState({
                eyeIcon: "faEye",
                type: "text"
            });
        else
            this.setState({
                eyeIcon: "faEyeSlash",
                type: "password"
            });
    }

    eyeIcon() {
        if (this.state.eyeIcon === "faEye")
            return (<FontAwesomeIcon icon="eye" border style={this.faStyle} />);
         
        return (<FontAwesomeIcon icon="eye-slash" border style={this.faStyle} />);
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
        if(this.loaded)
            return ( 
                <div className="input-group ">
                    {this.addLabel()}
                    <span data-tip={ this.props.description } >
                        <Input
                            type={this.state.type}
                            size={this.state.size}
                            className={this.state.classes} 
                            onChange={this.changedValue}
                            aria-describedby={this.props.name + "_id"}
                            required={this.props.required}
                            value={this.state.value}
                            disabled={this.state.disabled}
                            id={this.props.name + "_id"}
                            name={this.props.name}
                        />
                             
                    </span>  <span onClick={this.toggleType}  >{this.eyeIcon()}</span>
                    
                </div> 
                );
        else
            return(<div />)
    }
}