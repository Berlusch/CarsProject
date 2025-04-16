import React from 'react';
import SignpostButton from '../components/SignpostButton';

function Home() {
  return (
    <div className="home">
      <div className="main">
        <SignpostButton text="Car Makes" link="./carmakes" />
        <SignpostButton text="Car Models" link="./carmodels" />
        <SignpostButton text="Car Owners" link="./carowners" />
        <SignpostButton text="Car Registrations" link="./carregistrations" />
        <SignpostButton text="Car Engine Types" link="./carenginetypes" />
      </div>
    </div>
  );
}

export default Home;
