import { makeAutoObservable, runInAction } from "mobx";
import CarOwnerService from "../common/Services/CarOwnerService";

class CarOwnerStore {
  carOwners = [];
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

  constructor() {
    makeAutoObservable(this);
  }

  setSearchTerm(term) {
    this.searchTerm = term;
    this.currentPage = 1;
    this.fetchCarOwners();
  }

  setPage(page) {
    this.currentPage = page;
    this.fetchCarOwners();
  }

  async fetchCarOwners() {
  this.loading = true;
  this.error = null;

  try {
    const search = this.searchTerm?.toLowerCase() || "";

    const pfs = {
      paging: { pageNumber: this.currentPage, pageSize: this.pageSize },
      sorting: { orderBy: "LastName", descending: false },
      filter: { propertyName: "LastName", filter: "" } 
    };

    const response = await CarOwnerService.getCarOwnersPFS(pfs);

    runInAction(() => {     
      this.carOwners = (response.items ?? []).filter(
        owner =>
          owner.firstName.toLowerCase().includes(search) ||
          owner.lastName.toLowerCase().includes(search)
      );

      this.hasNextPage = response.hasNextPage ?? false;
      this.loading = false;
    });
  } catch (error) {
    runInAction(() => {
      this.error = "Error fetching car owners.";
      this.loading = false;
    });
    console.error(error);
  }
}

  async addCarOwner(firstName, lastName, dateOfBirth) {
    runInAction(() => {
      this.addStatus = { error: false, message: "Adding car owner..." };
    });

    try {
      const result = await CarOwnerService.add({
        firstName,
        lastName,
        dateOfBirth
      });

      if (result?.error) {
        runInAction(() => {
          this.addStatus = {
            error: true,
            message: result.message || "Error adding car owner."
          };
        });
        return;
      }

      runInAction(() => {
        this.addStatus = {
          error: false,
          message: result.message || "Car owner added successfully."
        };
      });

      await this.fetchCarOwners(); 
    } catch (error) {
      runInAction(() => {
        this.addStatus = {
          error: true,
          message: "Problem adding car owner."
        };
      });
      console.error(error);
    }
  }

  async getCarOwnerById(id) {
    try {
      const result = await CarOwnerService.getById(id);

      if (result?.error) {
        return { error: true, message: "Car owner not found." };
      }

      return result;
    } catch (error) {
      return { error: true, message: "Error fetching car owner." };
    }
  }

  async editCarOwner(id, carOwner) {
    try {
      await CarOwnerService.edit(id, carOwner);
      return { error: false, message: "Car owner edited successfully." };
    } catch (error) {
      return { error: true, message: "Error updating car owner." };
    }
  }
}

export default new CarOwnerStore();
