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

  // ---------- DEFINICIJA KOLONA SA SORTIRANJEM ----------
  const columns = [
    { header: 'Name', accessor: 'name', sortable: true },
    { header: 'Abrv', accessor: 'abrv', sortable: true },
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

  // ---------- FUNKCIJA ZA KLIK NA HEADER (SERVER-SIDE SORT) ----------
  const handleSort = (columnKey) => {
    CarMakeStore.setSorting(columnKey);
  };

  return (
    <div>
      <header className="entityName">Car Makes</header>

      <SearchBox
        value={CarMakeStore.searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by car make..."
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
                  {col.sortable && CarMakeStore.sorting.orderBy === col.accessor && (
                    <span>{CarMakeStore.sorting.descending ? '↓' : '↑'}</span>
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
        currentPage={CarMakeStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarMakeStore.hasNextPage}
      />
    </div>
  );
});

export default CarMakesList;