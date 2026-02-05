using AutoMapper;
using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Service.Common;
using CarsProject.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarMakeController : ControllerBase
    {
        private readonly ICarMakeService _carMakeService;
        private readonly IMapper _mapper;

        public CarMakeController(ICarMakeService carMakeService, IMapper mapper)
        {
            _carMakeService = carMakeService;
            _mapper = mapper;
        }

        [HttpPost("pfs")]
        public async Task<ActionResult> GetPfs([FromBody] PFSParameters pfs)
        {
            var pagedResult = await _carMakeService.GetCarMakesAsync(pfs);

            if (!pagedResult.Items.Any())
                return NotFound();

            var dtos = _mapper.Map<IEnumerable<CarMakeReadDto>>(pagedResult.Items);

            var response = new PFSResponseDto<CarMakeReadDto>
            {
                Items = dtos,
                TotalCount = pagedResult.TotalCount,
                HasNextPage = pagedResult.TotalPages > pagedResult.Paging.PageNumber
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var carMake = await _carMakeService.GetCarMakeByIdAsync(id);
            return Ok(_mapper.Map<CarMakeReadDto>(carMake));
        }
                
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarMakeInsertUpdateDto dto)
        {
            var carMake = _mapper.Map<CarMake>(dto);
            var created = await _carMakeService.AddCarMakeAsync(carMake);

            var readDto = _mapper.Map<CarMakeReadDto>(created);

            return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
        }
                
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CarMakeInsertUpdateDto dto)
        {
            var carMake = _mapper.Map<CarMake>(dto);
            var updated = await _carMakeService.UpdateCarMakeAsync(id, carMake);

            return Ok(_mapper.Map<CarMakeReadDto>(updated));
        }
       
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _carMakeService.DeleteCarMakeAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
