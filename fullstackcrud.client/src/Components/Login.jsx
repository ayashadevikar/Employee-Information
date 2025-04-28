import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';


export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async () => {
    const apiUrl = import.meta.env.VITE_API_URL;

    try {
      const response = await axios.post(`${apiUrl}/api/employee/login`, {
        email,
        password,
      });

      // Assuming token comes in response.data.token
    localStorage.setItem("token", response.data.token);

    alert("Login successful!");
    navigate('/employee-list'); // redirect after login
    } catch (error) {
      console.error(error);
      const message = error.response?.data || error.message;
      alert(`Error logging in: ${message}`);
    }
  };

  return (
    <div className="container mt-5">
      <div className="card p-4 shadow-sm">
        <h2 className="text-center mb-4">Login</h2>
        <div className="mb-3">
          <input
            className="form-control"
            placeholder="Email"
            value={email}
            onChange={e => setEmail(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <input
            className="form-control"
            placeholder="Password"
            type="password"
            value={password}
            onChange={e => setPassword(e.target.value)}
          />
        </div>
        <button className="btn btn-success w-100" onClick={handleLogin}>
          Login
        </button>
      </div>
    </div>
  );
}
