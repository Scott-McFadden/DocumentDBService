import { Collapse, Nav, NavItem, NavLink, Col, Row, Button, Form, TabContent, TabPane, Card, CardText, CardTitle } from 'reactstrap';
import React, { Component } from 'react';
import   classnames   from 'classnames';
import  NavBar1  from './components/NavBarComponent'; 
import TextElement from './components/htmlElements/textElement';
import DisplayPropertiesAndValues from './components/DisplayPropertiesAndValues';
import DateElement from './components/htmlElements/DateElement';
import ButtonElement from './components/htmlElements/buttonElement';
import PasswordElement from './components/htmlElements/PasswordElement';
import DisplayQueryDefList from './components/DisplayQueryDefList';


export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.toggle = this.toggle.bind(this);
        this.resetPanel = this.resetPanel.bind(this);
        this.handleShowPanel = this.handleShowPanel.bind(this);
        this.updateFormData = this.updateFormData.bind(this);
        this.onClickMeClick = this.onClickMeClick.bind(this);

        this.state = {
            forecasts: [],
            loading: true,
            isOpen: false,
            activeTab: '1',
            panel: {
                getOne: true,
                get: false,
                getlimit: false,
                insert: false,
                update: false,
                delete: false,
            },
            formData : {
                testitem : ""
            },
            currentQueryDef: {
                id: 'notloaded',
                name: "loading",
                description: "loading"
            },

        }; 
        this.resetPanel("getOne");
    };

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderTableRow(item) {
        return (
            <tr key={item}>
                <td>{item}</td>
            </tr>

        )
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table- striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>

                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast => <tr key={forecast}>
                        <td>{forecast}</td>
                    </tr>)}
                </tbody>
            </table>
        );
    }

    resetPanel(e) {
        var temppanel = {
            getOne: false,
            get: false,
            getlimit: false,
            insert: false,
            update: false,
            delete: false,
        };
        temppanel[e] = true;
        return temppanel;

    }

    updateFormData(field, value) {
        var s = this.state.formData;
        s[field] = value;
        this.setState({ formData: s });
    }

    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }

    handleTabChange(e) {
        console.log("tab change", e);
    }

    tabtoggle(tab) {
        if (this.state.activeTab !== tab) {
            this.setState({
                activeTab: tab
            });
        }
    }

    onClickMeClick(t) {
        console.log("click me ", t);
    }

    handleShowPanel(e) {
        
        this.setState({ panel : this.resetPanel(e)})
        console.log(e, this.state.panel[e]);
    }
    ChangeQueryDef(e) {
        this.setState({ currentQueryDef : e })

    }

    render() {
        let contents = this.state.loading
            ?  <span><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></span>
            : App.renderForecastsTable(this.state.forecasts);

        return (
            <div>
                
                <NavBar1 onTabChange={this.handleTabChange} onShowPanel={this.handleShowPanel} />
                <Nav tabs>
                    <NavItem>
                        <NavLink
                            className={classnames({ active: this.state.activeTab === '1' })}
                            onClick={() => { this.tabtoggle('1'); }}
                        >
                            QueryDefs
                        </NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink
                            className={classnames({ active: this.state.activeTab === '2' })}
                            onClick={() => { this.tabtoggle('2'); }}
                        >
                            Execute Queries
                        </NavLink>

                    </NavItem>
                    <NavItem>
                        <NavLink
                            className={classnames({ active: this.state.activeTab === '3' })}
                            onClick={() => { this.tabtoggle('3'); }}
                        >
                            QueryDef Editor
                        </NavLink>

                    </NavItem>
                    <NavItem>
                        <NavLink
                            className={classnames({ active: this.state.activeTab === '4' })}
                            onClick={() => { this.tabtoggle('4'); }}
                        >
                            Weather
                        </NavLink>

                    </NavItem>
                </Nav>
                <TabContent activeTab={this.state.activeTab} style={{marginLeft:"20px"}}>
                    <TabPane tabId="1">
                        <Row>
                            <Col sm="12">
                                <h4>Query Def lookup</h4>
                                <DisplayQueryDefList onQueryDefChange={(e) =>  this.ChangeQueryDef(e)  } />
                            </Col>
                        </Row>
                    </TabPane>
                    
                    <TabPane tabId="2">
                        <Row>
                             
                            <h5>View/Edit Query Definition for {this.state.currentQueryDef.name}</h5>
                            <Form>
                                <TextElement name='testitem'
                                    placeholder='placeholder test here'
                                    required={true}
                                    value='oldvalue'
                                    label='label this'
                                    datalist={['test', 'quest', 'west']}
                                    description='hover over me please'
                                    onChange={(field, value) => this.updateFormData(field, value) }
                                />
                                <TextElement name='newItem'
                                    placeholder='placeholder new items goes here'
                                    required={false}
                                    value='new item value'
                                    label='item number 2'
                                    description='hover over me please'
                                    onChange={(field, value) => this.updateFormData(field, value)}
                                />
                                <TextElement name='newItem2'
                                    placeholder='newItem2'
                                    required={false}
                                    description='hover over me please'
                                    onChange={(field, value) => this.updateFormData(field, value)}
                                />
                                <DateElement name='DateItem'
                                    required={false}
                                    value={new Date().toISOString().slice(0, 10) }
                                    label='Date item'
                                    description='hover over me please'
                                    onChange={(field, value) => this.updateFormData(field, value)}
                                />
                                <ButtonElement 
                                    buttonType='primary'
                                    description='button hover works'
                                    disabled={false}
                                    name="thing1" showprops={true}
                                    outline={false}
                                    onClick={(t) => this.onClickMeClick(t)} >clickme</ButtonElement>
                                <PasswordElement
                                    size='25'
                                    name="Password"
                                    label='Password'
                                    onChange={(field, value) => this.updateFormData(field, value)}
                                    />

                                <Button>Submit</Button>
                            </Form>
                        </Row>
                        <Row>
                            <DisplayPropertiesAndValues data={this.state.formData} />
                        </Row>
                    </TabPane>
                    <TabPane tabId="3">
                        <Row>
                            <Col sm="12">
                                <h4>Execute  {this.state.currentQueryDef.name}</h4>
                            </Col>
                        </Row>
                        <Row>
                            <Collapse isOpen={this.state.panel.getOne}>
                                
                            </Collapse>
                            <Collapse isOpen={this.state.panel.get}>
                                 getOnePanel
                            </Collapse>
                            <Collapse isOpen={this.state.panel.getlimit}>
                                 getPanelOpen
                            </Collapse>
                            <Collapse isOpen={this.state.panel.insert}>
                                 insertPanelOpen
                            </Collapse>
                            <Collapse isOpen={this.state.panel.update}>
                                 updatePanelOpen
                            </Collapse>
                            <Collapse isOpen={this.state.panel.delete}>
                                deletePanelOpen
                            </Collapse>
                        </Row>

                    </TabPane>
                    <TabPane tabId="4">
                        <Row>
                            <Col sm="6">
                                <Card body>
                                    <CardTitle>Weather Forcast</CardTitle>
                                    <CardText>{contents}</CardText>I 
                                </Card>
                            </Col>
                        </Row>
                    </TabPane>
                </TabContent>
                

            </div>
        );
    }
   

    


    async populateWeatherData() {
      //  const response = await fetch('https://localhost:49163/api/ControlData/ControlData/QueryDef/GetFields', { mode: 'cors' });
     //   const data = await response.json();
     //   this.setState({ forecasts: data, loading: false });
    }
}
