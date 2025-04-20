import { HttpService } from "./HttpService"; 

async function getCarModelsPFS(page = 1, pageSize = 5, sort = "name", filter = "") {
  try {
    const response = await HttpService.get('/CarModel/getPfs', {
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
  return await HttpService.get('/CarModel/' + id)
    .then((response)=>{
      return{error:false, message: response.data};
    })
    .catch((e)=>{})
}


async function add(CarModel) {
  try {
    await HttpService.post('/CarModel', CarModel);
    return { error: false, message: 'Car make added successfully' };
  } catch (error) {
    console.error('Error adding car make:', error);
    return { error: true, message: 'Problem adding car model' };
  }
}

async function edit(id, CarModel){
    return await HttpService.put('/CarModel/'+id, CarModel)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing problem'}})
}

async function remove(id,CarModel){
    return await HttpService.delete('/CarModel/'+id, CarModel)
    .then(()=>{return{error:false, message: 'Removed'}})
    .catch(()=>{return{error:true, message:'Operation failed'}})
}

  
  export default{
    getCarModelsPFS,
    getById,
    add,
    edit,
    remove    
}




