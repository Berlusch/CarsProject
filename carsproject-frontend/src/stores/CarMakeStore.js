import CarMakeService from "../common/Services/CarMakeService";
import { makeAutoObservable, runInAction } from "mobx";


class CarMakeStore {
  searchTerm = '';  
  currentPage = 1;  
  pageSize = 5;     
  hasNextPage = false; 
  addStatus = { error: false, message: '' }; 
  carMakes = [];
  addStatus = { error: false, message: '' };
  selectedCarMake = null;
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

  async fetchCarMakes() {
  try {
    const pfs = {
      currentPage: this.currentPage,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm
    };

    const response = await CarMakeService.getCarMakesPFS(pfs);

    runInAction(() => {
      this.carMakes = response.items;
      this.hasNextPage = response.hasNextPage;
    });

  } catch (error) {
    runInAction(() => {
      this.error = "Error fetching car makes";
    });
    console.error(error);
  }
}
  async addCarMake(name, abrv) {
    this.addStatus = { error: false, message: 'Adding car make...' }; 
  
    try {
      const result = await CarMakeService.add({ name, abrv });        
      if (result && result.error) {
        this.addStatus = { error: true, message: result.message || 'An error occurred while adding.' };  
      } else {
        this.carMakes.push({ name, abrv });  
        this.addStatus = { error: false, message: result.message || 'Car make added successfully!' };  
      }
    } catch (error) {
      this.addStatus = { error: true, message: 'Problem adding car make' };  
      console.error('Error adding car make:', error);
    }
  }
  
  async getCarMakeById(id) {
    try {      
      const result = await CarMakeService.getById(id);  
        if (result.error) {
        return { error: true, message: "Car Make not found." };
      }
      return result;
    } catch (error) {
      return { error: true, message: "Error fetching a car make." };
    }
  }

  async editCarMake(id, carMake) {
    try {
      await CarMakeService.edit(id, carMake);
      return { error: false, message: "Car make edited successfully." };
    } catch (error) {
      return { error: true, message: "Error updating Car Make." };
    }
  }
  

  
  
}

export default new CarMakeStore();
