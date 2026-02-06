import React, { useEffect } from 'react';
import { observer } from 'mobx-react';
import CarRegistrationStore from '../../stores/CarRegistrationStore';
import Table from '../../components/Table';
import Pagination from '../../components/Pagination';
import SearchBox from '../../components/SearchBox';
import { useNavigate } from 'react-router-dom';
import { RouteNames } from '../../common/constants';

const CarRegistrationsList = observer(() => {
  const navigate = useNavigate();
  const { currentPage, pageSize, searchTerm, carRegistrations, hasNextPage } = CarRegistrationStore;

  useEffect(() => {
    CarRegistrationStore.fetchCarRegistrations();
  }, []);

  const handleSearch = (term) => {
    CarRegistrationStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarRegistrationStore.setPage(page);
  };

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_REGISTRATION_EDIT.replace(':id', id));
  };

  const handleRemove = async (id) => {
    const carRegistration = carRegistrations.find(c => c.id === id);
    if (!carRegistration) return;

    if (!confirm(`Are you sure you want to remove ${carRegistration.registrationNumber}?`)) return;

    const response = await CarRegistrationStore.removeCarRegistration(id);
    if (response?.error) {
      alert(response.message);
      return;
    }

    CarRegistrationStore.fetchCarRegistrations();
  };

  const columns = [
    { header: 'Registration', accessor: 'registrationNumber' },
    { header: 'Car Owner', accessor: 'carOwnerFirstNameLastName' },
    { header: 'Car Model', accessor: 'carModelName' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carRegistrations.map(carRegistration => ({
    id: carRegistration.id,
    registrationNumber: carRegistration.registrationNumber,
    carOwnerFirstNameLastName: carRegistration.carOwnerFirstNameLastName,
    carModelName: carRegistration.carModelName,
    edit: (
        <button className="edit-button" onClick={() => handleEdit(carRegistration.id)}>
          <i className="fas fa-edit"></i>
        </button>
      ),
      remove: (
        <button className="delete-button" onClick={() => handleRemove(carRegistration.id)}>
          <i className="fas fa-trash"></i>
        </button>
      )
  }));

  return (
    <div>
      <header className="entityName">Car Registrations</header>

      <SearchBox
        value={searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by registration..."
      />

      <Table columns={columns} data={data} entityName="Car Registration" />

      <Pagination currentPage={currentPage} onPageChange={handlePageChange} hasNextPage={hasNextPage} />
    </div>
  );
});

export default CarRegistrationsList;
