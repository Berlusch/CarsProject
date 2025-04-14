using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<CarModelDTORead>>> GetPFS(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string sortBy = "name",
            [FromQuery] string filter = "")
        {
            var carModels = await _carModelService.GetCarModelsPagedAsync(pageNumber, pageSize, sortBy, filter);
            var carModelDtos = _mapper.Map<IEnumerable<CarModelDTORead>>(carModels);
            return Ok(carModelDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarModelDTORead>> GetById(int id)
        {
            var carModel = await _carModelService.GetCarModelByIdAsync(id);
            if (carModel == null)
                return NotFound();

            return Ok(_mapper.Map<CarModelDTORead>(carModel));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarModelDTOInsertUpdate carModelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var carModel = _mapper.Map<CarModel>(carModelDto); // Mapiraj DTO u domain model
            var createdCarModel = await _carModelService.AddCarModelAsync(carModelDto);

            return CreatedAtAction(nameof(GetById), new { id = createdCarModel.Id }, _mapper.Map<CarModelDTORead>(createdCarModel)); // Mapiraj domain model u DTO
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarModelDTOInsertUpdate carModelDto)
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

