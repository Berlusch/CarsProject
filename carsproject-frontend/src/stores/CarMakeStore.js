import { makeAutoObservable } from "mobx";

class CarMakeStore {
  searchTerm = '';
  currentPage = 1;
  pageSize = 5;
  sortDirection = "asc"; // 'asc' | 'desc'

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

  setSortDirection(direction) {
    this.sortDirection = direction;
  }
  
  get filteredCarMakes() {
    return {
      searchTerm: this.searchTerm,
      currentPage: this.currentPage,
      pageSize: this.pageSize,
      sortDirection: this.sortDirection,
    };
  }
}

export default new CarMakeStore();
