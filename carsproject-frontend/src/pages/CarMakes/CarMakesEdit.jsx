import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import CarMakeStore from "../../stores/CarMakeStore";

const CarMakesEdit = observer(() => {  
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [abrv, setAbrv] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  // Dohvati podatke za carMake prema id-u
  useEffect(() => {
    const fetchCarMake = async () => {              
  
      const result = await CarMakeStore.getCarMakeById(id);       
  
      if (result.error) {
        console.log('Error fetching Car Make:', result.message);  // Ispis poruke o grešci
        setMessage("Car Make not found.");
      } else {
        console.log('Fetched Car Make:', result.message);  // Ispis podataka ako su uspješno dohvaćeni
        setName(result.message.name);
        setAbrv(result.message.abrv);
      }
      setLoading(false);
    };
  
    if (id) {
      fetchCarMake();
    } else {
      console.log('No ID provided');  // Ako ID nije dostupan
    }
  }, [id]);
  

  const handleSubmit = async (e) => {
    e.preventDefault();
    const result = await CarMakeStore.editCarMake(id, { name, abrv });
    setMessage(result.message);
    if (!result.error) {
      navigate("/car-makes");
    }
  };

  const handleCancel = () => {
    navigate("/car-makes");
  };

  if (loading) return <p>Loading...</p>; // Prikazuje se "Loading..." dok se podaci ne učitaju

  return (
    <div className="form-container">
      <h2>Edit Car Make</h2>

      {message && <p className="form-message">{message}</p>}

      <div className="form-group">
        <label htmlFor="name">Name</label>
        <input
          type="text"
          id="name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Enter name"
        />
      </div>

      <div className="form-group">
        <label htmlFor="abrv">Abbreviation</label>
        <input
          type="text"
          id="abrv"
          value={abrv}
          onChange={(e) => setAbrv(e.target.value)}
          placeholder="Enter abbreviation"
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

export default CarMakesEdit;
