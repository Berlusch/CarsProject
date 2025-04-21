import { HttpService } from "./HttpService"; 

async function getCarRegistrationsPFS(page = 1, pageSize = 5, sort = "registrationNumber", filter = "") {
  try {
    const response = await HttpService.get('/CarRegistration/getPfs', {
      params: { pageNumber: page, pageSize: pageSize, sortBy: sort, filter:filter },
    });
    console.log(response.data);
    if (response.data.items && response.data.items.length === 0) {
      console.log("Sorry, no more data available, please go back!");
    }
    return response.data;
  } catch (error) {
    console.error("Fetching data error:", error);
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
    return { error: true, message: 'Problem adding car registration' };
  }
}

async function edit(id, CarRegistration){
    return await HttpService.put('/CarRegistration/'+id, CarRegistration)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing problem'}})
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




