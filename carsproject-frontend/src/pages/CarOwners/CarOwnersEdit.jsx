import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarOwnerStore from "../../stores/CarOwnerStore";

const CarOwnersEdit = observer(() => {  
  const navigate = useNavigate();
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [dateOfBirth, setDateOfBirth]= useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  
  useEffect(() => {
    const fetchCarOwner = async () => {              
  
      const result = await CarOwnerStore.getCarOwnerById(id);       
  
      if (result.error) {        
        setMessage("Car Owner not found.");
      } else {         
        setFirstName(result.message.firstName);
        setLastName(result.message.lastName);
        setDateOfBirth(result.message.dateOfBirth)
      }
      setLoading(false);
    };
  
    if (id) {
      fetchCarOwner();
    }  
    
  }, [id]);
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    const result = await CarOwnerStore.editCarOwner(id, { firstName, lastName, dateOfBirth });
    setMessage(result.message);
    if (!result.error) {
      navigate("/car-owners");
    }
  };

  const handleCancel = () => {
    navigate("/car-owners");
  };

  if (loading) return <p>Loading...</p>; 

  return (
    <div className="form-container">
      <h2>Edit Car Owner</h2>

      {message && <p className="form-message">{message}</p>}

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
          placeholder="Enter date of birth"
        />
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
});

export default CarOwnersEdit;
