import { observer } from 'mobx-react-lite';
import CarModelStore from '../../stores/CarModelStore';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form } from 'react-bootstrap';
import { RouteNames } from '../../common/constants';
import CarMakeService from '../../common/Services/CarMakeService';
import CarEngineTypeService from '../../common/Services/CarEngineTypeService';


const CarModelsAdd = () => {
  
  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [carMakes, setCarMakes] = useState([]);    
  const [carEngineTypes, setCarEngineTypes] = useState([]);
  const [carMakeId, setCarMakeId] = useState('');    
  const [carEngineTypeId, setCarEngineTypeId] = useState('');  
  const navigate = useNavigate();

  async function fetchCarEngineTypes() {
    try {
      const response = await CarEngineTypeService.getCarEngineTypesListOnly();
      if (Array.isArray(response) && response.length > 0) {
        setCarEngineTypes(response);
        setCarEngineTypeId(response[0].id); 
      } else {
        console.error("Data not available");
      }
    } catch (error) {
      console.error("Fetching error:", error);
    }
  }
  

    async function fetchCarMakes() {
      try {
        const response = await CarMakeService.getCarMakesPFS(1, 100, "name", "");
          if (Array.isArray(response) && response.length > 0) {
          setCarMakes(response);
          setCarMakeId(response.id)
        } else {
          console.error("Data not available");
        }
      } catch (error) {
        console.error("Fetching error:", error);
      }
    }

    useEffect(()=>{
      fetchCarMakes();  
      fetchCarEngineTypes();     
     
    },[]);
   

    const handleSubmit = async (e) => {
      e.preventDefault();   
                    
      await CarModelStore.addCarModel(name, abrv, (parseInt(carMakeId)), (parseInt(carEngineTypeId)));
    
      if (CarModelStore.addStatus.error) {
        alert('Adding item failed.');
      } else {
        CarModelStore.currentPage = 1;
        navigate(RouteNames.CAR_MODEL_LIST);
      }
    };
    

  const handleCancel = () => {    
    setCarModelName('');
    setCarModelAbrv('');
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
            onChange={(e)=>{setCarMakeId(e.target.value)}}
            >
            <option value="">Select a car make</option>
            {carMakes && carMakes.map((s,index)=>(
              <option key={index} value={s.id}>
                {s.name}
              </option>
            ))}
            </Form.Select>
      </div>

      <div className="form-group">
        <label htmlFor="carEngineTypeId">Car Engine Type</label>
        <Form.Select 
            onChange={(e)=>{setCarEngineTypeId(e.target.value)}}
            >
            <option value="">Select a car engine type</option>            
            {carEngineTypes && carEngineTypes.map((s,index)=>(
              <option key={index} value={s.id}>
                {s.type}
              </option>
            ))}
            </Form.Select>
      </div>

      <div className="form-button-container">
        <button className="cancel-button" onClick={handleCancel}>Cancel</button>
        <button className="add-button" onClick={handleSubmit}>Submit</button>
      </div>
    </div>
  );
};

export default observer(CarModelsAdd);
