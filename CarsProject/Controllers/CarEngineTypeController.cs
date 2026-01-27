using AutoMapper;
using CarsProject.Common;
using CarsProject.WebApi.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarEngineTypeController : ControllerBase
    {
        private readonly ICarEngineTypeService _carEngineTypeService;
        private readonly IMapper _mapper;

        public CarEngineTypeController(ICarEngineTypeService carEngineTypeService, IMapper mapper)
        {
            _carEngineTypeService = carEngineTypeService;
            _mapper = mapper;
        }

        [HttpGet("getPfs")]
        public async Task<ActionResult> GetPfs(
            [FromQuery] PagingParameters paging,
            [FromQuery] SortingParameters sorting,
            [FromQuery] FilterParameters filter)
        {
                var carEngineTypes = await _carEngineTypeService.GetCarEngineTypesPagedAsync(
                paging,
                sorting,
                filter
            );

            if (carEngineTypes == null || !carEngineTypes.Any())
                return NotFound("No car engine types found.");

            var carEngineTypeDtos = _mapper.Map<IEnumerable<CarEngineTypeReadDto>>(carEngineTypes);
            return Ok(carEngineTypeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarEngineTypeReadDto>> GetById(int id)
        {
            var carEngineType = await _carEngineTypeService.GetCarEngineTypeByIdAsync(id);
            if (carEngineType == null)
                return NotFound();

            return Ok(_mapper.Map<CarEngineTypeReadDto>(carEngineType));
        }
    }
}


