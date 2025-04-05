using Microsoft.AspNetCore.Mvc;
using CarsProject.Model;
using CarsProject.Repository.Common;

namespace CarsProject.Controllers
{    
        [ApiController]
        [Route("api/[controller]")]
        public class CarMakeController : ControllerBase
        {
        private readonly IUnitOfWork _unitOfWork;

        public CarMakeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarMake>>> GetAll()
        {
            var carMakes = await _unitOfWork.CarMakeRepository.GetAllAsync();
            return Ok(carMakes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CarMake>> GetById(int id)
        {
            var carMake = await _unitOfWork.CarMakeRepository.GetByIdAsync(id);

            if (carMake == null)
                return NotFound();

            return Ok(carMake);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CarMake carMake)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.CarMakeRepository.AddAsync(carMake);
            return CreatedAtAction(nameof(GetById), new { id = carMake.Id }, carMake);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarMake carMake)
        {
            if (id != carMake.Id)
                return BadRequest("ID mismatch");

            await _unitOfWork.CarMakeRepository.UpdateAsync(carMake);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfWork.CarMakeRepository.DeleteAsync(id);
            return NoContent();
        }
    }
    
}
