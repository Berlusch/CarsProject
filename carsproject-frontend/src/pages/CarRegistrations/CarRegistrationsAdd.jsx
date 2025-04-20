import { observer } from 'mobx-react-lite';
import CarRegistrationStore from '../../stores/CarRegistrationStore';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form } from 'react-bootstrap';
import { RouteNames } from '../../common/constants';
import CarOwnerService from '../../common/Services/CarOwnerService';
//import CarModelService from '../../common/Services/CarModelService'


const CarRegistrationsAdd = () => {
  const [registrationNumber, setRegistrationNumber] = useState('');
  const [carOwners, setCarOwners] = useState([]);    
  //const [carModels, setCarModels] = useState([]);
  const [carOwnerId, setCarOwnerId] = useState("Select a car owner");    
  //const [carModelId, setCarModelId] = useState([]);
  const navigate = useNavigate();

  /*async function fetchCarModels() {
      try {
        const response = await CarModelService.getCarModelsPFS(1, 5, "name", "");
        if (Array.isArray(response) && response.length > 0) {
          setCarModels(response);
          setCarModelId(response.id)
        } else {
          console.error("Data not available");
        }
      } catch (error) {
        console.error("Fetching error:", error);
      }
    }*/

    async function fetchCarOwners() {
      try {
        const response = await CarOwnerService.getCarOwnersPFS(1, 5, "name", "");
        if (Array.isArray(response) && response.length > 0) {
          setCarOwners(response);
          setCarOwnerId(response.id)
        } else {
          console.error("Data not available");
        }
      } catch (error) {
        console.error("Fetching error:", error);
      }
    }

    useEffect(()=>{
      fetchCarOwners();  
      //fetchCarModels();     
     
    },[]);
   

  const handleSubmit = async (e) => {
    e.preventDefault();
    await CarRegistrationStore.addCarRegistration(registrationNumber, carOwnerId); 
    if (CarRegistrationStore.addStatus.error) {
      alert('Adding item failed.');
    } else {
      CarRegistrationStore.currentPage = 1;
      navigate(RouteNames.CAR_REGISTRATION_LIST);
    }
    
  };

  const handleCancel = () => {    
    setRegistrationNumber('');
    setCarOwnerId('');
    //setCarModelId('');
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
        <label htmlFor="carOwner">Car Owner</label>
        <Form.Select 
            onChange={(e)=>{setCarOwnerId(e.target.value)}}
            >
            <option value="">Select a car owner</option>
            {carOwners && carOwners.map((s,index)=>(
              <option key={index} value={s.id}>
                {s.firstName}{s.lastName}
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

export default observer(CarRegistrationsAdd);
