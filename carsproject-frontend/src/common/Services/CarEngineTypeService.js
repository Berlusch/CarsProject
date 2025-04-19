import { HttpService } from "./HttpService";

async function getCarEngineTypesListOnly() {
  try {
    const response = await HttpService.get('/CarEngineType/getPfs');
    console.log('API odgovor:', response.data);  
    return response.data;    
  } catch (error) {
    console.error("Greška prilikom dohvaćanja engine tipova:", error);
    throw error;
  }
}

export default {
  getCarEngineTypesListOnly
};
