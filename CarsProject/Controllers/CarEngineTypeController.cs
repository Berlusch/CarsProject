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
            var pagedResult = await _carEngineTypeService.GetCarEngineTypesAsync(pfs);

            var dtos = _mapper.Map<IEnumerable<CarEngineTypeReadDto>>(pagedResult.Items);

            var response = new PFSResponseDto<CarEngineTypeReadDto>
            {
                Items = dtos.ToList(),
                TotalCount = pagedResult.TotalCount,
                HasNextPage = pagedResult.TotalPages > pagedResult.Paging.PageNumber
            };

            return Ok(response);
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
