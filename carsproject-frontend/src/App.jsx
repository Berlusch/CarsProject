import React from 'react';
import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import MainLayout from './layouts/MainLayout';
import CarMakesList from './pages/CarMakes/CarMakesList';
import { RouteNames } from './common/constants';
import CarMakesAdd from './pages/CarMakes/CarMakesAdd';
import CarMakesEdit from './pages/CarMakes/CarMakesEdit';
import CarEngineTypesList from './pages/CarEngineTypes/CarEngineTypesList';
import CarRegistrationsList from './pages/CarRegistrations/CarRegistrationsList';
import CarRegistrationsAdd from './pages/CarRegistrations/CarRegistrationsAdd';
import CarRegistrationsEdit from './pages/CarRegistrations/CarRegistrationsEdit';


function App() {
  return (
    <Router>
      <Routes>
        <Route element={<MainLayout />}>

          <Route path="/" element={<Home />} />

          <Route path={RouteNames.CAR_MAKE_LIST} element={<CarMakesList />} />
          <Route path={RouteNames.CAR_MAKE_ADD} element={<CarMakesAdd />} />
          <Route path={RouteNames.CAR_MAKE_EDIT} element={<CarMakesEdit/>} />

          <Route path={RouteNames.CAR_REGISTRATION_LIST} element={<CarRegistrationsList />} />
          <Route path={RouteNames.CAR_REGISTRATION_ADD} element={<CarRegistrationsAdd />} />
          <Route path={RouteNames.CAR_REGISTRATION_EDIT} element={<CarRegistrationsEdit/>} />

          <Route path={RouteNames.CAR_ENGINE_TYPE_LIST} element={<CarEngineTypesList/>} />
          
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
