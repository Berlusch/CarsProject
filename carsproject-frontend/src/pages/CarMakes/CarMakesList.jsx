import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import CarMakeStore from '../../stores/CarMakeStore';
import CarMakeService from '../../common/Services/CarMakeService';
import Table from '../../components/Table';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { RouteNames } from '../../common/constants';
import React, { useEffect } from 'react';

const CarMakesList = observer(() => {
  const navigate = useNavigate();
 
  useEffect(() => {
    CarMakeStore.fetchCarMakes();
  }, []);

  const handleSearch = (term) => {
    CarMakeStore.setSearchTerm(term);
    CarMakeStore.fetchCarMakes(); 
  };

  const handlePageChange = (page) => {
  CarMakeStore.setPage(page); 
};

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_MAKE_EDIT.replace(':id', id));
  };

  const handleRemove = async (id) => {
    const carMake = CarMakeStore.carMakes.find(c => c.id === id);
    if (!carMake) return;

    if (!confirm(`Are you sure you want to remove ${carMake.name}?`)) {
      return;
    }

    const response = await CarMakeService.remove(id);

    if (response?.error) {
      alert(response.message);
      return;
    }

    CarMakeStore.fetchCarMakes(); 
  };

  const columns = [
    { header: 'Name', accessor: 'name' },
    { header: 'Abrv', accessor: 'abrv' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = CarMakeStore.carMakes.map(carMake => ({
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
    )
  }));

  return (
    <div>
      <header className="entityName">Car Makes</header>

      <SearchBox
        value={CarMakeStore.searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by car make..."
      />

      <Table
        columns={columns}
        data={data}
        routeNames={RouteNames.CAR_MAKE_ADD}
        entityName="Car Make"
        page={CarMakeStore.currentPage}
      />

      <Pagination
        currentPage={CarMakeStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarMakeStore.hasNextPage}
      />
    </div>
  );
});

export default CarMakesList;
