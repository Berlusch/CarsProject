import { HttpService } from "./HttpService";

async function getCarEngineTypesListOnly() {
  try {
    const pfs = {
  paging: { pageNumber: 1, pageSize: 1000 },
  filter: { propertyName: "Type", filter: "" },  
  sorting: { orderBy: "Type" }                 
};

    const response = await HttpService.post('/CarEngineType/pfs', pfs);    
    return response.data;    
  } catch (error) {
    console.error("Error while fetching engine types:", error);
    throw error;
  }
}

export default {
  getCarEngineTypesListOnly
};

