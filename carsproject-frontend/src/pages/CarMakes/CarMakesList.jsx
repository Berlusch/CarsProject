import React, { useEffect, useState } from 'react';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import CarMakeService from '../../common/Services/CarMakeService';

export default function CarMakesList() {
  const [carMakes, setCarMakes] = useState({ items: [] });

  async function fetchCarMakes() {
    const response = await CarMakeService.getCarMakesPFS(1, 5, "name", "");
    setCarMakes(response);
  }

  function handleEdit(id) {
    console.log("Edit car make", id);
    // Dodaj logiku za uređivanje ako želiš
  }

  async function handleRemove(id) {
    const carMake = carMakes.items.find(c => c.id === id);
    const carMakeName = carMake?.name;

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
    { header: 'Name', accessor: 'Name' },
    { header: 'Abrv', accessor: 'Abrv' },
    { header: 'Edit', accessor: 'Edit' },
    { header: 'Remove', accessor: 'Remove' }
  ];

  const data = carMakes.items?.map(carMake => ({
    id: carMake.id,
    Name: carMake.name,
    Abrv: carMake.abrv,
    Edit: carMake.id,
    Remove: carMake.id
  })) || [];

  return (
    <Table
      columns={columns}
      data={data}
      onEdit={handleEdit}
      onRemove={handleRemove}
      onAdd={() => console.log('Add new car make')}
      routeNames={RouteNames.CAR_MAKE_ADD}
    />
  );
}
