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
    public class CarRegistrationController : ControllerBase
    {
        private readonly ICarRegistrationService _carRegistrationService;
        private readonly IMapper _mapper;

        public CarRegistrationController(ICarRegistrationService carRegistrationService, IMapper mapper)
        {
            _carRegistrationService = carRegistrationService;
            _mapper = mapper;
        }

        [HttpPost("pfs")]
        public async Task<ActionResult> GetPfs([FromBody] PFSParameters pfs)
        {
            // Default paging for registrations
            if (pfs.Paging.PageNumber <= 0) pfs.Paging.PageNumber = 1;
            if (pfs.Paging.PageSize <= 0) pfs.Paging.PageSize = 5;

            var registrations = await _carRegistrationService.GetCarRegistrationsAsync(pfs);

            if (registrations == null || !registrations.Any())
                return NotFound("No car registrations found.");

            var dtos = _mapper.Map<IEnumerable<CarRegistrationReadDto>>(registrations);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarRegistrationReadDto>> GetById(int id)
        {
            var registration = await _carRegistrationService.GetCarRegistrationByIdAsync(id);
            if (registration == null)
                return NotFound();

            return Ok(_mapper.Map<CarRegistrationReadDto>(registration));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarRegistrationInsertUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<CarRegistration>(dto);
            var created = await _carRegistrationService.AddCarRegistrationAsync(entity);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<CarRegistrationReadDto>(created)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarRegistrationInsertUpdateDto dto)
        {
            var entity = _mapper.Map<CarRegistration>(dto);
            var updated = await _carRegistrationService.UpdateCarRegistrationAsync(id, entity);

            if (updated == null)
                return NotFound();

            return Ok(_mapper.Map<CarRegistrationReadDto>(updated));
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
