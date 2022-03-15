import React from 'react';
import ButtonElement from './htmlElements/buttonElement';
import DateElement from './htmlElements/DateElement';
import PasswordElement from './htmlElements/PasswordElement';
import TextElement from './htmlElements/TextElement';

import 'bootstrap/dist/css/bootstrap.css';

export default class QueryDefForm extends React.Component {


    constructor(props) {
        super(props);

        this.saveForm = this.saveForm.bind(this);
        this.setViewMode = this.saveForm.bind(this);
        this.loadForm = this.loadForm.bind(this);
        this.handleFormChange = this.handleFormChange.bind(this);
        this.resetForm = this.resetForm.bind(this);


        this.state = {
            viewMode: "view",
            formData: this.props.formData,
            origData: this.props.formData,
            ready: false 
        };
    }
    componentDidMount() {
        this.setState({ready: true }) ;
    }
    saveForm() {
        return true;
    };

    setViewMod(newMode) {
        this.setState({ viewMode: newMode });
    };

    loadForm() {
        return true;
    };

    handleFormChange() {
        return true;
    };

    resetForm() {
        return true;
    };

    
    render() {
        return (
            <div id='qDefForm'>
                <form id='qDefContainer'>
                    <div id="field_id">
                        <label id="field_id_label" for="field_id_in">
                            id:
                        </label>
                        <Input id='field_id_in' type='text' value={this.state.formData.id} />
                    </div>
                    <input type="submit" value="Submit" />
                </form>
            </div>

            
            
            );
    }
}