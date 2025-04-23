import { HttpService } from "./HttpService"; 

async function getCarOwnersPFS(page = 1, pageSize = 5, sort = "last name", filter = "") {
  try {
    const response = await HttpService.get('/CarOwner/getPfs', {
      params: { pageNumber: page, pageSize: pageSize, sortBy: sort, filter:filter },
    });
    console.log("Service data", response.data);
    if (response.data.items && response.data.items.length === 0 && page>1) {
      console.log("Sorry, no more data available, please go back!");
    }
    return response.data;
  } catch (error) {
    console.error("Fetching data error:", error);
    throw error;
  }
}

async function getById(id) {
  return await HttpService.get('/CarOwner/' + id)
    .then((response)=>{
      return{error:false, message: response.data};
    })
    .catch((e)=>{})
}


async function add(CarOwner) {
  try {
    await HttpService.post('/CarOwner', CarOwner);
    return { error: false, message: 'Car make added successfully' };
  } catch (error) {
    console.error('Error adding car make:', error);
    return { error: true, message: 'Problem adding car make' };
  }
}

async function edit(id, CarOwner){
    return await HttpService.put('/CarOwner/'+id, CarOwner)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing problem'}})
}

async function remove(id,CarOwner){
    return await HttpService.delete('/CarOwner/'+id, CarOwner)
    .then(()=>{return{error:false, message: 'Removed'}})
    .catch(()=>{return{error:true, message:'Operation failed'}})
}

  
  export default{
    getCarOwnersPFS,
    getById,
    add,
    edit,
    remove    
}




