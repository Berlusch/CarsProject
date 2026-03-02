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
  sorting = { orderBy: "registrationNumber", descending: false };

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

  setSorting(columnKey) {
    if (this.sorting.orderBy === columnKey) {
      this.sorting.descending = !this.sorting.descending;
    } else {
      this.sorting.orderBy = columnKey;
      this.sorting.descending = false;
    }
    this.fetchCarRegistrations();
  }

  async fetchCarRegistrations() {
    this.loading = true;
    this.error = null;

    try {
      const search = this.searchTerm?.toLowerCase() || "";

      const pfs = {
        paging: { pageNumber: this.currentPage, pageSize: this.pageSize },
        sorting: { orderBy: this.sorting.orderBy, descending: this.sorting.descending },
        filter: { propertyName: "registrationNumber", filter: search }
      };

      const response = await CarRegistrationService.getCarRegistrationsPFS(pfs);

      runInAction(() => {
        this.carRegistrations = response.items ?? [];
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

  async addCarRegistration(payload) {
    try {
      const response = await CarRegistrationService.add(payload);
      runInAction(() => {
        this.addStatus = response;
      });
      this.fetchCarRegistrations();
      return response;
    } catch (error) {
      runInAction(() => {
        this.addStatus = { error: true, message: 'Problem adding car registration.' };
      });
      return { error: true, message: 'Problem adding car registration.' };
    }
  }

  async editCarRegistration(id, registration) {
    return await CarRegistrationService.edit(id, registration);
  }

  async removeCarRegistration(id) {
    return await CarRegistrationService.remove(id);
  }
}

export default new CarRegistrationStore();