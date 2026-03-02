import React, { useEffect } from 'react';
import { observer } from 'mobx-react';
import CarRegistrationStore from '../../stores/CarRegistrationStore';
import Pagination from '../../components/Pagination';
import SearchBox from '../../components/SearchBox';
import { useNavigate } from 'react-router-dom';
import { RouteNames } from '../../common/constants';

const CarRegistrationsList = observer(() => {
  const navigate = useNavigate();
  const { currentPage, carRegistrations, hasNextPage, searchTerm, sorting } = CarRegistrationStore;

  useEffect(() => {
    CarRegistrationStore.fetchCarRegistrations();
  }, []);

  const handleSearch = (term) => {
    CarRegistrationStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarRegistrationStore.setPage(page);
  };

  const handleSort = (columnKey) => {
    CarRegistrationStore.setSorting(columnKey);
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
    { header: 'Registration', accessor: 'registrationNumber', sortable: true },
    { header: 'Car Owner', accessor: 'carOwnerFirstNameLastName', sortable: true },
    { header: 'Car Model', accessor: 'carModelName', sortable: true },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carRegistrations.map(reg => ({
    id: reg.id,
    registrationNumber: reg.registrationNumber,
    carOwnerFirstNameLastName: reg.carOwnerFirstNameLastName,
    carModelName: reg.carModelName,
    edit: (
      <button className="edit-button" onClick={() => handleEdit(reg.id)}>
        <i className="fas fa-edit"></i>
      </button>
    ),
    remove: (
      <button className="delete-button" onClick={() => handleRemove(reg.id)}>
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
                  {col.sortable && sorting.orderBy === col.accessor && (
                    <span>{sorting.descending ? '↓' : '↑'}</span>
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
        currentPage={currentPage}
        onPageChange={handlePageChange}
        hasNextPage={hasNextPage}
      />
    </div>
  );
});

export default CarRegistrationsList;