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
const navigate=useNavigate();
const [carMakes, setCarMakes] = useState([]);
const [currentPageSize, setCurrentPageSize] = useState(0); 
const {currentPage, pageSize, searchTerm } = CarMakeStore.filters;

const fetchCarMakes = async () => {
const { currentPage, pageSize, searchTerm} = CarMakeStore.filters;


    const response = await CarMakeService.getCarMakesPFS(currentPage, pageSize, "name", searchTerm);
    setCarMakes(response);  
    
    setCurrentPageSize(response.length);
    
  };

  useEffect(() => {
    fetchCarMakes();
  }, [CarMakeStore.filters]); 

  const handleSearch = (term) => {
    CarMakeStore.setSearchTerm(term); 
  };

  const handlePageChange = (page) => {
    CarMakeStore.setPage(page); 
  };

  useEffect(() => {
    fetchCarMakes();  
  }, [currentPage, pageSize, searchTerm]);

  const hasNextPage = currentPageSize === pageSize;

  // Funkcija za promjenu stranice
  const onPageChange = (newPage) => {
    CarMakeStore.setPage(newPage);
    fetchCarMakes();  
  };

    
  const columns = [
    { header: 'Name', accessor: 'name' },
    { header: 'Abrv', accessor: 'abrv' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carMakes && carMakes.map(carMake => {
      
    return {
      id: carMake.id,
      name: carMake.name,
      abrv: carMake.abrv,
      edit: (      
        
          <button className="edit-button" 
          onClick={() => handleEdit(carMake.id)}>
            <i className="fas fa-edit"></i>
          </button>
       
      ),
      remove: (
        <button className="delete-button" 
        onClick={() => handleRemove(carMake.id)}>
          <i className="fas fa-trash"></i>
        </button>
      ),
    };
  });
  
  const handleRemove = async (id) => {
    const carMake = carMakes.find(c => c.id === id);
    const carMakeName = carMake.name;

    if (!confirm(`Are you sure you want to remove ${carMakeName}?`)) {
      return;
    }

    const response = await CarMakeService.remove(id);

    if (response.error) {
      alert(response.message);
      return;
    }

    

    fetchCarMakes();  
  };

  const handleEdit = (id) => {navigate(RouteNames.CAR_MAKE_EDIT.replace(':id', id))};
  

  return (
    <div> 
      <header className="entityName">
        Car Makes
      </header>
      
    
      <SearchBox
      value={CarMakeStore.searchTerm}  
         onChange={(value) => CarMakeStore.setSearchTerm(value)}  
         onSearch={handleSearch}  
         placeholder="Search by car make..."
      />

      <Table
        columns={columns}
        data={data}
        onEdit={handleEdit}
        onRemove={handleRemove}
        onAdd={() => console.log('Add new car make')}
        routeNames={RouteNames.CAR_MAKE_ADD}
        entityName="Car Make"
      />
      <Pagination
        currentPage={CarMakeStore.currentPage}        
        onPageChange={handlePageChange}
        hasNextPage={hasNextPage}
      />
    </div>
  );
});

export default CarMakesList;
