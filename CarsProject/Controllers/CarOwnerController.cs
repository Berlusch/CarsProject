using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarOwnerController : ControllerBase
    {
        private readonly ICarOwnerService _carOwnerService;
        private readonly IMapper _mapper;

        public CarOwnerController(ICarOwnerService carOwnerService, IMapper mapper)
        {
            _carOwnerService = carOwnerService;
            _mapper = mapper;
        }

        [HttpGet("getPfs")]
        public async Task<ActionResult<IEnumerable<CarOwnerDTORead>>> GetPfs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string sortBy = "last name",
            [FromQuery] string filter = "")
        {
            var carOwners = await _carOwnerService.GetCarOwnersPagedAsync(pageNumber, pageSize, sortBy, filter);
            var carOwnerDtos = _mapper.Map<IEnumerable<CarOwnerDTORead>>(carOwners);
            return Ok(carOwnerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarOwnerDTORead>> GetById(int id)
        {
            var carOwner = await _carOwnerService.GetCarOwnerByIdAsync(id);
            if (carOwner == null)
                return NotFound();

            return Ok(_mapper.Map<CarOwnerDTORead>(carOwner));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarOwnerDTOInsertUpdate carOwnerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var carOwner = _mapper.Map<CarOwner>(carOwnerDto); 
            var createdCarOwner = await _carOwnerService.AddCarOwnerAsync(carOwnerDto);

            return CreatedAtAction(nameof(GetById), new { id = createdCarOwner.Id }, _mapper.Map<CarOwnerDTORead>(createdCarOwner)); // Mapiraj domain model u DTO
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarOwnerDTOInsertUpdate carOwnerDto)
        {
            var updatedCarOwner = await _carOwnerService.UpdateCarOwnerAsync(id, carOwnerDto);

            if (updatedCarOwner == null)
                return NotFound();

            return Ok(updatedCarOwner);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _carOwnerService.DeleteCarOwnerAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

