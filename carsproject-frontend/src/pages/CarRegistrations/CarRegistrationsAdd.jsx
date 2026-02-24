import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form } from 'react-bootstrap';
import { RouteNames } from '../../common/constants';
import CarRegistrationStore from '../../stores/CarRegistrationStore';
import CarOwnerStore from '../../stores/CarOwnerStore';
import CarModelStore from '../../stores/CarModelStore';

const CarRegistrationsAdd = () => {
  const [registrationNumber, setRegistrationNumber] = useState('');
  const [carOwners, setCarOwners] = useState([]);
  const [carModels, setCarModels] = useState([]);
  const [carOwnerId, setCarOwnerId] = useState('');
  const [carModelId, setCarModelId] = useState('');
  const navigate = useNavigate();
  
  const fetchCarOwners = async () => {
    try {
      await CarOwnerStore.fetchCarOwners();
      setCarOwners([...CarOwnerStore.carOwners]);
    } catch (error) {
      console.error('Fetching car owners failed:', error);
    }
  };
  
  const fetchCarModels = async () => {
    try {
      await CarModelStore.fetchCarModels();
      setCarModels([...CarModelStore.carModels]);
    } catch (error) {
      console.error('Fetching car models failed:', error);
    }
  };

  useEffect(() => {
    fetchCarOwners();
    fetchCarModels();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!carOwnerId || !carModelId || !registrationNumber.trim()) {
      alert('Please fill all fields and select both car owner and car model.');
      return;
    }
   
    const payload = {
      RegistrationNumber: registrationNumber.trim(),
      CarOwnerId: Number(carOwnerId),
      CarModelId: Number(carModelId)
    };

    const result = await CarRegistrationStore.addCarRegistration(payload);

    if (result?.error) {
      alert(`Adding car registration failed: ${result.message}`);
    } else {
      CarRegistrationStore.currentPage = 1;
      navigate(RouteNames.CAR_REGISTRATION_LIST);
    }
  };

  const handleCancel = () => {
    setRegistrationNumber('');
    setCarOwnerId('');
    setCarModelId('');
    CarRegistrationStore.currentPage = 1;
    navigate(RouteNames.CAR_REGISTRATION_LIST);
  };

  return (
    <div className="form-container">
      <h2>Add Car Registration</h2>

      <div className="form-group">
        <label htmlFor="registrationNumber">Registration Number</label>
        <input
          type="text"
          id="registrationNumber"
          value={registrationNumber}
          onChange={(e) => setRegistrationNumber(e.target.value)}
          placeholder="Enter registration number"
        />
      </div>

      <div className="form-group">
        <label htmlFor="carOwnerId">Car Owner</label>
        <Form.Select
          value={carOwnerId || ''}
          onChange={(e) => setCarOwnerId(Number(e.target.value))}
        >
          <option value="">Select a car owner</option>
          {carOwners.map((owner) => (
            <option key={owner.id} value={owner.id}>
              {owner.firstName} {owner.lastName}
            </option>
          ))}
        </Form.Select>
      </div>

      <div className="form-group">
        <label htmlFor="carModelId">Car Model</label>
        <Form.Select
          value={carModelId || ''}
          onChange={(e) => setCarModelId(Number(e.target.value))}
        >
          <option value="">Select a car model</option>
          {carModels.map((model) => (
            <option key={model.id} value={model.id}>
              {model.name}
            </option>
          ))}
        </Form.Select>
      </div>

      <div className="form-button-container">
        <button className="cancel-button" onClick={handleCancel}>
          Cancel
        </button>
        <button className="add-button" onClick={handleSubmit}>
          Submit
        </button>
      </div>
    </div>
  );
};

export default observer(CarRegistrationsAdd);