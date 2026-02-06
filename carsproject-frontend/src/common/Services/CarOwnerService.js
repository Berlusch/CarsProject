import { HttpService } from "./HttpService"; 

async function getCarOwnersPFS(pfs) {
  const response = await HttpService.post('/CarOwner/pfs', pfs, {
    headers: { 'Content-Type': 'application/json' }
  });
  return response.data;
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
    return { error: true, message: 'Adding failed' };
  }
}

async function edit(id, CarOwner){
    return await HttpService.put('/CarOwner/'+id, CarOwner)
    .then(()=>{return{error:false, message: 'Edited'}})
    .catch(()=>{return{error:true, message:'Editing failed'}})
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




