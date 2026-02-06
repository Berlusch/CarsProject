import { HttpService } from "./HttpService";

async function getCarEngineTypesPFS(pfs) {
  const response = await HttpService.post('/CarEngineType/pfs', pfs, {
    headers: { 'Content-Type': 'application/json' }
  });
  return response.data;
}

export default {
  getCarEngineTypesPFS
};

