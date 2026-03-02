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
    { header: 'Last Name', accessor: 'lastName', sortable: true },
    { header: 'First Name', accessor: 'firstName', sortable: true },
    { header: 'Date of Birth', accessor: 'dateOfBirth', sortable: true },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];
  
  const data = CarOwnerStore.carOwners.map(owner => ({
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

  const handleSort = (columnKey) => {
    CarOwnerStore.setSorting(columnKey);
  };

  return (
    <div>
      <header className="entityName">Car Owners</header>

      <SearchBox
        value={CarOwnerStore.searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by first or last name..."
      />

      <div className="table-container">
        <table className="custom-table">
          <thead>
            <tr>
              {columns.map(col => (
                <th
                  key={col.accessor}
                  onClick={col.sortable ? () => handleSort(col.accessor) : undefined}
                  style={{ cursor: col.sortable ? 'pointer' : 'default' }}
                >
                  {col.header}{' '}
                  {col.sortable && CarOwnerStore.sorting.orderBy === col.accessor && (
                    <span>{CarOwnerStore.sorting.descending ? '↓' : '↑'}</span>
                  )}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {data.map((item, index) => (
              <tr key={item.id} className={index % 2 === 0 ? 'row-light' : 'row-white'}>
                {columns.map(col => (
                  <td key={col.accessor}>{item[col.accessor]}</td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Pagination
        currentPage={CarOwnerStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarOwnerStore.hasNextPage}
      />
    </div>
  );
});

export default CarOwnersList;