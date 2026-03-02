import { makeAutoObservable, runInAction } from "mobx";
import CarModelService from "../common/Services/CarModelService";

class CarModelStore {
  carModels = [];
  searchTerm = "";
  currentPage = 1;
  pageSize = 5;
  hasNextPage = false;
  loading = false;
  error = null;
  addStatus = { error: false, message: "" };
  sorting = { orderBy: "name", descending: false };

  constructor() {
    makeAutoObservable(this);
  }

  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;
    this.fetchCarModels();
  }

  setPage(page) {
    this.currentPage = page;
    this.fetchCarModels();
  }

  setSorting(columnKey) {
    if (this.sorting.orderBy === columnKey) {
      this.sorting.descending = !this.sorting.descending;
    } else {
      this.sorting.orderBy = columnKey;
      this.sorting.descending = false;
    }
    this.fetchCarModels();
  }

  async fetchCarModels() {
    this.loading = true;
    this.error = null;
    try {
      const pfs = {
        paging: { pageNumber: this.currentPage, pageSize: this.pageSize },
        sorting: { orderBy: this.sorting.orderBy, descending: this.sorting.descending },
        filter: { propertyName: "name", filter: this.searchTerm || "" }
      };
      const response = await CarModelService.getCarModelsPFS(pfs);
      runInAction(() => {
        this.carModels = response.items ?? [];
        this.hasNextPage = response.hasNextPage ?? false;
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.error = "Error fetching car models.";
        this.loading = false;
      });
      console.error(error);
    }
  }

  async addCarModel(name, abrv, carMakeId, carEngineTypeId) {
    runInAction(() => { this.addStatus = { error: false, message: "Adding car model..." }; });
    try {
      const result = await CarModelService.add({ name, abrv, carMakeId, carEngineTypeId });
      if (result?.error) {
        runInAction(() => { this.addStatus = { error: true, message: result.message || "Error adding car model." }; });
        return;
      }
      runInAction(() => { this.addStatus = { error: false, message: result.message || "Car model added successfully." }; });
      await this.fetchCarModels();
    } catch (error) {
      runInAction(() => { this.addStatus = { error: true, message: "Problem adding car model." }; });
      console.error(error);
    }
  }

  async getCarModelById(id) {
    try {
      const result = await CarModelService.getById(id);
      if (result?.error) return { error: true, message: "Car Model not found." };
      return result;
    } catch (error) {
      return { error: true, message: "Error fetching car model." };
    }
  }

  async editCarModel(id, carModel) {
    try {
      await CarModelService.edit(id, carModel);
      return { error: false, message: "Car Model edited successfully." };
    } catch (error) {
      return { error: true, message: "Error updating Car Model." };
    }
  }
}

export default new CarModelStore();