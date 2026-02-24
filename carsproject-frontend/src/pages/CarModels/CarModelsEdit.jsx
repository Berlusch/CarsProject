import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarModelStore from "../../stores/CarModelStore";
import CarMakeService from "../../common/Services/CarMakeService";
import CarEngineTypeService from "../../common/Services/CarEngineTypeService";
import { Form } from "react-bootstrap";

const CarModelsEdit = observer(() => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [name, setName] = useState("");
  const [abrv, setAbrv] = useState("");
  const [carMakes, setCarMakes] = useState([]);
  const [carEngineTypes, setCarEngineTypes] = useState([]);
  const [selectedCarMakeId, setSelectedCarMakeId] = useState("");
  const [carEngineTypeId, setCarEngineTypeId] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCarModel = async () => {
      try {
        const result = await CarModelStore.getCarModelById(id);
        if (result.error) {
          setMessage("Car Model not found.");
        } else {
          const model = result.message;
          setName(model.name);
          setAbrv(model.abrv);          
          CarModelStore.currentCarModel = model;
        }
      } catch (error) {
        console.error(error);
        setMessage("Error fetching car model.");
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchCarModel();
  }, [id]);
 
  useEffect(() => {
    const fetchCarMakes = async () => {
      try {
        const pfs = {
          paging: { pageNumber: 1, pageSize: 100 },
          sorting: { orderBy: "Name", descending: false },
          filter: { propertyName: "Name", filter: "" }
        };
        const response = await CarMakeService.getCarMakesPFS(pfs);
        if (response && Array.isArray(response.items)) {
          setCarMakes(response.items);
        }
      } catch (error) {
        console.error("Error fetching car makes:", error);
      }
    };
    fetchCarMakes();
  }, []);
  
  useEffect(() => {
    const fetchCarEngineTypes = async () => {
      try {
        const pfs = {
          paging: { pageNumber: 1, pageSize: 100 },
          sorting: { orderBy: "", descending: false },
          filter: { propertyName: "", filter: "" }
        };
        const response = await CarEngineTypeService.getCarEngineTypesPFS(pfs);
        if (response && Array.isArray(response.items)) {
          setCarEngineTypes(response.items);
        }
      } catch (error) {
        console.error("Error fetching car engine types:", error);
      }
    };
    fetchCarEngineTypes();
  }, []);
 
  useEffect(() => {
    if (!loading && carMakes.length > 0 && carEngineTypes.length > 0 && CarModelStore.currentCarModel) {
      const model = CarModelStore.currentCarModel;
      
      const make = carMakes.find(m => m.name === model.carMakeName);
      if (make) setSelectedCarMakeId(make.id);
      
      const engine = carEngineTypes.find(e => e.type === model.carEngineTypeType);
      if (engine) setCarEngineTypeId(engine.id);
    }
  }, [loading, carMakes, carEngineTypes]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!selectedCarMakeId) return alert("Please select a car make!");
    if (!carEngineTypeId) return alert("Please select a car engine type!");

    const result = await CarModelStore.editCarModel(id, {
      name,
      abrv,
      carMakeId: selectedCarMakeId,
      carEngineTypeId
    });

    setMessage(result.message);
    if (!result.error) navigate("/car-models");
  };

  const handleCancel = () => navigate("/car-models");

  if (loading) return <p>Loading...</p>;

  return (
    <div className="form-container">
      <h2>Edit Car Model</h2>
      {message && <p className="form-message">{message}</p>}

      <div className="form-group">
        <label htmlFor="name" className="small-label">Car Model</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="abrv" className="small-label">Car Model Abrv</label>
        <input
          type="text"
          id="abrv"
          value={abrv}
          onChange={(e) => setAbrv(e.target.value)}
        />
      </div>

      <Form.Group controlId='carMake'>
        <Form.Label className="small-label">Car Make</Form.Label>
        <Form.Select value={selectedCarMakeId} onChange={(e) => setSelectedCarMakeId(e.target.value)}>        
          {carMakes.map((s) => (
            <option key={s.id} value={s.id}>{s.name}</option>
          ))}
        </Form.Select>
      </Form.Group>

      <br />

      <Form.Group controlId='carEngineType'>
        <Form.Label className="small-label">Car Engine Type</Form.Label>
        <Form.Select value={carEngineTypeId} onChange={(e) => setCarEngineTypeId(e.target.value)}>        
          {carEngineTypes.map((s) => (
            <option key={s.id} value={s.id}>{s.type}</option>
          ))}
        </Form.Select>
      </Form.Group>

      <br />

      <div className="form-button-container">
        <button className="cancel-button" onClick={handleCancel}>Cancel</button>
        <button className="add-button" onClick={handleSubmit}>Submit</button>
      </div>
    </div>
  );
});

export default CarModelsEdit;