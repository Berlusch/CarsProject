import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import CarModelStore from '../../stores/CarModelStore';
import CarModelService from '../../common/Services/CarModelService';
import Pagination from '../../components/Pagination';
import SearchBox from '../../components/SearchBox';
import { RouteNames } from '../../common/constants';

const CarModelsList = observer(() => {
  const navigate = useNavigate();
  const { carModels, currentPage, hasNextPage, searchTerm, sorting } = CarModelStore;

  useEffect(() => {
    CarModelStore.fetchCarModels();
  }, []);

  const handleSearch = (term) => {
    CarModelStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarModelStore.setPage(page);
  };

  const handleSort = (columnKey) => {
    CarModelStore.setSorting(columnKey);
  };

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_MODEL_EDIT.replace(':id', id));
  };

  const handleRemove = async (id) => {
    const carModel = carModels.find(c => c.id === id);
    if (!carModel) return;

    if (!confirm(`Are you sure you want to remove ${carModel.name}?`)) return;

    const response = await CarModelService.remove(id);
    if (response?.error) {
      alert(response.message);
      return;
    }

    CarModelStore.fetchCarModels();
  };

  const columns = [
    { header: 'Model Name', accessor: 'name', sortable: true },
    { header: 'Model Abrv', accessor: 'abrv', sortable: true },
    { header: 'Car Make', accessor: 'carMake', sortable: true },
    { header: 'Car Engine Type', accessor: 'carEngineType', sortable: true },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carModels.map(model => ({
    id: model.id,
    name: model.name,
    abrv: model.abrv,
    carMake: model.carMakeName,
    carEngineType: model.carEngineTypeType,
    edit: (
      <button className="edit-button" onClick={() => handleEdit(model.id)}>
        <i className="fas fa-edit"></i>
      </button>
    ),
    remove: (
      <button className="delete-button" onClick={() => handleRemove(model.id)}>
        <i className="fas fa-trash"></i>
      </button>
    )
  }));

  return (
    <div>
      <header className="entityName">Car Models</header>

      <SearchBox
        value={searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by car model..."
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

export default CarModelsList;