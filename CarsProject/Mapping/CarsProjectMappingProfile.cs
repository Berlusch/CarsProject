using AutoMapper;
using CarsProject.Model;
using CarsProject.WebApi.DTO;

namespace CarsProject.WebApi.Mapping
{
    public class CarsProjectMappingProfile : Profile
    {
        public CarsProjectMappingProfile()        {
            
            CreateMap<CarEngineType, CarEngineTypeReadDto>();

            
            CreateMap<CarOwner, CarOwnerReadDto>();
            CreateMap<CarOwnerInsertUpdateDto, CarOwner>();

            
            CreateMap<CarMake, CarMakeReadDto>();
            CreateMap<CarMakeInsertUpdateDto, CarMake>();


            CreateMap<CarModel, CarModelReadDto>()
                .ForCtorParam("CarMakeName", opt => opt.MapFrom(src => src.CarMake.Name))
                .ForCtorParam("CarEngineTypeType", opt => opt.MapFrom(src => src.CarEngineType.Type));

            CreateMap<CarModelInsertUpdateDto, CarModel>()
                .ForMember(dest => dest.CarMakeId, opt => opt.MapFrom(src => src.CarMakeId))
                .ForMember(dest => dest.CarEngineTypeId, opt => opt.MapFrom(src => src.CarEngineTypeId))
                .ForMember(dest => dest.CarMake, opt => opt.Ignore())          
                .ForMember(dest => dest.CarEngineType, opt => opt.Ignore());

            CreateMap<CarModelInsertUpdateDto, CarModel>();
            CreateMap<CarModel, CarModelInsertUpdateDto>();
            
            CreateMap<CarRegistration, CarRegistrationReadDto>()
                .ForCtorParam("CarModelName", opt => opt.MapFrom(src => src.CarModel.Name))
                .ForCtorParam("CarOwnerFirstNameLastName", opt => opt.MapFrom(src => $"{src.CarOwner.FirstName} {src.CarOwner.LastName}"));

            CreateMap<CarRegistrationInsertUpdateDto, CarRegistration>()
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.CarModelId))
                .ForMember(dest => dest.CarOwner, opt => opt.MapFrom(src => src.CarOwnerId));

            CreateMap<CarRegistrationInsertUpdateDto, CarRegistration>();
            CreateMap<CarRegistration, CarRegistrationInsertUpdateDto>();
        }
    }
}
