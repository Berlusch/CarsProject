import React from "react";

const SignpostButton = ({ label }) => {
  return (
    <button className="signpost-button">
      <span className="label">{label}</span>
      <span className="arrow"></span>
    </button>
  );
};

export default SignpostButton;
