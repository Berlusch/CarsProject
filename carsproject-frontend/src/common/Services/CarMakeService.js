import { HttpService } from "./HttpService"; 

async function getCarMakesPFS(pfs) {
  const response = await HttpService.post('/CarMake/pfs', pfs, {
    headers: { 'Content-Type': 'application/json' }
  });
  return response.data;
}

async function getById(id) {
  try {
    const response = await HttpService.get('/CarMake/' + id);
    return { error: false, message: response.data };
  } catch (error) {
    return { error: true, message: 'Fetching by ID failed' };
  }
}

async function add(carMake) {
  try {
    await HttpService.post('/CarMake', carMake);
    return { error: false, message: 'Car make added successfully' };
  } catch (error) {
    console.error('Error while adding car make:', error);
    return { error: true, message: 'Problem adding a car make' };
  }
}

async function edit(id, carMake) {
  try {
    await HttpService.put('/CarMake/' + id, carMake);
    return { error: false, message: 'Edited' };
  } catch {
    return { error: true, message: 'Editing failed' };
  }
}

async function remove(id) {
  try {
    await HttpService.delete('/CarMake/' + id);
    return { error: false, message: 'Removed' };
  } catch {
    return { error: true, message: 'Operation failed' };
  }
}

export default {
  getCarMakesPFS,
  getById,
  add,
  edit,
  remove
}