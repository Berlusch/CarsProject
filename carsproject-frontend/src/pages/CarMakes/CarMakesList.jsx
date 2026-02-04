import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react';
import CarMakeStore from '../../stores/CarMakeStore';
import CarMakeService from '../../common/Services/CarMakeService';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { useNavigate } from 'react-router-dom';

const CarMakesList = observer(() => {
  const navigate = useNavigate();

  const [carMakes, setCarMakes] = useState([]);
  const [currentPageSize, setCurrentPageSize] = useState(0);

  const { currentPage, pageSize, searchTerm } = CarMakeStore.filters;
  
  const fetchCarMakes = async () => {
    const { currentPage, pageSize, searchTerm } = CarMakeStore.filters;

    const pfs = {
      paging: { pageNumber: currentPage, pageSize },
      sorting: { orderBy: "name", descending: false },
      filter: { propertyName: "name", filter: searchTerm || "" }
    };

    const response = await CarMakeService.getCarMakesPFS(pfs);

    const data = Array.isArray(response)
      ? response
      : response?.data ?? [];

    setCarMakes(data);
    setCurrentPageSize(data.length);
  };

  useEffect(() => {
  CarMakeStore.fetchCarMakes();
}, [CarMakeStore.currentPage, CarMakeStore.pageSize, CarMakeStore.searchTerm]);

  const handleSearch = (term) => {
    CarMakeStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarMakeStore.setPage(page);
  };

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_MAKE_EDIT.replace(':id', id));
  };

  const handleRemove = async (id) => {
    const carMake = carMakes.find(c => c.id === id);
    if (!carMake) return;

    if (!confirm(`Are you sure you want to remove ${carMake.name}?`)) {
      return;
    }

    const response = await CarMakeService.remove(id);

    if (response?.error) {
      alert(response.message);
      return;
    }

    fetchCarMakes();
  };

  const hasNextPage = currentPageSize === pageSize;

  const columns = [
    { header: 'Name', accessor: 'name' },
    { header: 'Abrv', accessor: 'abrv' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carMakes.map(carMake => ({
    id: carMake.id,
    name: carMake.name,
    abrv: carMake.abrv,
    edit: (
      <button
        className="edit-button"
        onClick={() => handleEdit(carMake.id)}
      >
        <i className="fas fa-edit"></i>
      </button>
    ),
    remove: (
      <button
        className="delete-button"
        onClick={() => handleRemove(carMake.id)}
      >
        <i className="fas fa-trash"></i>
      </button>
    )
  }));

  return (
    <div>
      <header className="entityName">Car Makes</header>

      <SearchBox
        value={searchTerm}
        onChange={(value) => CarMakeStore.setSearchTerm(value)}
        onSearch={handleSearch}
        placeholder="Search by car make..."
      />

      <Table
        columns={columns}
        data={data}
        onEdit={handleEdit}
        onRemove={handleRemove}
        routeNames={RouteNames.CAR_MAKE_ADD}
        entityName="Car Make"
        page={currentPage}
      />

      <Pagination
  currentPage={CarMakeStore.currentPage}
  onPageChange={(page) => CarMakeStore.setPage(page)}
  hasNextPage={CarMakeStore.hasNextPage}
/>
    </div>
  );
});

export default CarMakesList;