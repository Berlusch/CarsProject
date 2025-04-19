import { makeAutoObservable, runInAction } from "mobx";
import CarEngineTypeService from "../common/Services/CarEngineTypeService";

class CarEngineTypeStore {
  carEngineTypes = [];
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchCarEngineTypes() {
    this.loading = true;
    this.error = null;
    try {
      const items = await CarEngineTypeService.getCarEngineTypesListOnly();      
  
      if (items.length === 0) {
        console.log("No data available.");
      }
  
      runInAction(() => {
        this.carEngineTypes = items;
         
      });
    } catch (error) {
      runInAction(() => {
        this.error = "Fetching error";
      });
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  }
}


export default new CarEngineTypeStore();
