using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarModelServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICarModelRepository> _mockCarModelRepository;
        private readonly CarModelService _service;
        private readonly Mock<ICarMakeRepository> _mockCarMakeRepository;
        private readonly Mock<ICarEngineTypeRepository> _mockCarEngineTypeRepository;
        private readonly Mock<ICarMakeService> _mockCarMakeService;
        
        public CarModelServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCarModelRepository = new Mock<ICarModelRepository>();
            _mockCarMakeRepository = new Mock<ICarMakeRepository>();
            _mockCarEngineTypeRepository = new Mock<ICarEngineTypeRepository>();
            _mockCarMakeService = new Mock<ICarMakeService>();
            
            _mockUnitOfWork.Setup(uow => uow.CarModelRepository).Returns(_mockCarModelRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.CarMakeRepository).Returns(_mockCarMakeRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.CarEngineTypeRepository).Returns(_mockCarEngineTypeRepository.Object);

            _service = new CarModelService(_mockUnitOfWork.Object, _mockMapper.Object);
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
                CarMake = new CarMake { Id = 1, Name = "Make A", Abrv="MK" }, 
                CarEngineType = new CarEngineType { Id = 1, Type = "Gasoline", Abrv="GAS" } 
            };
                        
            var expectedDto = new CarModelDTORead(id, "Model A", "MA", "Make A", "Gasoline");
                        
            _mockUnitOfWork.Setup(u => u.CarModelRepository.GetByIdAsync(id)).ReturnsAsync(carModel);
                        
            _mockMapper.Setup(m => m.Map<CarModelDTORead>(carModel)).Returns(expectedDto);
                       
            var result = await _service.GetCarModelByIdAsync(id);
                        
            result.Should().BeEquivalentTo(expectedDto);
        }


        [Fact]
        public async Task GetCarModelsPagedAsync_ReturnsPagedFilteredSortedCarModels()
        {
            
            var pageNumber = 1;
            var pageSize = 2;
            var sortBy = "name";
            var filter = "id";

            
            var carModels = new List<CarModel>
    {
        new CarModel
        {
            Id = 1,
            Name = "Model A",
            Abrv = "MA",
            CarMake = new CarMake { Id = 1, Name = "Make A", Abrv="MK" }, 
            CarEngineType = new CarEngineType { Id = 1, Type = "Gasoline", Abrv="GAS" } 
        },
        new CarModel
        {
            Id = 2,
            Name = "Model B",
            Abrv = "MB",
            CarMake = new CarMake { Id = 2, Name = "Make B", Abrv="MB" }, 
            CarEngineType = new CarEngineType { Id = 2, Type = "Diesel", Abrv="DIE" } 
        }
    };

            var expectedDTOs = new List<CarModelDTORead>
    {
        new CarModelDTORead(1, "Model A", "MA", "Make A", "Gasoline"),
        new CarModelDTORead(2, "Model B", "MB", "Make B", "Diesel")
    };

            
            _mockUnitOfWork.Setup(u => u.CarModelRepository.GetAllCarModelsAsync())
                .ReturnsAsync(carModels);  

            _mockMapper.Setup(m => m.Map<IEnumerable<CarModelDTORead>>(It.IsAny<IEnumerable<CarModel>>()))
                .Returns(expectedDTOs); 

            
            _mockUnitOfWork.Setup(u => u.CarModelRepository.GetAllCarModelsAsync()).ReturnsAsync(carModels);
            _mockMapper.Setup(m => m.Map<IEnumerable<CarModelDTORead>>(It.IsAny<IEnumerable<CarModel>>()))
                       .Returns(expectedDTOs);

           
            var result = await _service.GetCarModelsPagedAsync(pageNumber, pageSize, sortBy, filter);

            
            result.Should().BeEquivalentTo(expectedDTOs);
        }


        [Fact]
        public async Task AddCarModelAsync_ReturnsAddedCarModelDTO()
        {
            var carModelDto = new CarModelDTOInsertUpdate("Model A", "MA", 1, 2);
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

            var expectedDto = new CarModelDTORead(1, "Model A", "MA", "Make A", "Gasoline");
            
            var mockCarMakeRepository = new Mock<ICarMakeRepository>();
            mockCarMakeRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(carMake);

            var mockCarEngineTypeRepository = new Mock<ICarEngineTypeRepository>();
            mockCarEngineTypeRepository
                .Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(carEngineType);

            _mockUnitOfWork
                .Setup(u => u.CarMakeRepository)
                .Returns(mockCarMakeRepository.Object);

            _mockUnitOfWork
                .Setup(u => u.CarEngineTypeRepository)
                .Returns(mockCarEngineTypeRepository.Object);
            
            _mockMapper.Setup(m => m.Map<CarModel>(carModelDto)).Returns(carModel);
            _mockUnitOfWork.Setup(u => u.CarModelRepository.AddAsync(carModel)).ReturnsAsync(carModel);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CarModelDTORead>(carModel)).Returns(expectedDto);
            
            var result = await _service.AddCarModelAsync(carModelDto);
                        
            result.Should().BeEquivalentTo(expectedDto);
        }


        [Fact]
        public async Task UpdateCarModelAsync_ValidInput_UpdatesAndReturnsDTO()
        {            
            var id = 1;

            var carModelDto = new CarModelDTOInsertUpdate(
                Name: "Corolla",
                Abrv: "COR",
                CarMakeId: 1,
                CarEngineTypeId: 2

            );

            var existingCarModel = new CarModel { Id = id, CarMake = new CarMake{
                Id = 0,
                Name = "Dummy",
                Abrv = "DUM"
            }, CarEngineType=new CarEngineType{
                Id = 0,
                Type = "DummyEngine",
                Abrv = "DME"
            }
            };
            var carMake = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" };
            var carEngineType = new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DIE" };            

            _mockUnitOfWork.Setup(u => u.CarModelRepository.GetByIdAsync(id))
                .ReturnsAsync(existingCarModel);
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(carModelDto.CarMakeId))
                .ReturnsAsync(carMake);
            _mockUnitOfWork.Setup(u => u.CarEngineTypeRepository.GetByIdAsync(carModelDto.CarEngineTypeId))
                .ReturnsAsync(carEngineType);

            _mockMapper.Setup(m => m.Map(carModelDto, existingCarModel));
            _mockUnitOfWork.Setup(u => u.CarModelRepository.UpdateAsync(existingCarModel))
                .ReturnsAsync(existingCarModel);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            var expectedDto = new CarModelDTORead(
                Id: id,
                Name: "Corolla",
                Abrv: "COR",
                CarMakeName: "Golf",
                CarEngineTypeType: "Diesel"
            );

            _mockMapper.Setup(m => m.Map<CarModelDTORead>(existingCarModel))
                .Returns(expectedDto);
            
            var result = await _service.UpdateCarModelAsync(id, carModelDto);
            
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
            Assert.Equal(expectedDto.Abrv, result.Abrv);
            Assert.Equal(expectedDto.CarMakeName, result.CarMakeName);
            Assert.Equal(expectedDto.CarEngineTypeType, result.CarEngineTypeType);

        }


        [Fact]
        public async Task DeleteCarModelAsync_ReturnsTrue_WhenCarModelIsDeleted()
        {
            
            var id = 1;

           
            var carMake = new CarMake { Id = 1, Name = "Make X", Abrv="MX" };  
            var carEngineType = new CarEngineType { Id = 2, Type = "Gasoline", Abrv="GAS"}; 

            
            var carModel = new CarModel
            {
                Id = id,
                Name = "Corolla",
                Abrv = "COR",
                CarMake = carMake,  
                CarEngineType = carEngineType  
            };

            
            _mockUnitOfWork.Setup(u => u.CarModelRepository.GetByIdAsync(id)).ReturnsAsync(carModel);
            _mockUnitOfWork.Setup(u => u.CarModelRepository.DeleteAsync(id)).ReturnsAsync(true);  
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);  

            
            var result = await _service.DeleteCarModelAsync(id);

            
            Assert.True(result);  
            _mockUnitOfWork.Verify(u => u.CarModelRepository.DeleteAsync(id), Times.Once);  
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);  
        }


    }
}
