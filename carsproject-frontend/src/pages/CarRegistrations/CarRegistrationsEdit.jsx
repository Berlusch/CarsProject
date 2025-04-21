import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarRegistrationStore from "../../stores/CarRegistrationStore";
import CarOwnerService from "../../common/Services/CarOwnerService";
import CarModelService from "../../common/Services/CarModelService";
import { Form } from "react-bootstrap";

const CarRegistrationsEdit = observer(() => {  
  const navigate = useNavigate();
  const [carRegistration, setCarRegistration] = useState("");
  const [registrationNumber, setRegistrationNumber] = useState("");
  const [carOwner, setCarOwner] = useState("");
  const [carOwners, setCarOwners] = useState("");
  const [carOwnerId, setCarOwnerId] = useState("");
  const [carOwnerFirstNameLastName, setCarOwnerFirstNameLastName] = useState("")
  const [carModel, setCarModel] = useState("");
  const [carModels, setCarModels] = useState("");
  const [carModelId, setCarModelId] = useState("");
  const [carModelName, setCarModelName] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  async function fetchCarOwners() {
          try {
            const response = await CarOwnerService.getCarOwnersPFS(1, 100, "last name", "");
            if (Array.isArray(response) && response.length > 0) {
              setCarOwners(response);            
            } else {
              console.error("Data not available");
            }
          } catch (error) {
            console.error("Fetching error:", error);
          }
        }
         
  async function fetchCarModels() {
          try {
            const response = await CarModelService.getCarModelsPFS(1, 100, "name", "");
            if (Array.isArray(response) && response.length > 0) {
              setCarModels (response);            
            } else {
              console.error("Data not available");
            }
          } catch (error) {
            console.error("Fetching error:", error);
          }
        }        
          
      
   
    async function fetchCarRegistration() {              
        const result = await CarRegistrationStore.getCarRegistrationById(id);       
  
      if (result.error) {
        console.log('Error fetching Car Registration:', result.message);  // Ispis poruke o grešci
        setMessage("Car Registration not found.");
      } else {
        console.log('Fetched Car Registration:', result.message);  // Ispis podataka ako su uspješno dohvaćeni
                        
      }
      setLoading(false);
      let p = result.message
        setCarRegistration(p);
        setRegistrationNumber(p.registrationNumber);
        setCarOwnerId(p.carOwnerId);  
        setCarModelId(p.carModelId);        
      
    };    
    

  async function fetchInitialData() {
    await fetchCarOwners();
    await fetchCarModels();
    await fetchCarRegistration();
  }
 
useEffect(()=>{
    fetchInitialData();        
},[]);
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    const result = await CarRegistrationStore.editCarRegistration
    (id, { registrationNumber, carOwnerId: parseInt(carOwnerId), carModelId: parseInt(carModelId)});
    setMessage(result.message);
    if (!result.error) {
      navigate("/car-registrations");
    }
  };

  const handleCancel = () => {
    navigate("/car-registrations");
  };

  if (loading) return <p>Loading...</p>; 

  return (
    <div className="form-container">
      
    <h2>Edit Car Registration</h2>

      {message && <p className="form-message">{message}</p>}

      <div className="form-group">
        <label htmlFor="registrationNumber">Registration Number</label>
        <input
          type="text"
          id="registrationNumber"
          value={registrationNumber}
          onChange={(e) => setRegistrationNumber(e.target.value)}
          
        />
      </div>

      <Form.Select 
        value={carOwnerId}
        onChange={(e) => {setCarOwnerId(e.target.value)}}
      >     
        {carOwners && carOwners.map((s, index) => (
          <option key={index} value={s.id}>
          {s.firstName} {s.lastName}
            </option>
        ))}
      </Form.Select>
      <br/>

      <Form.Select 
        value={carModelId}
        onChange={(e) => {setCarModelId(e.target.value)}}
      >    
        {carModels && carModels.map((s, index) => (
          <option key={index} value={s.id}>
          {s.name}
            </option>
        ))}
      </Form.Select>

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
});

export default CarRegistrationsEdit;
