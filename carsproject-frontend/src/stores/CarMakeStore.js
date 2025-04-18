import { makeAutoObservable, runInAction } from "mobx";
import httpService from "../common/Services/HttpService";

class CarMakeStore {
  carMakes = [];
  totalCount = 0;

  // Stanja za pretragu, paginaciju i sortiranje
  searchTerm = "";
  currentPage = 1;
  pageSize = 5;
  sortDirection = "asc"; // 'asc' | 'desc'

  constructor() {
    makeAutoObservable(this);
  }

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;
    this.fetchCarMakes();
  }

  setPage(page) {
    this.currentPage = page;
    this.fetchCarMakes();
  }

  setSortDirection(direction) {
    this.sortDirection = direction;
    this.fetchCarMakes();
  }

  async fetchCarMakes() {
    try {
      const response = await httpService.get("/carmake", {
        params: {
          name: this.searchTerm,
          page: this.currentPage,
          pageSize: this.pageSize,
          sortBy: "name",
          sortDirection: this.sortDirection,
        },
      });

      runInAction(() => {
        this.carMakes = response.data.items; // ili kako već vraća tvoj backend
        this.totalCount = response.data.totalCount;
      });
    } catch (error) {
      console.error("Failed to fetch car makes:", error);
    }
  }
}

export default new CarMakeStore();
