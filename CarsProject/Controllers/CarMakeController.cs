using AutoMapper;
using CarsProject.Model.DTO;
using CarsProject.Service;
using Microsoft.AspNetCore.Mvc;
using CarsProject.Model;

namespace CarsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarMakeController : ControllerBase
    {
        private readonly ICarMakeService _carMakeService;
        private readonly IMapper _mapper;

        public CarMakeController(ICarMakeService carMakeService, IMapper mapper)
        {
            _carMakeService = carMakeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarMakeDTORead>>> GetAll()
        {
            var carMakes = await _carMakeService.GetAllCarMakesAsync();
            return Ok(carMakes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarMakeDTORead>> GetById(int id)
        {
            var carMake = await _carMakeService.GetCarMakeByIdAsync(id);
            if (carMake == null)
                return NotFound();
            return Ok(carMake);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarMakeDTOInsertUpdate carMakeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCarMake = await _carMakeService.AddCarMakeAsync(carMakeDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCarMake.Id }, createdCarMake);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarMakeDTOInsertUpdate carMakeDto)
        {
            var updatedCarMake = await _carMakeService.UpdateCarMakeAsync(id, carMakeDto);

            if (updatedCarMake == null)
            {
                return NotFound();  // Ako je ažuriranje propalo jer CarMake nije pronađen
            }

            return Ok(updatedCarMake);  // Vraćamo ažurirani CarMakeDTORead
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
