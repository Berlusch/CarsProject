using AutoMapper;
using CarsProject.WebApi;
using CarsProject.WebApi.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.Controllers
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

        [HttpGet("getPfs")]
        public async Task<ActionResult<IEnumerable<CarMakeDTORead>>> GetPfs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string sortBy = "name",
            [FromQuery] string filter = "")
        {
            var carMakes = await _carMakeService.GetCarMakesPagedAsync(pageNumber, pageSize, sortBy, filter);
            var carMakeDtos = _mapper.Map<IEnumerable<CarMakeDTORead>>(carMakes);
            return Ok(carMakeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarMakeDTORead>> GetById(int id)
        {
            var carMake = await _carMakeService.GetCarMakeByIdAsync(id);
            if (carMake == null)
                return NotFound();

            return Ok(_mapper.Map<CarMakeDTORead>(carMake)); 
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarMakeDTOInsertUpdate carMakeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var carMake = _mapper.Map<CarMake>(carMakeDto); 
            var createdCarMake = await _carMakeService.AddCarMakeAsync(carMakeDto);

            return CreatedAtAction(nameof(GetById), new { id = createdCarMake.Id }, _mapper.Map<CarMakeDTORead>(createdCarMake)); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarMakeDTOInsertUpdate carMakeDto)
        {
            var updatedCarMake = await _carMakeService.UpdateCarMakeAsync(id, carMakeDto);

            if (updatedCarMake == null)
                return NotFound();

            return Ok(updatedCarMake); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _carMakeService.DeleteCarMakeAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
