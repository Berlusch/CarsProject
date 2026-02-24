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
  
  currentRegistration = null;

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

  async getCarRegistrationById(id) {
    this.loading = true;
    this.error = null;

    try {
      const response = await CarRegistrationService.getById(id);
      if (!response.error) {
        runInAction(() => {
          this.currentRegistration = response.message;
          this.loading = false;
        });
      }
      return response; 
    } catch (error) {
      runInAction(() => {
        this.error = "Error fetching registration by ID.";
        this.loading = false;
      });
      console.error(error);
      return { error: true, message: "Error fetching registration by ID." };
    }
  }

 
  async addCarRegistration(registration) {
    const response = await CarRegistrationService.add(registration);
    runInAction(() => {
      this.addStatus = response;
    });
    return response;
  }


  async editCarRegistration(id, registration) {
    return await CarRegistrationService.edit(id, registration);
  }


  async removeCarRegistration(id) {
    return await CarRegistrationService.remove(id);
  }
}

export default new CarRegistrationStore();