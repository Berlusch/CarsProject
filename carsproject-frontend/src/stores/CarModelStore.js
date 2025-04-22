import { makeAutoObservable } from "mobx";
import CarModelService from "../common/Services/CarModelService";


class CarModelStore {
  searchTerm = '';  // Pretraga
  currentPage = 1;  // Trenutna stranica
  pageSize = 5;     // Broj stavki po stranici
  sortDirection = "asc";  // Smjer sortiranja ('asc' ili 'desc')
  hasNextPage = false; 
  addStatus = { error: false, message: '' }; 
  carModels = [];
  addStatus = { error: false, message: '' };
  selectedCarModel = null;
  error = null;
  

  constructor() {
    makeAutoObservable(this);
  }

  // Postavljanje termina za pretragu
  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;  // Resetiranje stranice na 1 pri novoj pretrazi
  }

  // Postavljanje trenutne stranice
  setPage(page) {
    this.currentPage = page;
  }
    
  // Getter za filtere
  get filters() {
    return {
      searchTerm: this.searchTerm,   
      currentPage: this.currentPage, 
      pageSize: this.pageSize,       
      hasNextPage: this.hasNextPage   
    };
  }
  // Add Car Model
  async addCarModel(name, abrv, carMakeId, carEngineTypeId) {
    this.addStatus = { error: false, message: 'Adding car model...' }; 
  
    try {
      const result = await CarModelService.add({ name, abrv, carMakeId, carEngineTypeId });    
      
      if (result && result.error) {
        this.addStatus = { error: true, message: result.message || 'An error occurred while adding.' };  
      } else {
        this.carModels.push({ name, abrv, carMakeId, carEngineTypeId });  
        this.addStatus = { error: false, message: result.message || 'Car model added successfully!' }; 
      }
    } catch (error) {
      this.addStatus = { error: true, message: 'Problem adding car model' };  
      console.error('Error adding car model:', error);
    }
  }
  
  async getCarModelById(id) {
    try {      
      const result = await CarModelService.getById(id);  
      console.log("Store vraća: ", result);
        if (result.error) {
        return { error: true, message: "Car Model not found." };
      }
      return result;
      
    } catch (error) {
      return { error: true, message: "Error fetching car model." };
    }
  }

  async editCarModel(id, carModel) {
    try {
      await CarModelService.edit(id, carModel);
      return { error: false, message: "Car Model edited successfully." };
    } catch (error) {
      return { error: true, message: "Error updating Car Model." };
    }
  }

  //Search by Make
  get filteredModels() {
    return this.carModels.filter((model) =>
      model.carMakeName?.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  } 
 
  
}

export default new CarModelStore();
