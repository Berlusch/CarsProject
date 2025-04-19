import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react';
import CarRegistrationStore from '../../stores/CarRegistrationStore';
import CarRegistrationService from '../../common/Services/CarRegistrationService';
import Table from '../../components/Table';
import { RouteNames } from '../../common/constants';
import SearchBox from '../../components/SearchBox';
import Pagination from '../../components/Pagination';
import { useNavigate } from 'react-router-dom';



const CarRegistrationsList = observer(() => {
const navigate=useNavigate();
const [carRegistrations, setCarRegistrations] = useState([]);
const [currentPageSize, setCurrentPageSize] = useState(0); 
const {currentPage, pageSize, searchTerm } = CarRegistrationStore.filters;

const fetchCarRegistrations = async () => {
const { currentPage, pageSize, searchTerm} = CarRegistrationStore.filters;


    const response = await CarRegistrationService.getCarRegistrationsPFS(currentPage, pageSize, "name", searchTerm);
    setCarRegistrations(response);  
    setCurrentPageSize(response.length);
    
  };

  useEffect(() => {
    fetchCarRegistrations();
  }, [CarRegistrationStore.filters]); 

  const handleSearch = (term) => {
    CarRegistrationStore.setSearchTerm(term); 
  };

  const handlePageChange = (page) => {
    CarRegistrationStore.setPage(page); 
  };

  useEffect(() => {
    fetchCarRegistrations();  
  }, [currentPage, pageSize, searchTerm]);

  const hasNextPage = currentPageSize === pageSize;

  // Funkcija za promjenu stranice
  const onPageChange = (newPage) => {
    CarRegistrationStore.setPage(newPage);
    fetchCarRegistrations();  
  };

    
  const columns = [
    { header: 'Registration Number', accessor: 'registrationNumber' },
    { header: 'Car Owner', accessor: 'carOwnerFirstName + carOwnerLastName' },
    { header: 'Car Model', accessor: 'carModel' },
    { header: 'Edit', accessor: 'edit' },
    { header: 'Remove', accessor: 'remove' }
  ];

  const data = carRegistrations && carRegistrations.map(carRegistration => {
      
    return {
      id: carRegistration.id,
      registrationNumber: carRegistration.registrationNumber,
      carOwner: carRegistration.carOwner,
      edit: (      
        
          <button className="edit-button" 
          onClick={() => handleEdit(carRegistration.id)}>
            <i className="fas fa-edit"></i>
          </button>
       
      ),
      remove: (
        <button className="delete-button" 
        onClick={() => handleRemove(carRegistration.id)}>
          <i className="fas fa-trash"></i>
        </button>
      ),
    };
  });
  
  const handleRemove = async (id) => {
    const carRegistration = carRegistrations.find(c => c.id === id);
    const carRegistrationNumber = carRegistration.registrationNumber;

    if (!confirm(`Are you sure you want to remove ${carRegistrationNumber}?`)) {
      return;
    }

    const response = await CarRegistrationService.remove(id);

    if (response.error) {
      alert(response.message);
      return;
    }

    

    fetchCarRegistrations();  
  };

  const handleEdit = (id) => {navigate(RouteNames.CAR_REGISTRATION_EDIT.replace(':id', id))};
  

  return (
    <div> 
      <header className="entityName">
        Car Registrations
      </header>
      
    
      <SearchBox
      value={CarRegistrationStore.searchTerm}  
         onChange={(value) => CarRegistrationStore.setSearchTerm(value)}  
         onSearch={handleSearch}  
      />
      <Table
        columns={columns}
        data={data}
        onEdit={handleEdit}
        onRemove={handleRemove}
        onAdd={() => console.log('Add new car make')}
        routeNames={RouteNames.CAR_MAKE_ADD}
        entityName="Car Registration"
      />
      <Pagination
        currentPage={CarRegistrationStore.currentPage}        
        onPageChange={handlePageChange}
        hasNextPage={hasNextPage}
      />
    </div>
  );
});

export default CarRegistrationsList;
