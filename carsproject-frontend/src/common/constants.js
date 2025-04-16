export const RouteNames={
    HOME: '/',

    CARMAKE_LIST: '/carmakes',
    CARMAKE_ADD: '/carmakes/add',
    CARMAKE_EDIT: '/carmakes/:id',
    CARMAKE_DELETE: '/carmakes/delete/:id',

    CARMODEL_LIST: '/carmodels',
    CARMODEL_ADD: '/carmodels/dodaj',
    CARMODEL_EDIT: '/carmodels/:sifra',
    CARMODEL_DELETE: '/carmodels/obrisi/:sifra',

    CAROWNER_LIST: '/carowners',
    CAROWNER_ADD: '/carowners/add',
    CAROWNER_EDIT: '/carowners/:id',
    CAROWNER_DELETE: '/carowners/delete/:id',

    CARREGISTRATION_LIST: '/carregistrations',
    CARREGISTRATION_ADD: '/carregistrations/add',
    CARREGISTRATION_EDIT: '/carregistrations/:id',
    CARREGISTRATION_DELETE: '/carregistrations/delete/:id',

    CARENGINETYPE_LIST: '/carenginetypes',
    CARENGINETYPE_ADD: '/carenginetypes/add',
    CARENGINETYPE_EDIT: '/carenginetypes/:id',
    CARENGINETYPE_DELETE: '/carenginetypes/delete/:id'    
    
}


export const Backend_URL ='https://localhost:7023/swagger/index.html';