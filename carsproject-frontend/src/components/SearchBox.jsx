import React from 'react';
import './SearchBox.css';

const SearchBox = ({ value, onChange, placeholder }) => {
  return (
    <div className="search-box">
      <input
        type="text"
        value={value}
        onChange={onChange}
        placeholder={placeholder || 'Search...'}
        className="search-input"
      />
    </div>
  );
};

export default SearchBox;
