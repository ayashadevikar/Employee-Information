import React, { useState, useEffect } from "react";
import Table from 'react-bootstrap/Table';
import 'bootstrap/dist/css/bootstrap.min.css';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

const CRUD = () => {
    const [show, setShow] = useState(false);
    const [name, setName] = useState('');
    const [age, setAge] = useState('');
    const [editID, setEditId] = useState('');
    const [editName, setEditName] = useState('');
    const [editAge, setEditAge] = useState('');
    const [data, setData] = useState([]);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const getData = () => {
      const apiUrl = import.meta.env.VITE_API_URL; 
       
        //const apiUrl = `http://localhost:5000/api/Employee`;

        if (!apiUrl) {
            toast.error("API URL is missing. Please check your environment variables.");
            return;
        }


        axios.get(`${apiUrl}/api/Employee`)
       // axios.get(apiUrl)
            .then((result) => {
                const fetchedData = Array.isArray(result.data) ? result.data : []; // Ensure data is an array
                setData(fetchedData);
            })
            .catch((error) => {
                console.error(error);
                toast.error("Failed to fetch data.");
            });
    };

    useEffect(() => {
        getData();
    }, []);

    const handleEdit = (id) => {
        handleShow();
        const apiUrl = import.meta.env.VITE_API_URL; // Read API URL from environment variable
        //const apiUrl = `http://localhost:5000/api/Employee/${id}`;

        if (!apiUrl) {
            toast.error("API URL is missing.");
            return;
        }

        axios.get(`${apiUrl}/api/Employee/${id}`)
        //axios.get(apiUrl )
            .then((result) => {
                setEditName(result.data.name);
                setEditAge(result.data.age);
                setEditId(id);
            })
            .catch((error) => {
                console.error(error);
                toast.error("Failed to fetch employee data.");
            });
    };



    const handleDelete = (id) => {
        if (window.confirm("Are you sure you want to delete this employee?")) {
           // const apiUrl = `http://localhost:5000/api/Employee/${id}`;
          const apiUrl = `import.meta.env.VITE_API_URL`
            // Perform the DELETE request
            axios.delete(apiUrl)
                .then((result) => {
                    if (result.status === 204) {
                        toast.success("Employee has been deleted");
    
                        // Update the UI immediately by removing the employee from the state
                        // Make sure `id` is passed correctly from the backend, if needed change `id` to `_id`
                        setData(prevData => prevData.filter(emp => emp.id !== id));  // Assuming `emp.id` is used, adjust if `_id` is used
    
                        // Optionally, re-fetch after a short delay for consistency with the backend
                        setTimeout(() => {
                            getData();  // Re-fetch data if necessary
                        }, 500);
                    }
                })
                .catch((error) => {
                    console.error(error);
                    toast.error("Failed to delete employee.");
                });
        }
    };
    

    // const handleDelete = (id) => {
    //     if (window.confirm("Are you sure to delete this employee")) {
    //         const apiUrl = `http://localhost:5000/api/Employee/${id}`
    //        //const apiUrl = `import.meta.env.VITE_API_URL`
    //         if (!apiUrl) {
    //             toast.error("API URL is missing.");
    //             return;
    //         }

    //        //axios.delete(`${apiUrl}/api/Employee/${id}`)
    //       axios.delete(apiUrl)
    //             .then((result) => {
    //                 if (result.status === 200) {
    //                     toast.success('Employee has been deleted');
    //                    getData();
    //                 }
    //             })
    //             .catch((error) => {
    //                 console.error(error);
    //                 toast.error("Failed to delete employee.");
    //             });
    //     }
    // };

    const handleUpdate = () => {
        const apiUrl = import.meta.env.VITE_API_URL;
       //const apiUrl = `http://localhost:5000/api/Employee/${editID}`;

        if (!apiUrl) {
            toast.error("API URL is missing.");
            return;
        }

        const data = {
            "id": editID,
            "name": editName,
            "age": editAge
        };

       axios.put(`${apiUrl}/api/Employee/${editID}`, data)
       //axios.put(apiUrl, data)
            .then(() => {
                handleClose();
                getData();
                clear();
                toast.success('Employee has been updated');
            })
            .catch((error) => {
                console.error(error);
                toast.error("Failed to update employee.");
            });
    };

    const handleSave = () => {
       const apiUrl = import.meta.env.VITE_API_URL;
       //const apiUrl = `http://localhost:5000/api/Employee`;

        if (!apiUrl) {
            toast.error("API URL is missing.");
            return;
        }

        const data = {
            "name": name,
            "age": age
        };

        axios.post(`${apiUrl}/api/Employee`, data)
           //axios.post(apiUrl, data)
            .then(() => {
                getData();
                clear();
                toast.success('Employee has been added');
            })
            .catch((error) => {
                console.error(error);
                toast.error("Failed to add employee.");
            });
    };

    const clear = () => {
        setName('');
        setAge('');
        setEditName('');
        setEditAge('');
        setEditId('');
    };

    return (
        <>
            <ToastContainer />
            <h1>Employee List</h1>
            {/* <Container>
                <h1 className="text-center">Employee Information</h1>
                <Row className="m-4">
                    <Col>
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Enter Name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                        />
                    </Col>
                    <Col>
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Enter Age"
                            value={age}
                            onChange={(e) => setAge(e.target.value)}
                        />
                    </Col>
                    <Col>
                        <button className="btn btn-primary" onClick={() => handleSave()}>Submit</button>
                    </Col>
                </Row>
            </Container> */}

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Age</th>
                        {/* <th>Actions</th> */}
                    </tr>
                </thead>
                <tbody>
                    {
                        data && data.length > 0 ?
                            data.map((item, index) => {
                                return (
                                    <tr key={index}>
                                        <td>{index + 1}</td>
                                        <td>{item.name}</td>
                                        <td>{item.age}</td>
                                        {/* <td colSpan={2}>
                                            <button className="btn btn-primary" onClick={() => handleEdit(item.id)}>Edit</button> &nbsp;
                                            <button className="btn btn-danger" onClick={() => handleDelete(item.id)}>Delete</button>
                                        </td> */}
                                    </tr>
                                );
                            })
                            :
                            <tr>
                                <td colSpan="5" className="text-center">Loading...</td>
                            </tr>
                    }
                </tbody>
            </Table>

            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Modify / Update Employee</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col>
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Enter Name"
                                value={editName}
                                onChange={(e) => setEditName(e.target.value)}
                            />
                        </Col>
                        <Col>
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Enter Age"
                                value={editAge}
                                onChange={(e) => setEditAge(e.target.value)}
                            />
                        </Col>
                    </Row>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleUpdate}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default CRUD;
