import { makeAutoObservable } from "mobx";
import CarOwnerService from "../common/Services/CarOwnerService";


class CarOwnerStore {
  searchTerm = '';  
  currentPage = 1;  
  pageSize = 5;      
  hasNextPage = false; 
  addStatus = { error: false, message: '' }; 
  carOwners = [];
  addStatus = { error: false, message: '' };
  selectedCarOwner = null;
  error = null;
  

  constructor() {
    makeAutoObservable(this);
  }

  
  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;  
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
  
  async addCarOwner(firstName, lastName, dateOfBirth) {
    this.addStatus = { error: false, message: 'Adding car owner...' }; 
  
    try {
      const result = await CarOwnerService.add({ firstName, lastName, dateOfBirth });    
      
      if (result && result.error) {
        this.addStatus = { error: true, message: result.message || 'An error occurred while adding.' };  
      } else {
        this.carOwners.push({ firstName, lastName, dateOfBirth });  
        this.addStatus = { error: false, message: result.message || 'Car owner added successfully!' };  
      }
    } catch (error) {
      this.addStatus = { error: true, message: 'Problem adding car owner' };  
      console.error('Error adding car owner:', error);
    }
  }
  
  async getCarOwnerById(id) {
    try {      
      const result = await CarOwnerService.getById(id);  
        if (result.error) {
        return { error: true, message: "Car Owner not found." };
      }
      return result;
    } catch (error) {
      return { error: true, message: "Error fetching Car Owner." };
    }
  }

  async editCarOwner(id, carOwner) {
    try {
      await CarOwnerService.edit(id, carOwner);
      return { error: false, message: "Car Owner edited successfully." };
    } catch (error) {
      return { error: true, message: "Error updating Car Owner." };
    }
  }
  

  
  
}

export default new CarOwnerStore();
