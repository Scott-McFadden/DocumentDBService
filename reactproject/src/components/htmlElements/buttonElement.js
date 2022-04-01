import React from 'react';  
import './tooltips.css'; 
import "bootstrap/dist/css/bootstrap.css";
import { Button } from 'reactstrap';

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

export default class ButtonElement extends React.Component {
      
    constructor(props) {
        super(props);

        if ('showprops' in this.props)
            console.log("buttonelement", this.props);

        var act = ('active' in this.props) ? this.props.active : false;
        var bt = ("buttonType" in this.props) ? this.props.buttonType : "primary";
        var sz = ("size" in this.props) ? this.props.size : "sm";
        var dis = ("disabled" in this.props) ? this.props.disabled : false;
        var outl = ("outline" in this.props) ? this.props.outline : false;
        this.state = {
            active: act,
            buttonType: bt,
            size: sz,
            disabled: dis,
            outline: outl,
        }
        this.handleOnClick = this.handleOnClick.bind(this);
    }
    
    loaded = false;
    active = false; 
     
    componentDidMount() {
        this.loaded = true;
    }

    handleOnClick() {
        if(this.loaded)
           this.props.onClick(this.props.name);
        //this.setState({ active: !this.state.active });
    }
     
    render() {
        if(this.loaded)
            return ( 
                <div>     
                    <span data-tip={ this.props.description }>
                        <Button
                            color={this.state.buttonType}
                            size={this.state.size}
                            onClick={this.handleOnClick}
                            id={this.props.name + "_id"}
                            active={this.state.active}
                            outline={this.state.outline}
                            disabled={this.state.disabled}
                            name={this.props.name}
                        >{this.props.children}</Button> 
                      </span>  
                </div> 
                );
        else
            return(<div />)
    }
}