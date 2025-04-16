import React from 'react';
import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home'





function App() {
  return (
    
    <Router>
    <div className="app">
      {}
      <header>
      <a href="/">
        <img src="/cars-logo.png" alt="Logo" className="logo" />
        </a>
      </header>

      <main className="main">
        <Routes>
          {}
          <Route path="/" element={<Home />} />                                         
        </Routes>
        <div style={{ textAlign: 'center', paddingTop: '40px', paddingRight: '30px', fontSize: '12px' }}>
  &copy; Bernarda Lusch 2025
</div>
      </main>
    </div>
     
  </Router>
    
  );
}

export default App;
