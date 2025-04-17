import { makeAutoObservable, runInAction } from "mobx";
import CarMakeService from "../common/Services/CarMakeService";

class CarMakeStore {
  carMakes = [];
  isLoading = false;
  error = null;
  pageNumber = 1;
  pageSize = 5;
  sortBy = 'name'; 
  filter = '';

  constructor() {
    makeAutoObservable(this);
  }
  
  setPageNumber(page) {
    this.pageNumber = page;
  }

  setPageSize(size) {
    this.pageSize = size;
  }

  setSortBy(sort) {
    this.sortBy = sort;
  }

  setFilter(filter) {
    this.filter = filter;
  }

  async fetchCarMakes() {
    this.isLoading = true;
    this.error = null;
  
    try {
      
      const data = await CarMakeService.getPSF(        
        this.pageNumber,
        this.pageSize,
        this.sortBy,
        this.filter
      );
  
      runInAction(() => {
        this.carMakes = data;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err.message;
      });
    } finally {
      runInAction(() => {
        this.isLoading = false;
      });
    }
  }

  async getCarMakeById(id) {
    this.isLoading = true;
    this.error = null;

    try {
      const carMake = await CarMakeService.getById(id);
      return carMake;
    } catch (error) {
      runInAction(() => {
        this.error = `Failed to load car make: ${error.message || "Unknown error"}`;
      });
      console.error(error);
      return null;
    } finally {
      runInAction(() => {
        this.isLoading = false;
      });
    }
  }

  async addCarMake(carMake) {
    try {
      const response = await CarMakeService.add(carMake);
      if (!response.ok) {
        throw new Error('Failed to add car make');
      }
      await this.fetchCarMakes();
      return { error: false, message: "Added!" };
    } catch (e) {
      console.error(e);
      return { error: true, message: "Adding error" };
    }
  }

  async editCarMake(id, updatedCarMake) {
    try {
      const response = await CarMakeService.edit(id, updatedCarMake);
      if (!response.ok) {
        throw new Error('Failed to edit car make');
      }
      await this.fetchCarMakes();
      return { error: false, message: "Edited!" };
    } catch (e) {
      console.error(e);
      return { error: true, message: "Editing problem" };
    }
  }

  async removeCarMake(id) {
    try {
      const response = await CarMakeService.remove(id);
      if (!response.ok) {
        throw new Error('Failed to remove car make');
      }
      await this.fetchCarMakes();
      return { error: false, message: "Removed!" };
    } catch (e) {
      console.error(e);
      return { error: true, message: "Operation failed" };
    }
  }

  setFilter(filter) {
    this.filter = filter;
    this.fetchCarMakes(this.page, this.pageSize, this.sort, filter);
  }

  setPage(page) {
    this.page = page;
    this.fetchCarMakes(page, this.pageSize, this.sort, this.filter);
  }

  setPageSize(pageSize) {
    this.pageSize = pageSize;
    this.fetchCarMakes(this.page, pageSize, this.sort, this.filter);
  }
}

export default new CarMakeStore();
