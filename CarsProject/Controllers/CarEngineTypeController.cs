using AutoMapper;
using CarsProject.Common;
using CarsProject.Service;
using CarsProject.Service.Common;
using CarsProject.WebApi.DTO;
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

        [HttpPost("pfs")]
        public async Task<ActionResult> GetPfs([FromBody] PFSParameters pfs)
        {
            if (pfs.Paging.PageNumber <= 0) pfs.Paging.PageNumber = 1;
            if (pfs.Paging.PageSize <= 0) pfs.Paging.PageSize = 1000;

            var carEngineTypes = await _carEngineTypeService.GetCarEngineTypesAsync(pfs);

            if (!carEngineTypes.Any())
                return NotFound();

            var dtos = _mapper.Map<IEnumerable<CarEngineTypeReadDto>>(carEngineTypes);
            return Ok(dtos);
        }
        

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarEngineTypeReadDto>> GetById(int id)
        {
            var carEngineType = await _carEngineTypeService.GetCarEngineTypeByIdAsync(id);
            if (carEngineType == null)
                return NotFound();

            return Ok(_mapper.Map<CarEngineTypeReadDto>(carEngineType));
        }
    }
}
