using AutoMapper;
using CarsProject.Model.DTO;
using CarsProject.Model;


namespace CarsProject.Mapping
{
    public class CarsProjectMappingProfile : Profile
    {
        public CarsProjectMappingProfile()
        {
            // CarEngineType Mapping
            CreateMap<CarEngineType, CarEngineTypeDTORead>();
            
            // CarOwner Mapping
            CreateMap<CarOwner, CarOwnerDTORead>();
            CreateMap<CarOwnerDTOInsertUpdate, CarOwner>();

            // CarMake Mapping
            CreateMap<CarMake, CarMakeDTORead>();
            CreateMap<CarMakeDTOInsertUpdate, CarMake>();

            // CarModel Mapping
            CreateMap<CarsProject.DAL.CarModel, CarsProject.Model.DTO.CarModelDTORead>()
                .ForCtorParam("CarMakeName", opt => opt.MapFrom(src => src.CarMake.Name))
                .ForCtorParam("CarEngineTypeType", opt => opt.MapFrom(src => src.CarEngineType.Type));

            CreateMap<CarModelDTOInsertUpdate, CarsProject.DAL.CarModel>()
                .ForMember(dest => dest.CarMake, opt => opt.MapFrom(src => src.CarMakeId));
                
            // CarRegistration Mapping
            CreateMap<CarRegistration, CarRegistrationDTORead>()
                .ForCtorParam("CarModelName", opt => opt.MapFrom(src => src.CarModel.Name))
                .ForCtorParam("CarOwnerFirstNameLastName", opt => opt.MapFrom(src => $"{src.CarOwner.FirstName} {src.CarOwner.LastName}"));

            CreateMap<CarRegistrationDTOInsertUpdate, CarRegistration>()
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.CarModelId))
                .ForMember(dest => dest.CarOwner, opt => opt.MapFrom(src => src.CarOwnerId));
        }
    }
}

