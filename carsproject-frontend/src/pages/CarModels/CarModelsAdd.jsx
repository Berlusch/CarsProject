import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form } from 'react-bootstrap';
import { RouteNames } from '../../common/constants';
import CarModelStore from '../../stores/CarModelStore';
import CarMakeStore from '../../stores/CarMakeStore';
import CarEngineTypeStore from '../../stores/CarEngineTypeStore';

const CarModelsAdd = () => {
  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [carMakes, setCarMakes] = useState([]);
  const [carEngineTypes, setCarEngineTypes] = useState([]);
  const [carMakeId, setCarMakeId] = useState('');
  const [carEngineTypeId, setCarEngineTypeId] = useState('');
  const navigate = useNavigate();
 
  const fetchCarMakes = async () => {
    try {
      await CarMakeStore.fetchCarMakes();
      setCarMakes([...CarMakeStore.carMakes]);      
    } catch (error) {
      console.error('Fetching car makes failed:', error);
    }
  };

  const fetchCarEngineTypes = async () => {
    try {
      await CarEngineTypeStore.fetchCarEngineTypes();
      setCarEngineTypes([...CarEngineTypeStore.carEngineTypes]);      
    } catch (error) {
      console.error('Fetching car engine types failed:', error);
    }
  };

  useEffect(() => {
    fetchCarMakes();
    fetchCarEngineTypes();
  }, []);
  
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!carMakeId || !carEngineTypeId) {
      alert('Please select both a car make and an engine type.');
      return;
    }

    console.log('Submitting DTO:', {
      name,
      abrv,
      carMakeId,
      carEngineTypeId,
    });

    await CarModelStore.addCarModel(
      name,
      abrv,
      parseInt(carMakeId),
      parseInt(carEngineTypeId)
    );

    if (CarModelStore.addStatus.error) {
      alert('Adding car model failed.');
    } else {
      CarModelStore.currentPage = 1;
      navigate(RouteNames.CAR_MODEL_LIST);
    }
  };

  const handleCancel = () => {
    setName('');
    setAbrv('');
    setCarMakeId('');
    setCarEngineTypeId('');
    CarModelStore.currentPage = 1;
    navigate(RouteNames.CAR_MODEL_LIST);
  };

  return (
    <div className="form-container">
      <h2>Add Car Model</h2>

      <div className="form-group">
        <label htmlFor="name">Car Model</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Enter a car model name"
        />
      </div>

      <div className="form-group">
        <label htmlFor="abrv">Car Model Abrv</label>
        <input
          type="text"
          id="abrv"
          value={abrv}
          onChange={(e) => setAbrv(e.target.value)}
          placeholder="Enter a car model abrv"
        />
      </div>

      <div className="form-group">
        <label htmlFor="carMakeId">Car Make</label>
        <Form.Select
          value={carMakeId || ''}
          onChange={(e) => setCarMakeId(Number(e.target.value))}
        >
          <option value="">Select a car make</option>
          {carMakes.map((make) => (
            <option key={make.id} value={make.id}>
              {make.name}
            </option>
          ))}
        </Form.Select>
      </div>

      <div className="form-group">
        <label htmlFor="carEngineTypeId">Car Engine Type</label>
        <Form.Select
          value={carEngineTypeId || ''}
          onChange={(e) => setCarEngineTypeId(Number(e.target.value))}
        >
          <option value="">Select a car engine type</option>
          {carEngineTypes.map((type) => (
            <option key={type.id} value={type.id}>
              {type.type}
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

export default observer(CarModelsAdd);