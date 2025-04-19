import { makeAutoObservable } from "mobx";
import CarMakeService from "../common/Services/CarMakeService";

class CarMakeStore {
  searchTerm = '';  // Pretraga
  currentPage = 1;  // Trenutna stranica
  pageSize = 5;     // Broj stavki po stranici
  sortDirection = "asc";  // Smjer sortiranja ('asc' ili 'desc')
  hasNextPage = false; 
  addStatus = { error: false, message: '' }; 
  carMakes = [];
  addStatus = { error: false, message: '' };

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
  // Add Car Make
  async addCarMake(name, abrv) {
    this.addStatus = { error: false, message: 'Adding car make...' }; // Inicijaliziramo status
  
    try {
      const result = await CarMakeService.add({ name, abrv });  // Pozivamo servis za dodavanje
  
      // Provjera ako result nije null ili undefined
      if (result && result.error) {
        this.addStatus = { error: true, message: result.message || 'An error occurred while adding.' };  // Ako dođe do greške, postavljamo status
      } else {
        this.carMakes.push({ name, abrv });  // Ako je uspješno, dodajemo novi car make u store
        this.addStatus = { error: false, message: result.message || 'Car make added successfully!' };  // Ažuriramo status s uspješnim porukama
      }
    } catch (error) {
      this.addStatus = { error: true, message: 'Problem adding car make' };  // Ako dođe do greške u komunikaciji, postavljamo status
      console.error('Error adding car make:', error);
    }
  }
  

  
  
}

export default new CarMakeStore();
