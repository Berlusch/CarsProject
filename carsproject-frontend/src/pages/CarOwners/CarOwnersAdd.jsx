import { observer } from 'mobx-react-lite';
import CarOwnerStore from '../../stores/CarOwnerStore';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { RouteNames } from '../../common/constants';


const CarOwnersAdd = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [dateOfBirth, setDateOfBirth] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    await CarOwnerStore.addCarOwner(firstName, lastName, dateOfBirth); 
    if (CarOwnerStore.addStatus.error) {
      alert('Adding item failed.');
    } else {
      CarOwnerStore.currentPage = 1;
      navigate(RouteNames.CAR_OWNER_LIST);
    }    
  };

  const handleCancel = () => {
    setFirstName('');
    setLastName('');
    setDateOfBirth('')
    CarOwnerStore.currentPage = 1;
    navigate(RouteNames.CAR_OWNER_LIST);
  };

  return (
    <div className="form-container">
      <h2>Add Car Owner</h2>
      
      <div className="form-group">
        <label htmlFor="firstName">First Name</label>
        <input
          type="text"
          id="firstName"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          placeholder="Enter first name"
        />
      </div>

      <div className="form-group">
        <label htmlFor="lastName">Last Name</label>
        <input
          type="text"
          id="lastName"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          placeholder="Enter last name"
        />
      </div>

      <div className="form-group">
        <label htmlFor="dateOfBirth">Date of Birth</label>
        <input
          type="date"
          id="dateOfBirth"
          value={dateOfBirth}
          onChange={(e) => setDateOfBirth(e.target.value)}
          placeholder="Enter date of birth (dd-mm-yyyy)"
        />
      </div>

      <div className="form-button-container">
        <button className="cancel-button" onClick={handleCancel}>Cancel</button>
        <button className="add-button" onClick={handleSubmit}>Submit</button>
      </div>
    </div>
  );
};

export default observer(CarOwnersAdd);
