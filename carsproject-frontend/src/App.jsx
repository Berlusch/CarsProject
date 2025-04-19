import React from 'react';
import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import MainLayout from './layouts/MainLayout';
import CarMakesList from './pages/CarMakes/CarMakesList';
import { RouteNames } from './common/constants';
import CarMakesAdd from './pages/CarMakes/CarMakesAdd';
import CarMakesEdit from './pages/CarMakes/CarMakesEdit';


function App() {
  return (
    <Router>
      <Routes>
        <Route element={<MainLayout />}>
          <Route path="/" element={<Home />} />
          <Route path={RouteNames.CAR_MAKE_LIST} element={<CarMakesList />} />
          <Route path={RouteNames.CAR_MAKE_ADD} element={<CarMakesAdd />} />
          <Route path={RouteNames.CAR_MAKE_EDIT} element={<CarMakesEdit/>} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
