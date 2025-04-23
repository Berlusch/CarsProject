import React from 'react';
import './SearchBox.css';

const SearchBox = ({ value, onChange, onSearch, placeholder }) => {
  
  // Funkcija koja se poziva kad korisnik pritisne Enter
  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      onSearch(value);  // Poziva pretragu s trenutnim value
    }
  };

  return (
    <div className="search-box">
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}         
        placeholder={placeholder || 'Search...'}
        className="search-input"
      />
    </div>
  );
};

export default SearchBox;
