import React from 'react';

import ButtonElement from './htmlElements/buttonElement';
import DateElement from './htmlElements/DateElement';
import PasswordElement from './htmlElements/PasswordElement';
import TextElement from './htmlElements/textElement';
import VersionElement from './htmlElements/VersionElement';
import VersionElement from './htmlElements/TextAreaElement';
import VersionElement from './htmlElements/JSONEditorElement';

export default class QueryDefForm extends React.Component {

    constructor(props) {
        super(props);

        this.saveForm = this.saveForm.bind(this);
        this.applyValues = this.applyValues.bind(this);
         
        this.handleSubmit = this.handleSubmit.bind(this);
        
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
                
        this.applyValues(this.props.formData);

        this.setState({
            origData: this.props.formData,
            ready: true,
            fields: fields
        });
    }

    applyValues(formData) {
        if (this.state === undefined)
            return;

        let fields = this.state.fields;
        for (var a = 0; a < fields.length; a++) {

            if (formData[fields[a].name] !== undefined)
                fields[a]["value"] = formData[fields[a].name];
        }
        this.setState({ fields: fields });
    }

    availableComponents = {
        "ButtonElement": ButtonElement,
        "DateElement": DateElement,
        "PasswordElement": PasswordElement,
        "TextElement": TextElement
    };

    saveForm(e) {
        console.log("submit", e);
        var formdata = {}
        for (var a = 0; a < e.target.length; a++) {
            if (e.target[a].name !== '')
                formdata[e.target[a].name] = e.target[a].value;

        }
        this.props.onSubmit(formdata);
        e.preventDefault();
    };

    setViewMod(newMode) {
        this.setState({ viewMode: newMode });
    }; 

    changedValue(e) {
        
        this.setState({ [e.name]: e.value });
        console.log("state", this.state);
        console.log("event", e);
    }

    handleFormChange() {
        return true;
    };
     Here s
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
                    value: this.hasField(  field, ""),
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
                    value: this.hasField(field, ""),
                    active: false,
                    onChange: (t) => this.changedValue(t)
                });
            }
            else if (field.inputType === "TextAreaElement" || field.inputType.toLowerCase() === "textarea") {
                console.log("TextArea");
                return React.createElement(TextAreaElement, {
                    description: this.hasField(field, "description", ""),
                    disabled: false,
                    name: this.hasField(field, "name", ""),
                    required: this.hasField(field, "required", false),
                    label: this.hasField(field, "name", ""),
                    showprops: true, 
                    value: this.hasField(field, ""),
                    active: false,
                    onChange: (t) => this.changedValue(t)
                });
            }
            else if (field.inputType === "JSONEditorElement" || field.inputType.toLowerCase() === "jsoneditor") {
                console.log("JsonEditor");
                return React.createElement(JSONEditorElement, {
                    description: this.hasField(field, "description", ""),
                    disabled: false,
                    name: this.hasField(field, "name", ""),
                    required: this.hasField(field, "required", false),
                    label: this.hasField(field, "name", ""),
                    showprops: true,
                    value: this.hasField(field, ""),
                    onChange: (t) => this.changedValue(t)
                });
            }
            else if (field.inputType === "VersionElement" || field.inputType.toLowerCase() === "versiontext") {
                console.log("version");
                return React.createElement(VersionElement, {
                    description: this.hasField(field, "description", ""),
                    disabled: false,
                    name: this.hasField(field, "name", ""),
                    required: this.hasField(field, "required", false),
                    label: this.hasField(field, "name", ""),
                    showprops: true,
                     
                    value: this.hasField(field, ""),
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
                    value: this.hasField(field, ""),
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
                <div id='qdefeditor' className="container border"  >
                    <p>!!!</p>
                    <form id='qDefContainer' onSubmit={(e) => { this.saveForm(e); e.preventDefault()}} >
                        {this.props.QueryDef.fields.map(field => this.components(field))}
                        <input type='submit' value='Submit' className="btn btn-primary"   />
                        <input type='reset' value='Reset' className="btn btn-warning" />
                    </form>
                </div>



            );
        else
            return (<div>loading</div>);
    }
}