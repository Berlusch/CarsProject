import { makeAutoObservable } from "mobx";
import CarRegistrationService from "../common/Services/CarRegistrationService";


class CarRegistrationStore {
  searchTerm = '';  // Pretraga
  currentPage = 1;  // Trenutna stranica
  pageSize = 5;     // Broj stavki po stranici
  sortDirection = "asc";  // Smjer sortiranja ('asc' ili 'desc')
  hasNextPage = false; 
  addStatus = { error: false, message: '' }; 
  carRegistrations = [];
  addStatus = { error: false, message: '' };
  selectedCarRegistration = null;
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
  // Add Car Registration
  async addCarRegistration(name, abrv) {
    this.addStatus = { error: false, message: 'Adding car make...' }; // Inicijaliziramo status
  
    try {
      const result = await CarRegistrationService.add({ name, abrv });  // Pozivamo servis za dodavanje
  
      // Provjera ako result nije null ili undefined
      if (result && result.error) {
        this.addStatus = { error: true, message: result.message || 'An error occurred while adding.' };  // Ako dođe do greške, postavljamo status
      } else {
        this.carRegistrations.push({ name, abrv });  // Ako je uspješno, dodajemo novi car make u store
        this.addStatus = { error: false, message: result.message || 'Car make added successfully!' };  // Ažuriramo status s uspješnim porukama
      }
    } catch (error) {
      this.addStatus = { error: true, message: 'Problem adding car make' };  // Ako dođe do greške u komunikaciji, postavljamo status
      console.error('Error adding car make:', error);
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
