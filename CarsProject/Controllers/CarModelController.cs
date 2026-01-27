using AutoMapper;
using CarsProject.WebApi;
using CarsProject.WebApi.DTO;
using CarsProject.Service;
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

        [HttpGet("getPfs")]
        public async Task<ActionResult<IEnumerable<CarModelReadDto>>> GetPfs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string sortBy = "name",
            [FromQuery] string filter = "")
        {
            var carModels = await _carModelService.GetCarModelsPagedAsync(pageNumber, pageSize, sortBy, filter);
            var carModelDtos = _mapper.Map<IEnumerable<CarModelReadDto>>(carModels);
            return Ok(carModelDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarModelReadDto>> GetById(int id)
        {
            var carModel = await _carModelService.GetCarModelByIdAsync(id);
            if (carModel == null)
                return NotFound();

            return Ok(_mapper.Map<CarModelReadDto>(carModel));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarModelInsertUpdateDto carModelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var carModel = _mapper.Map<CarModel>(carModelDto); 
            var createdCarModel = await _carModelService.AddCarModelAsync(carModelDto);

            return CreatedAtAction(nameof(GetById), new { id = createdCarModel.Id }, _mapper.Map<CarModelReadDto>(createdCarModel)); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarModelInsertUpdateDto carModelDto)
        {
            var updatedCarModel = await _carModelService.UpdateCarModelAsync(id, carModelDto);

            if (updatedCarModel == null)
                return NotFound();

            return Ok(updatedCarModel); // Već je DTORead iz servisa
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _carModelService.DeleteCarModelAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

