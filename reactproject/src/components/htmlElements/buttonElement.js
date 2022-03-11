import React from 'react';  
import './tooltips.css'; 
import "bootstrap/dist/css/bootstrap.css";
import { Button } from 'reactstrap';
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
            <div  >     
                
                <span data-tip={ this.props.description }>
                    <Button
                        color={this.state.buttonType}
                        size={this.state.size}
                        onClick={this.handleOnClick}
                        id={this.props.name + "_id"}
                        active={this.state.active}
                        outline={this.state.outline}
                        disabled={this.state.disabled}
                    >{this.props.value}</Button>
                        
                    
                    
                  </span>  
            </div> 
            );
        else
            return(<div />)
    }
}