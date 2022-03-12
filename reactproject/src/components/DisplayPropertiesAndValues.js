import React from 'react';

export default class DisplayPropertiesAndValues extends React.Component {
     
    loaded = false;

    componentDidMount() {
        this.loaded = true;
    }
     
    render() {
        var a = Object.entries(this.props.data);
        return (
            <table className='table'>
                <thead>
                    <tr>
                        <th scope="col">Property</th>
                        <th scope="col">Value</th>
                    </tr>
                </thead>
                <tbody>
                    {a.map(b => <tr key={b[0]}><td>{b[0]}</td><td>{b[1]}</td></tr>) }
                </tbody>
            </table> 
            );
    }
}