import { makeAutoObservable, toJS } from "mobx";

class SharedStore {
  carOwners = [];
  carRegistrations = [];
  carModels = [];
  carMakes = [];
  searchTerm = "";  

  constructor() {
    makeAutoObservable(this);
  }

  setCarOwners(owners) {
    this.carOwners = owners;
    console.log("Owners set in sharedStore (toJS): ", toJS(this.carOwners));
  }

  setCarRegistrations(registrations) {
    this.carRegistrations = registrations;
  }

  setCarModels(models) {
    this.carModels = models;
  }

  setCarMakes(makes) {
    this.carMakes = makes;
  }

  // Funkcija za filtriranje carOwners prema više parametara
  get filteredCarOwners() {
    console.log("Filtered car owners: ", toJS(this.carOwners));
    if (!this.searchTerm) {
      return this.carOwners;  
    }
  
    return this.carOwners.filter(owner => {
      return (
        owner.firstName.toLowerCase().includes(this.searchTerm.toLowerCase()) &&
        owner.lastName.toLowerCase().includes(this.searchTerm.toLowerCase()) 
        /*this.carModels.some(model => model.carOwnerId === owner.id && model.name.toLowerCase().includes(this.searchTerm.toLowerCase())) ||
        this.carMakes.some(make => make.carOwnerId === owner.id && make.name.toLowerCase().includes(this.searchTerm.toLowerCase()))*/
      );
    });
  }  

  setSearchTerm(term) {
    this.searchTerm = term;
  }
}

const sharedStore = new SharedStore();
export default sharedStore;
