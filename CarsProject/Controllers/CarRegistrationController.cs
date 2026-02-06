using AutoMapper;
using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Service;
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
            var pagedResult = await _carRegistrationService.GetCarRegistrationsAsync(pfs);

            var dtos = _mapper.Map<IEnumerable<CarRegistrationReadDto>>(pagedResult.Items);

            var response = new PFSResponseDto<CarRegistrationReadDto>
            {
                Items = dtos.ToList(),
                TotalCount = pagedResult.TotalCount,
                HasNextPage = pagedResult.TotalPages > pagedResult.Paging.PageNumber
            };

            return Ok(response);
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
