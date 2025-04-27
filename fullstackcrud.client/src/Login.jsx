import React, { useState } from 'react';
import axios from 'axios';

export default function Login({ setToken }) {
  const [form, setForm] = useState({ username: '', password: '' });

  const handleLogin = async () => {
    try {
      // Use the VITE_API_URL environment variable for the API URL
      const apiUrl = import.meta.env.VITE_API_URL;

      // Send the POST request to the login API endpoint
      const res = await axios.post(`${apiUrl}/login`, form, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      // On successful login, save the token and call setToken
      const token = res.data.token;
      localStorage.setItem('token', token);
      setToken(token);
      alert('Login successful');
    } catch (err) {
      // Handle error during login
      alert('Invalid credentials');
    }
  };

  return (
    <div>
      <h2>Login</h2>
      <input placeholder="Username" onChange={e => setForm({ ...form, username: e.target.value })} />
      <input placeholder="Password" type="password" onChange={e => setForm({ ...form, password: e.target.value })} />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
}
