import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarRegistrationStore from "../../stores/CarRegistrationStore";
import CarOwnerService from "../../common/Services/CarOwnerService";
import CarModelService from "../../common/Services/CarModelService";
import { Form } from "react-bootstrap";

const CarRegistrationsEdit = observer(() => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [registrationNumber, setRegistrationNumber] = useState("");
  const [carOwners, setCarOwners] = useState([]);
  const [carOwnerId, setCarOwnerId] = useState("");
  const [carModels, setCarModels] = useState([]);
  const [carModelId, setCarModelId] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);

  
  const [currentRegistration, setCurrentRegistration] = useState(null);


  const fetchCarOwners = async () => {
    try {
      const pfs = { paging: { pageNumber: 1, pageSize: 100 }, sorting: { orderBy: "LastName", descending: false }, filter: { propertyName: "LastName", filter: "" } };
      const response = await CarOwnerService.getCarOwnersPFS(pfs);
      if (Array.isArray(response.items)) setCarOwners(response.items);
    } catch (error) {
      console.error(error);
    }
  };

  const fetchCarModels = async () => {
    try {
      const pfs = { paging: { pageNumber: 1, pageSize: 100 }, sorting: { orderBy: "Name", descending: false }, filter: { propertyName: "Name", filter: "" } };
      const response = await CarModelService.getCarModelsPFS(pfs);
      if (Array.isArray(response.items)) setCarModels(response.items);
    } catch (error) {
      console.error(error);
    }
  };


  const fetchCarRegistration = async () => {
    try {
      const reg = await CarRegistrationStore.getCarRegistrationById(id);
      if (reg.error) {
        setMessage("Car Registration not found.");
        return;
      }
      setRegistrationNumber(reg.message.registrationNumber);
      setCurrentRegistration(reg.message);
    } catch (error) {
      console.error(error);
      setMessage("Error fetching registration.");
    }
  };

  const fetchInitialData = async () => {
    await fetchCarOwners();
    await fetchCarModels();
    await fetchCarRegistration();
    setLoading(false);
  };

  useEffect(() => {
    fetchInitialData();
  }, []);


  useEffect(() => {
    if (!loading && carOwners.length > 0 && carModels.length > 0 && currentRegistration) {
      
      const owner = carOwners.find(o => `${o.firstName} ${o.lastName}` === currentRegistration.carOwnerFirstNameLastName);
      if (owner) setCarOwnerId(owner.id);

      const model = carModels.find(m => m.name === currentRegistration.carModelName);
      if (model) setCarModelId(model.id);
    }
  }, [loading, carOwners, carModels, currentRegistration]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!carOwnerId) return alert("Please select a car owner!");
    if (!carModelId) return alert("Please select a car model!");

    const result = await CarRegistrationStore.editCarRegistration(id, {
      registrationNumber,
      carOwnerId: parseInt(carOwnerId),
      carModelId: parseInt(carModelId),
    });

    setMessage(result.message);
    if (!result.error) navigate("/car-registrations");
  };

  const handleCancel = () => navigate("/car-registrations");

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

      <Form.Group controlId="carOwner">
        <Form.Label>Car Owner</Form.Label>
        <Form.Select value={carOwnerId} onChange={(e) => setCarOwnerId(e.target.value)}>
          <option value="">Select a car owner</option>
          {carOwners.map((owner) => (
            <option key={owner.id} value={owner.id}>
              {owner.firstName} {owner.lastName}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <br />

      <Form.Group controlId="carModel">
        <Form.Label>Car Model</Form.Label>
        <Form.Select value={carModelId} onChange={(e) => setCarModelId(e.target.value)}>
          <option value="">Select a car model</option>
          {carModels.map((model) => (
            <option key={model.id} value={model.id}>
              {model.name}
            </option>
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

export default CarRegistrationsEdit;