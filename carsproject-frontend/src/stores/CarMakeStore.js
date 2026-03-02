import { makeAutoObservable, runInAction } from "mobx";
import CarMakeService from "../common/Services/CarMakeService";

class CarMakeStore {
  carMakes = [];
  searchTerm = "";
  currentPage = 1;
  pageSize = 5;
  hasNextPage = false;

  loading = false;
  error = null;

  addStatus = {
    error: false,
    message: ""
  };
  sorting = { orderBy: "name", descending: false }; 

  constructor() {
    makeAutoObservable(this);
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
  
  setSorting(columnKey) {
  
    if (this.sorting.orderBy === columnKey) {
      this.sorting.descending = !this.sorting.descending;
    } else {
      this.sorting.orderBy = columnKey;
      this.sorting.descending = false;
    }
   
    this.fetchCarMakes();
  }

  async fetchCarMakes() {
    this.loading = true;
    try {
      const pfs = {
        paging: { pageNumber: this.currentPage, pageSize: this.pageSize },
        sorting: { 
          orderBy: this.sorting.orderBy, 
          descending: this.sorting.descending 
        },
        filter: { propertyName: "Name", filter: this.searchTerm || "" }
      };

      const response = await CarMakeService.getCarMakesPFS(pfs);

      runInAction(() => {
        this.carMakes = response.items ?? [];
        this.hasNextPage = response.hasNextPage ?? false;
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.error = "Error fetching car makes.";
        this.loading = false;
      });
      console.error(error);
    }
  }

  async addCarMake(name, abrv) {
    runInAction(() => {
      this.addStatus = { error: false, message: "Adding car make..." };
    });

    try {
      const result = await CarMakeService.add({ name, abrv });

      if (result?.error) {
        runInAction(() => {
          this.addStatus = {
            error: true,
            message: result.message || "Error adding car make."
          };
        });
        return;
      }

      runInAction(() => {
        this.addStatus = {
          error: false,
          message: result.message || "Car make added successfully."
        };
      });

      await this.fetchCarMakes();
    } catch (error) {
      runInAction(() => {
        this.addStatus = {
          error: true,
          message: "Problem adding car make."
        };
      });
      console.error(error);
    }
  }

  async getCarMakeById(id) {
    try {
      const result = await CarMakeService.getById(id);

      if (result?.error) {
        return { error: true, message: "Car Make not found." };
      }

      return result;
    } catch (error) {
      return { error: true, message: "Error fetching car make." };
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