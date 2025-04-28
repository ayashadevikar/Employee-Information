import { Navigate , Routes, Route } from 'react-router-dom';
import Login from './Components/Login.jsx';
import Register from './Components/Register.jsx';
import React from "react";
import Home from "./Components/Home.jsx";
import { useState } from 'react';
import Navbar from './Components/Navbar.jsx';
import EmployeeList from './Components/EmployeeList.jsx';


function App() {
   
    const [token, setToken] = useState(localStorage.getItem('token'));

   
    return (
        <>
      
      <Navbar token={token} setToken={setToken} />
       
      <Routes>
       <Route path="/login" element={<Login setToken={setToken} />} />
       <Route path="/data" element={token ? <Home token={token} /> : <Navigate to="/login" />} />
       <Route path="/" element={<Home />} />
        <Route path="/register" element={<Register />} />
        <Route path="/employee-list" element={<EmployeeList />} />
      </Routes>
   
  
    </>
    )
    
   
}

export default App;