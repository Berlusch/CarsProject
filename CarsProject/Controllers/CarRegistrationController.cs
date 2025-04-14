using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarRegistrationController : ControllerBase
    {
        private readonly ICarRegistrationService _carRegistrationService;
        private readonly IMapper _mapper;

        public CarRegistrationController(ICarRegistrationService carRegistrationService, IMapper mapper)
        {
            _carRegistrationService = carRegistrationService;
            _mapper = mapper;
        }

        [HttpGet("getPfs")]
        public async Task<ActionResult<IEnumerable<CarRegistrationDTORead>>> GetPFS(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string sortBy = "name",
            [FromQuery] string filter = "")
        {
            var carRegistrations = await _carRegistrationService.GetCarRegistrationsPagedAsync(pageNumber, pageSize, sortBy, filter);
            var carRegistrationDtos = _mapper.Map<IEnumerable<CarRegistrationDTORead>>(carRegistrations);
            return Ok(carRegistrationDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarRegistrationDTORead>> GetById(int id)
        {
            var carRegistration = await _carRegistrationService.GetCarRegistrationByIdAsync(id);
            if (carRegistration == null)
                return NotFound();

            return Ok(_mapper.Map<CarRegistrationDTORead>(carRegistration));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarRegistrationDTOInsertUpdate carRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var carRegistration = _mapper.Map<CarRegistration>(carRegistrationDto); // Mapiraj DTO u domain model
            var createdCarRegistration = await _carRegistrationService.AddCarRegistrationAsync(carRegistrationDto);

            return CreatedAtAction(nameof(GetById), new { id = createdCarRegistration.Id }, _mapper.Map<CarRegistrationDTORead>(createdCarRegistration)); // Mapiraj domain model u DTO
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarRegistrationDTOInsertUpdate carRegistrationDto)
        {
            var updatedCarRegistration = await _carRegistrationService.UpdateCarRegistrationAsync(id, carRegistrationDto);

            if (updatedCarRegistration == null)
                return NotFound();

            return Ok(updatedCarRegistration); // Već je DTORead iz servisa
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _carRegistrationService.DeleteCarRegistrationAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}


