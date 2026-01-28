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
    public class CarOwnerController : ControllerBase
    {
        private readonly ICarOwnerService _carOwnerService;
        private readonly IMapper _mapper;

        public CarOwnerController(ICarOwnerService carOwnerService, IMapper mapper)
        {
            _carOwnerService = carOwnerService;
            _mapper = mapper;
        }

        [HttpPost("pfs")]
        public async Task<ActionResult> GetPfs([FromBody] PSFParameters pfs)
        {
            var carOwners = await _carOwnerService.GetCarOwnersAsync(pfs);

            if (!carOwners.Any())
                return NotFound();

            var dtos = _mapper.Map<IEnumerable<CarOwnerReadDto>>(carOwners);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var carOwner = await _carOwnerService.GetCarOwnerByIdAsync(id);
            return Ok(_mapper.Map<CarOwnerReadDto>(carOwner));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarOwnerInsertUpdateDto dto)
        {
            var carOwner = _mapper.Map<CarOwner>(dto);
            var created = await _carOwnerService.AddCarOwnerAsync(carOwner);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<CarOwnerReadDto>(created)
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CarOwnerInsertUpdateDto dto)
        {
            var carOwner = _mapper.Map<CarOwner>(dto);
            var updated = await _carOwnerService.UpdateCarOwnerAsync(id, carOwner);

            return Ok(_mapper.Map<CarOwnerReadDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _carOwnerService.DeleteCarOwnerAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
