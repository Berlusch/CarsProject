import React from 'react';
import SignpostButton from '../components/SignpostButton';

function Home() {
  return (
    <div className="home">
      <div className="main">
        <SignpostButton text="Car Makes" link="./car-makes" />
        <SignpostButton text="Car Models" link="./car-models" />
        <SignpostButton text="Car Owners" link="./car-owners" />
        <SignpostButton text="Car Registrations" link="./car-registrations" />
        <SignpostButton text="Car Engine Types" link="./car-engine-types" />
      </div>
    </div>
  );
}

export default Home;
