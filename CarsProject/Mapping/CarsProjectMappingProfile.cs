using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;


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
            CreateMap<CarModel, CarsProject.Model.DTO.CarModelDTORead>()
                .ForMember(dest => dest.CarMakeName, opt => opt.MapFrom(src => src.CarMake.Name))
                .ForMember(dest => dest.CarEngineTypeType, opt => opt.MapFrom(src => src.CarEngineType.Type));


            CreateMap<CarModelDTOInsertUpdate, CarModel>()
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

