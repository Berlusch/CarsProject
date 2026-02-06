import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import CarModelStore from '../../stores/CarModelStore';
import CarModelService from '../../common/Services/CarModelService';
import Table from '../../components/Table';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { RouteNames } from '../../common/constants';

const CarModelsList = observer(() => {
  const navigate = useNavigate();

  useEffect(() => {
    CarModelStore.fetchCarModels();
  }, []);

  const handleSearch = (term) => {
    CarModelStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarModelStore.setPage(page);
  };

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_MODEL_EDIT.replace(':id', id));
  };

  const handleRemove = async (id) => {
    const carModel = CarModelStore.carModels.find(c => c.id === id);
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
    { header: 'Model Name', accessor: 'name' },
    { header: 'Model Abrv', accessor: 'abrv' },
    { header: 'Car Make', accessor: 'carMake' },
    { header: 'Car Engine Type', accessor: 'carEngineType' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = CarModelStore.carModels.map(carModel => ({
    id: carModel.id,
    name: carModel.name,
    abrv: carModel.abrv,
    carMake: carModel.carMakeName,
    carEngineType: carModel.carEngineTypeType,
    edit: (
      <button className="edit-button" onClick={() => handleEdit(carModel.id)}>
        <i className="fas fa-edit"></i>
      </button>
    ),
    remove: (
      <button className="delete-button" onClick={() => handleRemove(carModel.id)}>
        <i className="fas fa-trash"></i>
      </button>
    )
  }));

  return (
    <div>
      <header className="entityName">Car Models</header>

      <SearchBox
        value={CarModelStore.searchTerm}
        onChange={handleSearch}
        onSearch={handleSearch}
        placeholder="Search by car model..."
      />

      <Table
        columns={columns}
        data={data}
        routeNames={RouteNames.CAR_MODEL_ADD}
        entityName="Car Model"
        page={CarModelStore.currentPage}
      />

      <Pagination
        currentPage={CarModelStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={CarModelStore.hasNextPage}
      />
    </div>
  );
});

export default CarModelsList;
