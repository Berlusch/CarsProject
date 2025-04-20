import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarRegistrationStore from "../../stores/CarRegistrationStore";
import { Form } from "react-bootstrap";
import CarOwnerService from "../../common/Services/CarOwnerService";


const CarRegistrationsEdit = observer(() => {  
  const navigate = useNavigate();
  const [registrationNumber, setRegistrationNumber] = useState("");
  const [carOwnerId, setCarOwnerId] = useState("");
  const [carOwners, setCarOwners] = useState([])
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();
  
  useEffect(() => {
    const fetchCarRegistration = async () => {              
  
      const result = await CarRegistrationStore.getCarRegistrationById(id);       
  
      if (result.error) {
        console.log('Error fetching Car Registration:', result.message);  
        setMessage("Car Registration not found.");
      } else {
        console.log('Fetched Car Registration:', result.message);  
        setRegistrationNumber(result.message.registrationNumber);
        setCarOwnerId(result.message.carOwnerId);
        setCarOwners(result.message.carOwners);
        
      }
      setLoading(false);
    };
  
    if (id) {
      fetchCarRegistration();
    } else {
      console.log('No ID provided');  
    }
  }, [id]);

  async function fetchCarOwners() {
        try {
          const response = await CarOwnerService.getCarOwnersPFS(1, 5, "name", "");
          if (Array.isArray(response) && response.length > 0) {
            setCarOwners(response);            
          } else {
            console.error("Data not available");
          }
        } catch (error) {
          console.error("Fetching error:", error);
        }
      }
  
      useEffect(() => {
        fetchCarOwners();  
      }, []); 
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    const result = await CarRegistrationStore.editCarRegistration
    (id, { registrationNumber, carOwnerId });
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
        onChange={(e) => setCarOwnerId(e.target.value)}
      >
        <option value="">Select a car owner</option>
        {carOwners && carOwners.map((s, index) => (
          <option key={index} value={s.id}>
          {s.firstName} {s.lastName}
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
