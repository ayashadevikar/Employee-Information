import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './Login';
import Register from './Register';
import React from "react";
import CRUD from "./CRUD";
import { useState } from 'react';
import Navbar from './Navbar.jsx'

function App() {
   
    const [token, setToken] = useState(localStorage.getItem('token'));

   
    return (
        <>
      
      <Navbar token={token} setToken={setToken} />
       
      <Routes>
       <Route path="/login" element={<Login setToken={setToken} />} />
        <Route path="/register" element={<Register />} />
        {/* <Route path="/employees" element={<Employees />} /> */}
      </Routes>
   
    <CRUD />
    </>
    )
    
   
}

export default App;