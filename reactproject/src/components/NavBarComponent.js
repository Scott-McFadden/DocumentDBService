import React from 'react';
import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,Button,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,DropdownItem, Dropdown,
    DropdownMenu, 
} from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.css';

export default class NavBar1 extends React.Component {
    constructor(props) {
        super(props);
        this.handleTabChange = this.handleTabChange.bind(this);
        this.toggle = this.toggle.bind(this);
        this.dropdownToggle = this.dropdownToggle.bind(this);
        this.state = {
            isOpen: false, 
            dropdownOpen : false
        };
    }
     loaded = false;

    componentDidMount() {
        this.loaded = true;
    }
    handleShowPanel(e) {
        if (!this.loaded) return;
        this.props.onShowPanel(e);

    }

    handleTabChange(e) {
        if (!this.loaded) return; 
        this.props.onTabChange(e);
         
    }

    toggle() {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }

    dropdownToggle() {
        this.setState({
            dropdownOpen: !this.state.dropdownOpen
        });
    }
    render() {
        return (
            <div>
                <Navbar color="light" light expand="md">
                    <NavbarBrand href="/">DocDB</NavbarBrand>
                    <NavbarToggler onClick={this.toggle} />
                    <Collapse isOpen={this.state.isOpen} navbar>
                        <Nav className="ml-auto" navbar>
                            <NavItem>
                                <Button outline color="primary" onClick={() => this.handleTabChange("1")} >Query Definitions</Button>
                            </NavItem>
                            <div>&nbsp;</div>
                            <NavItem>
                                <Button outline color="primary" onClick={() => this.handleTabChange("2")}>Editor</Button>
                            </NavItem>
                            <Dropdown isOpen={this.state.dropdownOpen} toggle={this.dropdownToggle}>
                                <DropdownToggle caret>
                                    Dropdown
                                </DropdownToggle>
                            <DropdownMenu>
                                <DropdownItem onClick={() =>  this.handleShowPanel("getOne")}>GetOne</DropdownItem>
                                <DropdownItem onClick={() =>  this.handleShowPanel("get")}>Get </DropdownItem>
                                <DropdownItem onClick={() => this.handleShowPanel("getlimit")}>Get Limit </DropdownItem>
                                <DropdownItem divider />
                                <DropdownItem onClick={() => this.handleShowPanel("insert")}>Insert </DropdownItem>
                                <DropdownItem onClick={() =>  this.handleShowPanel("update")}>Update </DropdownItem>
                                <DropdownItem onClick={() => this.handleShowPanel("delete")}>Delete </DropdownItem>
                                </DropdownMenu>
                                </Dropdown>
                        </Nav>
                    </Collapse>
                </Navbar>
            </div>
        );
    }
}