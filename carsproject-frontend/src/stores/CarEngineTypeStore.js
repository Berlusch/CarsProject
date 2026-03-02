import { makeAutoObservable, runInAction } from "mobx";
import CarEngineTypeService from "../common/Services/CarEngineTypeService";

class CarEngineTypeStore {
  carEngineTypes = [];
  currentPage = 1;
  pageSize = 5;
  hasNextPage = false;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  setPage(page) {
    this.currentPage = page;
    this.fetchCarEngineTypes();
  }

  async fetchCarEngineTypes() {
    this.loading = true;
    this.error = null;

    try {
      const pfs = {
        paging: { pageNumber: this.currentPage, pageSize: this.pageSize },
        sorting: { orderBy: "Type", descending: false },
        filter: { propertyName: "Type", filter: "" }
      };

      const response = await CarEngineTypeService.getCarEngineTypesPFS(pfs);

      runInAction(() => {
        this.carEngineTypes = response.items ?? [];
        this.hasNextPage = response.hasNextPage ?? false;
      });
    } catch (error) {
      runInAction(() => {
        this.error = "Error fetching engine types.";
      });
      console.error(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }
}

export default new CarEngineTypeStore();