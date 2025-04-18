import { makeAutoObservable } from "mobx";

class CarMakeStore {
  searchTerm = '';  // Pretraga
  currentPage = 1;  // Trenutna stranica
  pageSize = 5;     // Broj stavki po stranici
  sortDirection = "asc";  // Smjer sortiranja ('asc' ili 'desc')
  totalCount = 0;

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
      searchTerm: this.searchTerm,   // Trenutni pojam za pretragu
      currentPage: this.currentPage, // Trenutna stranica
      pageSize: this.pageSize,       // Broj stavki po stranici      
    };
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}

export default new CarMakeStore();
