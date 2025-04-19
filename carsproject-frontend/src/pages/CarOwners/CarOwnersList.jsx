import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react';
import CarOwnerStore from '../../stores/CarOwnerStore';
import CarOwnerService from '../../common/Services/CarOwnerService';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { useNavigate } from 'react-router-dom';



const CarOwnersList = observer(() => {
const navigate=useNavigate();
const [carOwners, setCarOwners] = useState([]);
const [currentPageSize, setCurrentPageSize] = useState(0); 
const {currentPage, pageSize, searchTerm } = CarOwnerStore.filters;

const fetchCarOwners = async () => {
const { currentPage, pageSize, searchTerm} = CarOwnerStore.filters;


    const response = await CarOwnerService.getCarOwnersPFS(currentPage, pageSize, "last name", searchTerm);
    setCarOwners(response);  
    console.log("podatci:", response)
    
    setCurrentPageSize(response.length);
    
  };

  useEffect(() => {
    fetchCarOwners();
  }, [CarOwnerStore.filters]); 

  const handleSearch = (term) => {
    CarOwnerStore.setSearchTerm(term); 
  };

  const handlePageChange = (page) => {
    CarOwnerStore.setPage(page); 
  };

  useEffect(() => {
    fetchCarOwners();  
  }, [currentPage, pageSize, searchTerm]);

  const hasNextPage = currentPageSize === pageSize;

  // Funkcija za promjenu stranice
  const onPageChange = (newPage) => {
    CarOwnerStore.setPage(newPage);
    fetchCarOwners();  
  };

    
  const columns = [
    { header: 'First Name', accessor: 'firstName' },
    { header: 'Last Name', accessor: 'lastName' },
    { header: 'Date of Birth', accessor: 'dateOfBirth' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carOwners && carOwners.map(carOwner => {
      
    return {
      id: carOwner.id,
      firstName: carOwner.firstName,
      lastName: carOwner.lastName,
      dateOfBirth: carOwner.dateOfBirth,
      edit: (      
        
          <button className="edit-button" 
          onClick={() => handleEdit(carOwner.id)}>
            <i className="fas fa-edit"></i>
          </button>
       
      ),
      remove: (
        <button className="delete-button" 
        onClick={() => handleRemove(carOwner.id)}>
          <i className="fas fa-trash"></i>
        </button>
      ),
    };
  });
  
  const handleRemove = async (id) => {
    const carOwner = carOwners.find(c => c.id === id);
    const carOwnerFirstName = carOwner.firstName;
    const carOwnerLastName = carOwner.lastName;

    if (!confirm(`Are you sure you want to remove ${carOwnerFirstName + carOwnerLastName}?`)) {
      return;
    }

    const response = await CarOwnerService.remove(id);

    if (response.error) {
      alert(response.message);
      return;
    }

    

    fetchCarOwners();  
  };

  const handleEdit = (id) => {navigate(RouteNames.CAR_OWNER_EDIT.replace(':id', id))};
  

  return (
    <div> 
      <header className="entityName">
        Car Owners
      </header>
      
    
      <SearchBox
      value={CarOwnerStore.searchTerm}  
         onChange={(value) => CarOwnerStore.setSearchTerm(value)}  
         onSearch={handleSearch}  
      />
      <Table
        columns={columns}
        data={data}
        onEdit={handleEdit}
        onRemove={handleRemove}
        onAdd={() => console.log('Add new car make')}
        routeNames={RouteNames.CAR_OWNER_ADD}
        entityName="Car Owner"
      />
      <Pagination
        currentPage={CarOwnerStore.currentPage}        
        onPageChange={handlePageChange}
        hasNextPage={hasNextPage}
      />
    </div>
  );
});

export default CarOwnersList;
