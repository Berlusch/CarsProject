import React, { useEffect, useState } from 'react';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import CarMakeService from '../../common/Services/CarMakeService';
import { observer } from 'mobx-react';
import { carMakeStore } from '../../stores/CarMakeStore';



export default function CarMakesList() {
  const [carMakes, setCarMakes] = useState([]);  
  

  async function fetchCarMakes() {
    const response = await CarMakeService.getCarMakesPFS(1, 5, "name", "");
    setCarMakes(response);    
  }

  function handleEdit(id) {
    console.log("Edit car make", id);
    
  }

  async function handleRemove(id) {
    const carMake = carMakes.items.find(c => c.id === id);
    const carMakeName = carMake.name;

    if (!confirm(`Are you sure you want to remove ${carMakeName}?`)) {
      return;
    }

    const response = await CarMakeService.remove(id);

    if (response.error) {
      alert(response.message);
      return;
    }

    fetchCarMakes();
  }

  useEffect(() => {
    fetchCarMakes();
  }, []);

  const columns = [
    { header: 'Name', accessor: 'name' },
    { header: 'Abrv', accessor: 'abrv' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
    
  ];

  const data = carMakes && carMakes.map(carMake => ({
    id: carMake.id, // Čuvamo id za kasniju upotrebu u akcijama
    name: carMake.name,
    abrv: carMake.abrv,
    edit: ( // Dodavanje kolone za edit
      <button className="edit-button" onClick={() => handleEdit(carMake.id)}>
        <i className="fas fa-edit"></i> {/* Ikona za Edit */}
      </button>
    ),
    remove: ( // Dodavanje kolone za remove
      <button className="delete-button" onClick={() => handleRemove(carMake.id)}>
        <i className="fas fa-trash"></i> {/* Ikona za Delete */}
      </button>
    ),
    
  })); []
  

  return (
    <Table
      columns={columns}
      data={data}
      onEdit={handleEdit}
      onRemove={handleRemove}
      onAdd={() => console.log('Add new car make')}
      routeNames={RouteNames.CAR_MAKE_ADD}
      entityName="Car Make"
    />
  );
}