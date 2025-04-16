import React from 'react';
import { Link } from 'react-router-dom';

function SignpostButton({ text, link }) {
  return (
    <Link to={link} className="signpost-button">
      {text}
    </Link>
  );
}

export default SignpostButton;
