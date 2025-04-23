import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarModelStore from "../../stores/CarModelStore";
import CarMakeService from "../../common/Services/CarMakeService";
import CarEngineTypeService from "../../common/Services/CarEngineTypeService";
import { Form } from "react-bootstrap";

const CarModelsEdit = observer(() => {  
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [abrv, setAbrv] = useState("");  
  const [carMakes, setCarMakes] = useState("");
  const [carMakeId, setCarMakeId] = useState("");  
  const [carEngineTypes, setCarEngineTypes] = useState("");
  const [carEngineTypeId, setCarEngineTypeId] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  
  useEffect(() => {
    const fetchCarModel = async () => {              
  
      const result = await CarModelStore.getCarModelById(id);       
  
      if (result.error) {        
        setMessage("Car Model not found.");
      } else {
        setName(result.message.name);
        setAbrv(result.message.abrv);               
      }
      setLoading(false);
    };
  
    if (id) {
      fetchCarModel();
    } 
    
  }, [id]);

  async function fetchCarMakes() {
          try {
            const response = await CarMakeService.getCarMakesPFS(1, 100, "name", "");
            if (Array.isArray(response) && response.length > 0) {
              setCarMakes(response);            
            } else {
              console.error("Data not available");
            }
          } catch (error) {
            console.error("Fetching error:", error);
          }
        }
  
    async function fetchCarEngineTypes() {
          try {
            const response = await CarEngineTypeService.getCarEngineTypesListOnly();
            if (Array.isArray(response) && response.length > 0) {
              setCarEngineTypes (response);            
            } else {
              console.error("Data not available");
            }
          } catch (error) {
            console.error("Fetching error:", error);
          }
        }
    
        useEffect(() => {
          fetchCarMakes();
          fetchCarEngineTypes();  
        }, [id]); 
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!carMakeId) {
      alert("Please select a car make!");
      return;
    }
    if (!carEngineTypeId) {
      alert("Please select a car engine type!");
      return;
    }
    const result = await CarModelStore.editCarModel
    (id, { name, abrv, carMakeId, carEngineTypeId});
    setMessage(result.message);
    if (!result.error) {
      navigate("/car-models");
    }
  };

  const handleCancel = () => {
    navigate("/car-models");
  };

  if (loading) return <p>Loading...</p>; 

  return (
    <div className="form-container">
      
    <h2>Edit Car Model</h2>

      {message && <p className="form-message">{message}</p>}

      <div className="form-group">
        <label htmlFor="name">Car Model</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          
        />
      </div>

      <div className="form-group">
        <label htmlFor="abrv">Car Model Abrv</label>
        <input
          type="text"
          id="abrv"
          value={abrv}
          onChange={(e) => setAbrv(e.target.value)}
          
        />
      </div>

      <Form.Select 
        value={carMakeId}
        onChange={(e) => setCarMakeId(e.target.value)}
      >
        <option value="">Select a car make</option>
        {carMakes && carMakes.map((s, index) => (
          <option key={index} value={s.id}>
          {s.name}
            </option>
        ))}
      </Form.Select>
      <br/>

      <Form.Select 
        value={carEngineTypeId}
        onChange={(e) => setCarEngineTypeId(e.target.value)}
      >
        <option value="">Select a car engine Type</option>
        {carEngineTypes && carEngineTypes.map((s, index) => (
          <option key={index} value={s.id}>
          {s.type}
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

export default CarModelsEdit;
