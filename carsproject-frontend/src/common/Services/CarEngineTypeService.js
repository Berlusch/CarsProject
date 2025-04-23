import { HttpService } from "./HttpService";

async function getCarEngineTypesListOnly() {
  try {
    const response = await HttpService.get('/CarEngineType/getPfs');    
    return response.data;    
  } catch (error) {
    console.error("Error while fetching engine types:", error);
    throw error;
  }
}

export default {
  getCarEngineTypesListOnly
};
