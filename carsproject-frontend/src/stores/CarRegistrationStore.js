import { makeAutoObservable } from "mobx";
import CarRegistrationService from "../common/Services/CarRegistrationService";


class CarRegistrationStore {
  searchTerm = '';  
  currentPage = 1;  
  pageSize = 5;    
  hasNextPage = false; 
  addStatus = { error: false, message: '' }; 
  carRegistrations = [];
  addStatus = { error: false, message: '' };
  selectedCarRegistration = null;
  error = null;
  

  constructor() {
    makeAutoObservable(this);
  }
  
  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;  // Resetiranje stranice na 1 pri novoj pretrazi
  }
  
  setPage(page) {
    this.currentPage = page;
  }
      
  get filters() {
    return {
      searchTerm: this.searchTerm,   
      currentPage: this.currentPage, 
      pageSize: this.pageSize,       
      hasNextPage: this.hasNextPage   
    };
  }
  async addCarRegistration(registrationNumber, carOwnerId, carModelId) {
    this.addStatus = { error: false, message: 'Adding car registration...' };
  
    try {
      const result = await CarRegistrationService.add({ registrationNumber, carOwnerId, carModelId });
        
      if (result && result.error) {
        this.addStatus = { error: true, message: result.message || 'An error occurred while adding.' };
      } else {
        this.carRegistrations.push({ registrationNumber, carOwnerId, carModelId });
        this.addStatus = { error: false, message: result.message || 'Car registration added successfully!' };
      }
    } catch (error) {
      this.addStatus = { error: true, message: 'Problem adding car registration' };
      console.error('Error adding car registration:', error);
      
    }  
  }
  
  async getCarRegistrationById(id) {
    try {      
      const result = await CarRegistrationService.getById(id);  
        if (result.error) {
        return { error: true, message: "Car Registration not found." };
      }
      return result;
    } catch (error) {
      return { error: true, message: "Error fetching Car Registration." };
    }
  }

  async editCarRegistration(id, carRegistration) {
    try {
      await CarRegistrationService.edit(id, carRegistration);
      return { error: false, message: "Car Registration edited successfully." };
    } catch (error) {
      return { error: true, message: "Error updating Car Registration." };
    }
  }
    
  
}

export default new CarRegistrationStore();
