using AutoMapper;
using CarsProject.DAL;
using CarsProject.Model;
using CarsProject.Model.DTO;


namespace CarsProject.Mapping
{
    public class CarsProjectMappingProfile: Profile
    {
        public CarsProjectMappingProfile()
        {
            //CarEngineType Mapping

            CreateMap<CarsProject.DAL.CarEngineType, CarsProject.Model.DTO.CarEngineTypeDTORead>();

            //CarOwner Mapping

            CreateMap<CarsProject.DAL.CarOwner, CarsProject.Model.DTO.CarOwnerDTORead>();
            CreateMap<CarsProject.Model.DTO.CarOwnerDTOInsertUpdate, CarsProject.DAL.CarOwner>();

            //CarMake Mapping

            CreateMap<CarsProject.DAL.CarMake, CarsProject.Model.DTO.CarMakeDTORead>();
            CreateMap<CarsProject.Model.DTO.CarMakeDTOInsertUpdate, CarsProject.DAL.CarMake>();

            //CarModel Mapping

            CreateMap<CarsProject.DAL.CarModel, CarsProject.Model.DTO.CarModelDTORead>();
            CreateMap<CarsProject.Model.DTO.CarModelDTOInsertUpdate, CarsProject.DAL.CarModel>();

            CreateMap<CarsProject.DAL.CarModel, CarsProject.Model.DTO.CarModelDTORead>().ForCtorParam(
                   "CarMakeName",
                   opt => opt.MapFrom(src => src.CarMake.Name)
               );

            CreateMap<CarsProject.Model.DTO.CarModelDTOInsertUpdate, CarsProject.DAL.CarModel>().ForMember(
                    dest => dest.CarMake,
                    opt => opt.MapFrom(src => src.CarMakeId)
                );

            CreateMap<CarsProject.DAL.CarModel, CarsProject.Model.DTO.CarModelDTORead>().ForCtorParam(
                   "CarEngineTypeType",
                   opt => opt.MapFrom(src => src.CarEngineType.Type)
               );

            CreateMap<CarsProject.Model.DTO.CarModelDTOInsertUpdate, CarsProject.DAL.CarModel>().ForMember(
                    dest => dest.CarEngineType,
                    opt => opt.MapFrom(src => src.CarEngineTypeId)
                );

            //CarRegistration Mapping

            CreateMap<CarsProject.DAL.CarRegistration, CarsProject.Model.DTO.CarRegistrationDTORead>();
            CreateMap<CarsProject.Model.DTO.CarRegistrationDTOInsertUpdate, CarsProject.DAL.CarRegistration>();

            CreateMap<CarsProject.DAL.CarRegistration, CarsProject.Model.DTO.CarRegistrationDTORead>().ForCtorParam(
                    "CarModelName",
                    opt => opt.MapFrom(src => src.CarModel.Name)
                );
            
            CreateMap<CarsProject.Model.DTO.CarRegistrationDTOInsertUpdate, CarsProject.DAL.CarRegistration>().ForMember(
                    dest => dest.CarModel,
                    opt => opt.MapFrom(src => src.CarModelId)
                );

            CreateMap<CarsProject.DAL.CarRegistration, CarsProject.Model.DTO.CarRegistrationDTORead>().ForCtorParam(
                    "CarOwnerFirstNameLastName",
                    opt => opt.MapFrom(src => $"{src.CarOwner.FirstName} {src.CarOwner.LastName}")
                );

            CreateMap<CarsProject.Model.DTO.CarRegistrationDTOInsertUpdate, CarsProject.DAL.CarRegistration>().ForMember(
                    dest => dest.CarOwner,
                    opt => opt.MapFrom(src => src.CarOwnerId)
                );


        }
    }
    
    
}
