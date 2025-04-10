import React, {useState, useEffect} from "react";
import Table from 'react-bootstrap/Table';
import 'bootstrap/dist/css/bootstrap.min.css';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css'


const CRUD = () => {
    const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const [name, setName] = useState('');
  const [age, setAge] = useState('');

  const[editID, setEditId] = useState('');
  const [editName, setEditName] = useState('');
  const [editAge, setEditAge] = useState('');

    //const empdata = [
    //    {
    //       id : 1,
    //       name : 'Manoj',
    //       age : 29,
    //     },

    //     {
    //        id : 2,
    //        name : 'Virat',
    //        age : 29,
    //      },

    //      {
    //        id : 3,
    //        name : 'Sam',
    //        age : 29,
    //      },
    //]

    const [data, setData] = useState([]);

    useEffect(()=>{
         getData();
    },[])

    const getData = () => {
      axios.get(`https://employee-information.onrender.com/api/Employee`)
      .then((result)=>{
        setData(result.data)
      })
      .catch((error)=>{
        console.log(error)
      })
    }

    const handleEdit = (id) => {
        //alert(id);
        handleShow();
        axios.get(`https://employee-information.onrender.com/api/Employee/${id}`)
        .then((result)=>{
            setEditName(result.data.name);
            setEditAge(result.data.age);
            setEditId(id);
          })
        .catch((error)=>{
          toast.error(error);
        })
    }

    const handleDelete = (id) => {
        if(window.confirm("Are you sure to delete this employee") == true)
        {
            axios.delete(`https://employee-information.onrender.com/api/Employee/${id}`)
            .then((result)=>{
              if(result.status === 200){
                toast.success('Employee has been deleted');
                getData();
              }
            })
            .catch((error)=>{
              toast.error(error);
            })
        }
    }

    const handleUpdate =()=>{
        const url = `https://employee-information.onrender.com/api/Employee/${editID}`
        const data = {
          "id": editID,
          "name": editName,
          "age": editAge
        }

        axios.put(url, data)
        .then(() =>{
          handleClose();
          getData();
          clear();
          toast.success('Employee has been updated');
        }) .catch((error)=>{
          toast.error(error);
        })
    }

    const handleSave =()=>{
        const url = `https://employee-information.onrender.com/api/Employee`;
        const data = {
          "name": name,
          "age": age
        }

        axios.post(url, data)
        .then(() =>{
          getData();
          clear();
          toast.success('Employee has been added');
        }) .catch((error)=>{
          toast.error(error);
        })
    }

    const clear = () =>{
      setName('');
      setAge('');
      setEditName('');
      setEditAge('');
      setEditId('');
    }

    return(
        <>
     <ToastContainer />
     <Container>
          <Row>
               <Col>
                 <input type="text" className="form-control" placeholder="Enter Name" value={name} onChange={(e)=> setName(e.target.value)}/>
               </Col>
               <Col>
               <input type="text" className="form-control" placeholder="Enter Age" value={age} onChange={(e)=> setAge(e.target.value)}/>
               </Col>
               <Col>
               <button className="btn btn-primary" onClick={() => handleSave()}>Submit</button>
               </Col>
           </Row>
      </Container>


          <Table striped bordered hover>
      <thead>
        <tr>
          <th>#</th>
          <th>Name</th>
          <th>Age</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
 
          {
            data && data.length > 0 ?
                  data.map((item, index) => {
                    return(
                        <tr key={index}>
                            <td>{index+ 1}</td>
                            <td>{item.id}</td>
                            <td>{item.name}</td>
                            <td>{item.age}</td>
                            <td colSpan={2}>
                                <button className="btn btn-primary" onClick={()=> handleEdit(item.id)}>Edit</button> &nbsp;
                                <button className="btn btn-danger" onClick={()=> handleDelete(item.id)}>Delete</button>
                            </td>
                        </tr>
                    )
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
                 <input type="text" className="form-control" placeholder="Enter Name" value={editName} onChange={(e)=> setEditName(e.target.value)}/>
               </Col>
               <Col>
               <input type="text" className="form-control" placeholder="Enter Age" value={editAge} onChange={(e)=> setEditAge(e.target.value)}/>
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
    )
}

export default CRUD;