import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarModelStore from "../../stores/CarModelStore";
import { Form } from "react-bootstrap";
import CarMakeService from "../../common/Services/CarMakeService";
import CarEngineTypeService from "../../common/Services/CarEngineTypeService";


const CarModelsEdit = observer(() => {  
  const navigate = useNavigate();
  const [modelName, setModelName] = useState("");
  const [modelAbrv, setModelAbrv] = useState("");
  const [carMakeId, setCarMakeId] = useState("");
  const [carMake, setCarMake] = useState("")
  const [carMakes, setCarMakes] = useState([])
  const [carEngineTypeId, setCarEngineTypeId] = useState("");
  const [carEngineType, setCarEngineType] = useState("")
  const [carEngineTypes, setCarEngineTypes] = useState([])
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  
  useEffect(() => {
    const fetchCarModel = async () => {              
  
      const result = await CarModelStore.getCarModelById(id);     
      console.log("ID iz URL-a:", id); 
  
      if (result.error) {
        console.log('Error fetching Car Model:', result.message);  
        setMessage("Car Model not found.");
      } else {
        console.log('Fetched Car Model:', result.message); 
        
        setModelName(result.message.name);
        setModelAbrv(result.message.abrv);
        setCarMake(result.message.carMake);
        setCarMakeId(result.message.parseInt(carMake.id));  
        setCarEngineType(result.message.carEngineType);
        setCarEngineTypeId(result.message.parseInt(carEngineType.id));             
        
      }
      setLoading(false);
    };
  
    if (id) {
      fetchCarModel();
    } else {
      console.log('No ID provided');  
    }
  }, [id]);

  async function fetchCarMakes() {
        try {
          const response = await CarMakeService.getCarMakesPFS(1, 5, "name", "");
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
    const result = await CarModelStore.editCarModel
    (modelName, modelAbrv, carMakeId,
        carEngineTypeId);
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
        <label htmlFor="modelName">Model Name</label>
        <input
          type="text"
          id="modelName"
          value={modelName}
          onChange={(e) => setModelName(e.target.value)}
          
        />
      </div>

      <div className="form-group">
        <label htmlFor="modelAbrv">Model Abrv</label>
        <input
          type="text"
          id="modelAbrv"
          value={modelAbrv}
          onChange={(e) => setModelAbrv(e.target.value)}
          
        />
      </div>

      <Form.Select 
        value={carMakeId}
        onChange={(e) => setCarMakeId(e.target.value)}
      >
        <option value="">{carMake}</option>
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
        <option value=""></option>
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
