import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react';
import CarModelStore from '../../stores/CarModelStore';
import CarModelService from '../../common/Services/CarModelService';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { useNavigate } from 'react-router-dom';


const CarModelsList = observer(() => {
  const navigate = useNavigate();
  const [carModels, setCarModels] = useState([]);
  const [currentPageSize, setCurrentPageSize] = useState(0);
  const { currentPage, pageSize, searchTerm } = CarModelStore.filters;
  

  const fetchCarModels = async () => {
    const { currentPage, pageSize, searchTerm } = CarModelStore.filters;
    const response = await CarModelService.getCarModelsPFS(currentPage, pageSize, "name", ""); // prazno jer filtriraš ručno


  const filtered = response.filter(model =>
  model.carMakeName.toLowerCase().includes(searchTerm.toLowerCase())
);

setCarModels(filtered);

setCurrentPageSize(filtered.length);
  };  
    useEffect(() => {
    fetchCarModels();    
    
  }, [CarModelStore.filters]);

  const handleSearch = (term) => {
    CarModelStore.setSearchTerm(term);
  };

  const handlePageChange = (page) => {
    CarModelStore.setPage(page);
  };

  useEffect(() => {
    fetchCarModels();
  }, [currentPage, pageSize, searchTerm]);

  const hasNextPage = currentPageSize === pageSize;  

  const columns = [
    { header: 'Model Name', accessor: 'name' },
    { header: 'Model Abrv', accessor: 'abrv' },
    { header: 'Car Make', accessor: 'carMake' },
    { header: 'Car Engine Type', accessor: 'carEngineType' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carModels && carModels.map(carModel => {
    return {
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
      ),
    };
  });


  const handleRemove = async (id) => {
    const carModel = carModels.find(c => c.id === id);
    const carModelNumber = carModel.registrationNumber;
    if (!confirm(`Are you sure you want to remove ${carModelNumber}?`)) {
      return;
    }
    const response = await CarModelService.remove(id);
    if (response.error) {
      alert(response.message);
      return;
    }
    fetchCarModels();
  };

  const handleEdit = (id) => {
    navigate(RouteNames.CAR_MODEL_EDIT.replace(':id', id));
  };

  return (
    <div>
      <header className="entityName">Car Models</header>
      <SearchBox
        value={CarModelStore.searchTerm}
        onChange={(value) => CarModelStore.setSearchTerm(value)}
        onSearch={handleSearch}
        placeholder="Search by car make..."
      />
      <Table
        columns={columns}
        data={data}
        onEdit={handleEdit}
        onRemove={handleRemove}
        onAdd={() => console.log('Add a new car model')}
        routeNames={RouteNames.CAR_MODEL_ADD}
        entityName="Car Model"
      />
      <Pagination
        currentPage={CarModelStore.currentPage}
        onPageChange={handlePageChange}
        hasNextPage={hasNextPage}
      />
    </div>
  );
});

export default CarModelsList;
