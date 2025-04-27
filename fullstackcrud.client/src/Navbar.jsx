import React from 'react';
import { useNavigate, Link } from 'react-router-dom';


function Navbar() {
  const navigate = useNavigate();
  const token = localStorage.getItem('token');

  const handleLogout = () => {
    localStorage.removeItem('token'); // Remove JWT
    navigate('/login'); // Redirect to login
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
      <div className="container">
        <a className="navbar-brand" href="/">Employee App</a> 
        <Link to='/login' type="button" className="btn btn-primary px-4">Login</Link>
        <Link to='/register' type="button" className="btn btn-primary px-4">Register</Link>
       
        <div className="d-flex">
          {token ? (
            <button className="btn btn-outline-danger" onClick={handleLogout}>
              Logout
            </button>
          ) : null}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;