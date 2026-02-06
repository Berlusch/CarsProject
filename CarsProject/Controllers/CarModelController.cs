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
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelService _carModelService;
        private readonly IMapper _mapper;

        public CarModelController(ICarModelService carModelService, IMapper mapper)
        {
            _carModelService = carModelService;
            _mapper = mapper;
        }

        [HttpPost("pfs")]
        public async Task<ActionResult> GetPfs([FromBody] PFSParameters pfs)
        {
            var pagedResult = await _carModelService.GetCarModelsAsync(pfs);

            var dtos = _mapper.Map<IEnumerable<CarModelReadDto>>(pagedResult.Items);

            var response = new PFSResponseDto<CarModelReadDto>
            {
                Items = dtos.ToList(),
                TotalCount = pagedResult.TotalCount,
                HasNextPage = pagedResult.TotalPages > pagedResult.Paging.PageNumber
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var carModel = await _carModelService.GetCarModelByIdAsync(id);
            return Ok(_mapper.Map<CarModelReadDto>(carModel));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarModelInsertUpdateDto dto)
        {
            var carModel = _mapper.Map<CarModel>(dto);
            var created = await _carModelService.AddCarModelAsync(carModel);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<CarModelReadDto>(created)
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CarModelInsertUpdateDto dto)
        {
            var carModel = _mapper.Map<CarModel>(dto);
            var updated = await _carModelService.UpdateCarModelAsync(id, carModel);

            return Ok(_mapper.Map<CarModelReadDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _carModelService.DeleteCarModelAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
