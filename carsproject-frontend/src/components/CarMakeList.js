import React, { useState, useEffect } from 'react';
import { observer } from 'mobx-react';
import carMakeStore from '../stores/CarMakeStore';

const CarMakeList = observer(() => {
  const [newMake, setNewMake] = useState('');
  const [filter, setFilter] = useState('');

  useEffect(() => {
    carMakeStore.fetchCarMakes(); // Dohvati podatke s API-ja
  }, []);

  const handleAddMake = () => {
    const newCarMake = { id: Date.now(), name: newMake };
    carMakeStore.addCarMake(newCarMake);
    setNewMake('');
  };

  const filteredMakes = carMakeStore.carMakes.filter(make =>
    make.name.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <div>
      <h2>Lista Marki Vozila</h2>
      <input
        type="text"
        value={newMake}
        onChange={(e) => setNewMake(e.target.value)}
        placeholder="Unesite novu marku"
      />
      <button onClick={handleAddMake}>Dodaj Marka</button>
      <input
        type="text"
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
        placeholder="Filtriraj marke"
      />
      <ul>
        {filteredMakes.map(make => (
          <li key={make.id}>{make.name}</li>
        ))}
      </ul>
    </div>
  );
});

export default CarMakeList;
