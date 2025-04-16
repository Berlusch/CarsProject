import React from 'react';
import { Outlet } from 'react-router-dom';

const MainLayout = () => {
  return (
    <div className="app">
      <header>
        <a href="/">
          <img src="/cars-logo.png" alt="Logo" className="logo" />
        </a>
      </header>

      <main className="main">
        <Outlet />
        <div style={{ textAlign: 'center', paddingTop: '40px', paddingRight: '30px', fontSize: '12px' }}>
          &copy; Bernarda Lusch 2025
        </div>
      </main>
    </div>
  );
};

export default MainLayout;
