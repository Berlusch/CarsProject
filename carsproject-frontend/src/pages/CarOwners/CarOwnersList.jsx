import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import CarOwnerStore from '../../stores/CarOwnerStore';
import CarOwnerService from '../../common/Services/CarOwnerService';
import Table from '../../components/Table';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { RouteNames } from '../../common/constants';

const CarOwnersList = observer(() => {
  const navigate = useNavigate();
  
  useEffect(() => {
    CarOwnerStore.fetchCarOwners();
  }, []);

  const handleSearch = (term) => {
    CarOwnerStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarOwnerStore.setPage(page);
  };

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_OWNER_EDIT.replace(':id', id));
  };

  const handleRemove = async (id) => {
    const carOwner = CarOwnerStore.carOwners.find(c => c.id === id);
    if (!carOwner) return;

    if (!confirm(`Are you sure you want to remove ${carOwner.firstName} ${carOwner.lastName}?`)) return;

    const response = await CarOwnerService.remove(id);

    if (response?.error) {
      alert(response.message);
      return;
    }

    CarOwnerStore.fetchCarOwners();
  };

  const columns = [
    { header: 'Last Name', accessor: 'lastName' },
    { header: 'First Name', accessor: 'firstName' },
    { header: 'Date of Birth', accessor: 'dateOfBirth' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];
  
  const data = CarOwnerStore.carOwners
    .filter(owner => {
      const term = CarOwnerStore.searchTerm.toLowerCase();
      return (
        owner.firstName.toLowerCase().includes(term) ||
        owner.lastName.toLowerCase().includes(term)
      );
    })
    .map(owner => ({
      id: owner.id,
      lastName: owner.lastName,
      firstName: owner.firstName,
      dateOfBirth: owner.dateOfBirth,
      edit: (
        <button className="edit-button" onClick={() => handleEdit(owner.id)}>
          <i className="fas fa-edit"></i>
        </button>
      ),
      remove: (
        <button className="delete-button" onClick={() => handleRemove(owner.id)}>
          <i className="fas fa-trash"></i>
        </button>
      )
    }));

  return (
    <div>
      <header className="entityName">Car Owners</header>

      <SearchBox
        value={CarOwnerStore.searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by first or last name..."
      />

      <Table
        columns={columns}
        data={data}
        routeNames={RouteNames.CAR_OWNER_ADD}
        entityName="Car Owner"
        page={CarOwnerStore.currentPage}
      />

      <Pagination
        currentPage={CarOwnerStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarOwnerStore.hasNextPage}
      />
    </div>
  );
});

export default CarOwnersList;
