import React from 'react';
import Table from '../../components/Table.jsx';


const columns = [
  { header: 'ID', accessor: 'id' },
  { header: 'Marka', accessor: 'make' },
  { header: 'Model', accessor: 'model' },
  { header: 'Godina', accessor: 'year' }
];

const data = [
  { id: 1, make: 'Toyota', model: 'Corolla', year: 2020 },
  { id: 2, make: 'Volkswagen', model: 'Golf', year: 2019 },
  { id: 3, make: 'Ford', model: 'Focus', year: 2021 },
  { id: 4, make: 'BMW', model: 'X5', year: 2022 }
];

const Cars = () => {
  return (
    <div>
      <h2>Cars List</h2>
      <Table columns={columns} data={data} />
    </div>
  );
};

export default Cars;
