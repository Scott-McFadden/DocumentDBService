import React from 'react';

import ButtonElement from './htmlElements/buttonElement';
import DateElement from './htmlElements/DateElement';
import PasswordElement from './htmlElements/PasswordElement';
import TextElement from './htmlElements/textElement';


export default class QueryDefForm extends React.Component {


    constructor(props) {
        super(props);

        this.saveForm = this.saveForm.bind(this);
        this.setViewMode = this.saveForm.bind(this);
        this.loadForm = this.loadForm.bind(this);
        this.handleFormChange = this.handleFormChange.bind(this);
        this.resetForm = this.resetForm.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fieldContent = this.fieldContent.bind(this);
        this.hasField = this.hasField.bind(this);
        this.changedValue = this.changedValue.bind(this);

        this.state = {
            viewMode: "view",
            formData: [], 
            origData: [], 
            fields: [],
            ready: false 
        };
    }
    componentDidMount() {
        if ("showprops" in this.props)
            console.log("querydefform => ", this.props);
        var fields = this.hasField( this.props.QueryDef, "fields", []);
        this.setState({
            formData: this.props.formData,
            origData: this.props.formData,
             
            ready: true
        });
        //    {username: 'Emcfadden', age: '44'}
        /*     {
                "name": "Validation",
                "dbName": "validation",
                "description": "validation strategy",
                "dataType": "object",
                "validation": "none",
                "validationType": 0,
                "inputType": "Text"
                }
        */
       

        
            //for (var b = 0; b < fields.length; a++) {
            //    for (let x in this.props.formData) {

            //    }
            //}
       /* }*/

        }
    }

    availableComponents = {
        "ButtonElement": ButtonElement,
        "DateElement": DateElement,
        "PasswordElement": PasswordElement,
        "TextElement": TextElement
    };

    saveForm() {
        return true;
    };
    fieldContent() {
 
    }
    setViewMod(newMode) {
        this.setState({ viewMode: newMode });
    };

    loadForm() {
        return true;
    };
    changedValue(e) {
        let value = e.target.value;
        this.setState({ f: value });
        console.log("event": e);
    }
    handleFormChange() {
        return true;
    };

    resetForm() {
        return true;
    };
    handleSubmit(event) {
        console.log(event);
    }

    hasField(obj, field, defaultValue) {
        return (field in obj) ? obj[field] : defaultValue;
    }

    components(field) {

        
        if (field.name === undefined  || field.name==='')
            return (<span />);
        if (field.inputType === "ButtonElement") {
            return React.createElement(ButtonElement, {
                description: this.hasField(field, "description", ""),
                disabled: false,
                name: this.hasField(field, "name", ""),
                size: "sm",
                showprops: true,
                active: false,
                onClick: (t) => this.onClick(t)
            });
        } else
            if (field.inputType === "DateElement") {
                return React.createElement(DateElement, {
                    description: this.hasField(field, "description", ""),
                    disabled: false,
                    name: this.hasField(field, "name", ""),
                    required: this.hasField(field, "required", false),
                    label: this.hasField(field, "name", ""),
                    showprops: true,
                    value: this.hasField(this.state.formData, field.name, ""),

                    active: false,
                    onChange: (t) => this.changedValue(t)
                });
            }
            else if (field.inputType === "TextElement" || field.inputType.toLowerCase() === "text") {
                console.log("text");
                return React.createElement(TextElement, {
                    description: this.hasField(field, "description", ""),
                    disabled: false,
                    name: this.hasField(field, "name", ""),
                    required: this.hasField(field, "required", false),
                    label: this.hasField(field, "name", ""),
                    showprops: true,
                    list: this.hasField(field, "datalist", null),
                    value:    "", // this.hasField(this.state.formData, field.name, ""),
                    active: false,
                    onChange: (t) => this.changedValue(t)
                });
            }
            else if (field.inputType === "PasswordElement" || field.inputType.toLowerCase() === "password") {
                return React.createElement(PasswordElement, {
                    description: this.hasField(field, "description", ""),
                    disabled: false,
                    label: this.hasField(field, "name", ""),
                    name: this.hasField(field, "name", ""),
                    required: this.hasField(field, "required", false),
                     
                    value: this.hasField(this.state.formData, field.name, ""),
                    showprops: true,
                    active: false,
                    onChange: (t) => this.changedValue(t)
                });
            }
         
        return (<div>The component ({field.name}) is undefined </div> );
    }

    render() {
        console.log("fields", this.props.QueryDef);
        if ( this.state.ready  && ("fields" in this.props.QueryDef)) 
            return (
                <div id='qdefeditor' className="container border">
                    <p>!!!</p>
                    <form id='qDefContainer' onSubmit={this.handleSubmit}>
                        {this.props.QueryDef.fields.map(field => this.components(field))}
                        <input type='submit' value='Submit' className="btn btn-primary" /><input type='reset' value='Reset' className="btn btn-warning" />
                    </form>
                </div>



            );
        else
            return (<div>loading</div>);
    }
}