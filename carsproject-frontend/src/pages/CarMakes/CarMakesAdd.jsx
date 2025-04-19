// U tvojoj komponenti za dodavanje
import { observer } from 'mobx-react-lite';
import CarMakeStore from '../../stores/CarMakeStore';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { RouteNames } from '../../common/constants';


const CarMakesAdd = () => {
  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    await CarMakeStore.addCarMake(name, abrv); // Pozivamo metodu iz store-a
    if (CarMakeStore.addStatus.error) {
      alert('Adding item failed.');
    } else {
      navigate(RouteNames.CAR_MAKE_LIST);
    }
    
  };

  const handleCancel = () => {
    // Funkcija za poništavanje (možeš dodati logiku za vraćanje na prethodnu stranicu)
    setName('');
    setAbrv('');
    navigate(RouteNames.CAR_MAKE_LIST);
  };

  return (
    <div className="form-container">
      <h2>Add Car Make</h2>
      
      <div className="form-group">
        <label htmlFor="name">Name</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Enter name"
        />
      </div>

      <div className="form-group">
        <label htmlFor="abrv">Abbreviation</label>
        <input
          type="text"
          id="abrv"
          value={abrv}
          onChange={(e) => setAbrv(e.target.value)}
          placeholder="Enter abbreviation"
        />
      </div>

      <div className="form-button-container">
        <button className="cancel-button" onClick={handleCancel}>Cancel</button>
        <button className="add-button" onClick={handleSubmit}>Submit</button>
      </div>
    </div>
  );
};

export default observer(CarMakesAdd);
