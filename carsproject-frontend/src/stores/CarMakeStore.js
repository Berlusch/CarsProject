import { makeAutoObservable } from 'mobx';
import axios from 'axios';

class CarMakeStore {
  carMakes = [];

  constructor() {
    makeAutoObservable(this);
  }

  async fetchCarMakes() {
    try {
      const response = await axios.get('https://your-backend-api.com/car-makes');
      this.carMakes = response.data; // Postavi podatke u store
    } catch (error) {
      console.error('Error fetching car makes:', error);
    }
  }

  setCarMakes(makes) {
    this.carMakes = makes;
  }

  addCarMake(make) {
    this.carMakes.push(make);
  }
}

const carMakeStore = new CarMakeStore();
export default carMakeStore;