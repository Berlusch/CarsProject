using AutoMapper;
using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.WebApi.DTO;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarModelServiceTests
    {
        private readonly Mock<IGenericRepository<CarModel>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CarModelService _service;

        public CarModelServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<CarModel>>();
            _mockMapper = new Mock<IMapper>();
            _service = new CarModelService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCarModelByIdAsync_ReturnsCorrectCarModelDTO()
        {
            var id = 1;
            var carModel = new CarModel
            {
                Id = id,
                Name = "Model A",
                Abrv = "MA",
                CarMake = new CarMake { Id = 1, Name = "Make A", Abrv = "MK" },
                CarEngineType = new CarEngineType { Id = 1, Type = "Gasoline", Abrv = "GAS" }
            };
            var expectedDto = new CarModelReadDto(id, "Model A", "MA", "Make A", "Gasoline");

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carModel);
            _mockMapper.Setup(m => m.Map<CarModelReadDto>(carModel)).Returns(expectedDto);

            var result = await _service.GetCarModelByIdAsync(id);

            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetCarModelsPagedAsync_ReturnsPagedFilteredSortedCarModels()
        {            
            var pfs = new PFSParameters
            {
                Paging = new PagingParameters { PageNumber = 1, PageSize = 2 },
                Sorting = new SortingParameters { OrderBy = "name", Descending = true },
                Filter = new FilterParameters { PropertyName = "Id", Filter = "1" }
            };

            // mock podaci
            var carModels = new List<CarModel>
    {
        new CarModel
        {
            Id = 1,
            Name = "Model A",
            Abrv = "MA",
            CarMake = new CarMake { Id = 1, Name = "Make A", Abrv = "MK" },
            CarEngineType = new CarEngineType { Id = 1, Type = "Gasoline", Abrv = "GAS" }
        },
        new CarModel
        {
            Id = 2,
            Name = "Model B",
            Abrv = "MB",
            CarMake = new CarMake { Id = 2, Name = "Make B", Abrv = "MB" },
            CarEngineType = new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DIE" }
        }
    };
           
            var expectedDTOs = new List<CarModelReadDto>
    {
        new CarModelReadDto(1, "Model A", "MA", "Make A", "Gasoline"),
        new CarModelReadDto(2, "Model B", "MB", "Make B", "Diesel")
    };
                       
            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PFSParameters>())).Returns(carModels.AsQueryable());
                        
            _mockMapper.Setup(m => m.Map<IEnumerable<CarModelReadDto>>(It.IsAny<IEnumerable<CarModel>>()))
                       .Returns(expectedDTOs);
                        
            var result = await _service.GetCarModelsAsync(pfs);
                        
            result.Should().BeEquivalentTo(expectedDTOs);
        }


        [Fact]
        public async Task AddCarModelAsync_ReturnsAddedCarModelDTO()
        {
            var carModelDto = new CarModelInsertUpdateDto("Model A", "MA", 1, 2);
            var carMake = new CarMake { Id = 1, Name = "Make A", Abrv = "MK" };
            var carEngineType = new CarEngineType { Id = 2, Type = "Gasoline", Abrv = "GAS" };

            var carModel = new CarModel
            {
                Id = 1,
                Name = "Model A",
                Abrv = "MA",
                CarMake = carMake,
                CarEngineType = carEngineType
            };

            var expectedDto = new CarModelReadDto(1, "Model A", "MA", "Make A", "Gasoline");

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                     .Returns(new List<CarModel>().AsQueryable());
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<CarModel>())).ReturnsAsync(carModel);
            _mockMapper.Setup(m => m.Map<CarModelReadDto>(carModel)).Returns(expectedDto);

            var result = await _service.AddCarModelAsync(carModel);

            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task UpdateCarModelAsync_ValidInput_UpdatesAndReturnsDTO()
        {
            var id = 1;
            var carModelDto = new CarModelInsertUpdateDto("Corolla", "COR", 1, 2);

            var existingCarModel = new CarModel
            {
                Id = id,
                CarMake = new CarMake { Id = 0, Name = "Dummy", Abrv = "DUM" },
                CarEngineType = new CarEngineType { Id = 0, Type = "DummyEngine", Abrv = "DME" }
            };

            var updatedCarModel = new CarModel
            {
                Id = id,
                Name = "Corolla",
                Abrv = "COR",
                CarMake = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DIE" }
            };

            var expectedDto = new CarModelReadDto(id, "Corolla", "COR", "Toyota", "Diesel");

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingCarModel);
            _mockRepo.Setup(r => r.UpdateAsync(existingCarModel)).ReturnsAsync(updatedCarModel);
            _mockMapper.Setup(m => m.Map<CarModelReadDto>(updatedCarModel)).Returns(expectedDto);

            var result = await _service.UpdateCarModelAsync(id, updatedCarModel);

            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task DeleteCarModelAsync_ReturnsTrue_WhenCarModelIsDeleted()
        {
            var id = 1;
            var carModel = new CarModel
            {
                Id = id,
                Name = "Corolla",
                Abrv = "COR",
                CarMake = new CarMake { Id = 1, Name = "Make X", Abrv = "MX" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Gasoline", Abrv = "GAS" }
            };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carModel);
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteCarModelAsync(id);

            result.Should().BeTrue();
            _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
