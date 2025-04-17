import React from 'react';
import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import MainLayout from './layouts/MainLayout';
import CarMakesList from './pages/CarMakes/CarMakesList';
import { RouteNames } from './common/constants';


function App() {
  return (
    <Router>
      <Routes>
        <Route element={<MainLayout />}>
          <Route path="/" element={<Home />} />
          <Route path={RouteNames.CAR_MAKE_LIST} element={<CarMakesList />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
