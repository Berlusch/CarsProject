using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<CarEngineTypeDTORead>>> GetPFS(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string sortBy = "type",
            [FromQuery] string filter = "")
        {
            var carEngineTypes = await _carEngineTypeService.GetCarEngineTypesPagedAsync(pageNumber, pageSize, sortBy, filter);
            var carEngineTypeDtos = _mapper.Map<IEnumerable<CarEngineTypeDTORead>>(carEngineTypes);
            return Ok(carEngineTypeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarEngineTypeDTORead>> GetById(int id)
        {
            var carEngineType = await _carEngineTypeService.GetCarEngineTypeByIdAsync(id);
            if (carEngineType == null)
                return NotFound();

            return Ok(_mapper.Map<CarEngineTypeDTORead>(carEngineType));
        }        


    }
}

