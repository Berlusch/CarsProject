import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import Table from '../../components/Table';
import CarMakeStore from '../../stores/CarMakeStore';
import { RouteNames } from '../../common/constants';

const CarMakesList = () => {
  const {
    carMakes,
    fetchCarMakes,
    removeCarMake,
    isLoading,
    error
  } = CarMakeStore;

  useEffect(() => {
    fetchCarMakes();
  }, []);

  const columns = [
    { header: 'Name', accessor: 'Name' },
    { header: 'Abrv', accessor: 'Abrv' },
    { header: 'Edit', accessor: 'Edit' },
    { header: 'Remove', accessor: 'Remove' }
  ];

  const handleEdit = (id) => {
    console.log("Edit car make", id);
    // Dodaj navigaciju ili logiku za uređivanje
  };

  const handleRemove = async (id) => {
    await removeCarMake(id);
  };

  if (isLoading) return <p>Učitavanje podataka...</p>;
  if (error) return <p>Greška pri dohvaćanju: {error}</p>;

  return (
    <Table
      columns={columns}
      data={carMakes}
      onEdit={handleEdit}
      onRemove={handleRemove}
      onAdd={() => console.log('Add new car make')}
      routeNames={RouteNames.CARMAKE_ADD}
    />
  );
};

export default observer(CarMakesList);
