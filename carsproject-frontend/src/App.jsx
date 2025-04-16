import React from "react";
import SignpostButton from "./components/SignpostButton";

function App() {
  return (
    <div className="button-container">
      <SignpostButton label="Car Makes" />
      <SignpostButton label="Car Models" />
      <SignpostButton label="Car Owners" />
      <SignpostButton label="Car Registrations" />
      <SignpostButton label="Car Engine Types" />
    </div>
  );
}

export default App;
