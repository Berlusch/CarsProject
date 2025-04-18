import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react';
import CarMakeStore from '../../stores/CarMakeStore';
import CarMakeService from '../../common/Services/CarMakeService';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';

const CarMakesList = observer(() => {
  const [carMakes, setCarMakes] = useState([]);

  const fetchCarMakes = async () => {
    const { currentPage, pageSize, searchTerm } = CarMakeStore.filters;

    const response = await CarMakeService.getCarMakesPFS(currentPage, pageSize, "name", searchTerm);
    setCarMakes(response);  // Spremi dohvaćene podatke
    
  };

  useEffect(() => {
    fetchCarMakes();
  }, [CarMakeStore.filters]); // Pretraga, stranica izazivaju ponovno dohvaćanje podataka

  const handleSearch = (term) => {
    CarMakeStore.setSearchTerm(term); // Postavi pretragu i resetiraj stranicu
  };

  const handlePageChange = (page) => {
    CarMakeStore.setPage(page); // Promjena stranice
  };

  const columns = [
    { header: 'Name', accessor: 'name' },
    { header: 'Abrv', accessor: 'abrv' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carMakes && carMakes.map(carMake => ({
    id: carMake.id,
    name: carMake.name,
    abrv: carMake.abrv,
    edit: (
      <button className="edit-button" onClick={() => handleEdit(carMake.id)}>
        <i className="fas fa-edit"></i>
      </button>
    ),
    remove: (
      <button className="delete-button" onClick={() => handleRemove(carMake.id)}>
        <i className="fas fa-trash"></i>
      </button>
    ),
  }));

  const handleEdit = (id) => {
    console.log("Edit car make", id);
  };

  const handleRemove = async (id) => {
    const carMake = carMakes.find(c => c.id === id);
    const carMakeName = carMake.name;

    if (!confirm(`Are you sure you want to remove ${carMakeName}?`)) {
      return;
    }

    const response = await CarMakeService.remove(id);

    if (response.error) {
      alert(response.message);
      return;
    }

    fetchCarMakes();  // Ponovno dohvati podatke nakon brisanja
  };

  return (
    <div>
      <SearchBox
      value={CarMakeStore.searchTerm}  // Poveži vrijednost sa store-om
         onChange={(value) => CarMakeStore.setSearchTerm(value)}  // Ažurira searchTerm
         onSearch={handleSearch}  // Pokreće pretragu
      />
      <Table
        columns={columns}
        data={data}
        onEdit={handleEdit}
        onRemove={handleRemove}
        onAdd={() => console.log('Add new car make')}
        routeNames={RouteNames.CAR_MAKE_ADD}
        entityName="Car Make"
      />
      <Pagination
        currentPage={CarMakeStore.currentPage}
        totalPages={Math.ceil(CarMakeStore.totalCount / CarMakeStore.pageSize)}
        onPageChange={handlePageChange}
      />
    </div>
  );
});

export default CarMakesList;
