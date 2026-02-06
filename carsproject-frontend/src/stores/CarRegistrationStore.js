import { makeAutoObservable, runInAction } from "mobx";
import CarRegistrationService from "../common/Services/CarRegistrationService";

class CarRegistrationStore {
  carRegistrations = [];
  searchTerm = "";
  currentPage = 1;
  pageSize = 5;
  hasNextPage = false;

  loading = false;
  error = null;

  addStatus = { error: false, message: "" };

  constructor() {
    makeAutoObservable(this);
  }

  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;
    this.fetchCarRegistrations();
  }

  setPage(page) {
    this.currentPage = page;
    this.fetchCarRegistrations();
  }

  async fetchCarRegistrations() {
    this.loading = true;
    this.error = null;

    try {
      const pfs = {
        paging: { pageNumber: this.currentPage, pageSize: this.pageSize },
        sorting: { orderBy: "RegistrationNumber", descending: false },
        filter: { propertyName: "RegistrationNumber", filter: this.searchTerm || "" }
      };

      const response = await CarRegistrationService.getCarRegistrationsPFS(pfs);

      
      const filtered = response.items.filter(reg =>
        reg.registrationNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        reg.carOwnerFirstNameLastName.toLowerCase().includes(this.searchTerm.toLowerCase())
      );

      runInAction(() => {
        this.carRegistrations = filtered;
        this.hasNextPage = response.hasNextPage ?? false;
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.error = "Error fetching car registrations.";
        this.loading = false;
      });
      console.error(error);
    }
  }
}

export default new CarRegistrationStore();
