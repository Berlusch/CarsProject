import { HttpService } from "./HttpService"; 

async function getCarMakesPFS(currentPage = 1, pageSize = 5, sortBy = "name", searchTerm = "") {
  try {
    const payload = {
      pfs: {
        paging: { pageNumber: currentPage - 1, pageSize }, 
        sorting: { orderBy: sortBy, descending: false },
        filter: { propertyName: "name", filter: searchTerm }
      }
    };

    const response = await HttpService.post('/CarMake/pfs', payload, {
      headers: { 'Content-Type': 'application/json' }
    });

    return response.data; 
  } catch (error) {
    throw error;
  }
}
async function getById(id) {
  return await HttpService.get('/CarMake/' + id)
    .then((response)=>{
      return{error:false, message: response.data};
    })
    .catch((e)=>{})
}


async function add(CarMake) {
  try {
    await HttpService.post('/CarMake', CarMake);
    return { error: false, message: 'Car make added successfully' };
  } catch (error) {
    console.error('Error while adding car make:', error);
    return { error: true, message: 'Problem adding a car make' };
  }
}

async function edit(id, CarMake){
    return await HttpService.put('/CarMake/'+id, CarMake)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing failed'}})
}

async function remove(id,CarMake){
    return await HttpService.delete('/CarMake/'+id, CarMake)
    .then(()=>{return{error:false, message: 'Removed'}})
    .catch(()=>{return{error:true, message:'Operation failed'}})
}

  
  export default{
    getCarMakesPFS,
    getById,
    add,
    edit,
    remove    
}




