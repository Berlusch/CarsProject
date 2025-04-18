import { HttpService } from "./HttpService"; 

async function getCarMakesPFS(page = 1, pageSize = 5, sort = "name", filter = "") {
  try {
    const response = await HttpService.get('/CarMake/getPfs', {
      params: { pageNumber: page, pageSize: pageSize, sortBy: sort, filter:filter },
    });
    console.log(response.data);
    return response.data;
  } catch (error) {
    console.error("Fetching data error:", error);
    throw error;
  }
}

async function getById(id){
    return await HttpService.get('/CarMake/'+id)
    .then((response)=>{
        //console.table(response.data)
        return {error:false, message: response.data};
    })
    .catch((e)=>{})
}

async function add(CarMake){
    return await HttpService.post('/CarMake', CarMake)
    .then(()=>{return{error:false, message: 'Added'}})
    .catch(()=>{return{error:true, message:'Adding problem'}})
}

async function edit(id,CarMake){
    return await HttpService.put('/CarMake/'+id, CarMake)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing problem'}})
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




