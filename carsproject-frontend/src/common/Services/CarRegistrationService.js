import { HttpService } from "./HttpService"; 

async function getCarRegistrationsPFS(currentPage = 1, pageSize = 5, sortBy = "name", searchTerm = "") {
  try {
    const payload = {
      pfs: {
        paging: { pageNumber: currentPage - 1, pageSize }, 
        sorting: { orderBy: sortBy, descending: false },
        filter: { propertyName: "name", filter: searchTerm }
      }
    };

    const response = await HttpService.post('/CarRegistration/pfs', payload, {
      headers: { 'Content-Type': 'application/json' }
    });

    return response.data; 
  } catch (error) {
    throw error;
  }
}


async function getById(id) {
  return await HttpService.get('/CarRegistration/' + id)
    .then((response)=>{
      return{error:false, message: response.data};
    })
    .catch((e)=>{})
}


async function add(CarRegistration) {
  try {
    const response = await HttpService.post('/CarRegistration', CarRegistration);
    return { error: false, message: 'Car registration added successfully' };
  } catch (error) {
    console.error('Error adding car registration:', error);
    return { error: true, message: 'Adding failed' };
  }
}

async function edit(id, CarRegistration){
    return await HttpService.put('/CarRegistration/'+id, CarRegistration)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing failed'}})
}

async function remove(id,CarRegistration){
    return await HttpService.delete('/CarRegistration/'+id, CarRegistration)
    .then(()=>{return{error:false, message: 'Removed'}})
    .catch(()=>{return{error:true, message:'Operation failed'}})
}

  
  export default{
    getCarRegistrationsPFS,
    getById,
    add,
    edit,
    remove    
}




