using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;
using Moq;

namespace CarsProject.Service.Tests

{
    public class CarEngineTypeServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICarEngineTypeRepository> _mockCarEngineTypeRepository;
        private readonly CarEngineTypeService _service;

        public CarEngineTypeServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCarEngineTypeRepository = new Mock<ICarEngineTypeRepository>();
            
            _mockUnitOfWork.Setup(uow => uow.CarEngineTypeRepository).Returns(_mockCarEngineTypeRepository.Object);
            
            _service = new CarEngineTypeService(_mockUnitOfWork.Object, _mockMapper.Object);
        }
        [Fact]
        public async Task GetCarEngineTypesPagedAsync_ReturnsPagedFilteredSortedCarEngineTypes()
        {            
            var pageNumber = 1;
            var pageSize = 2;
            var sortBy = "name";
            var filter = "f";

            var carEngineTypes = new List<CarEngineType>
    {
        new CarEngineType { Id = 1, Type = "FirstType", Abrv = "FT" },
        new CarEngineType { Id = 2, Type = "SecondType", Abrv = "ST" }
    };

            var expectedDTOs = new List<CarEngineTypeDTORead>
    {
        new CarEngineTypeDTORead(1, "FirstType", "FT"),
        new CarEngineTypeDTORead(2, "SecondType", "ST")
    };

            _mockUnitOfWork.Setup(u => u.CarEngineTypeRepository.GetAllCarEngineTypesAsync()).ReturnsAsync(carEngineTypes);
            _mockMapper.Setup(m => m.Map<IEnumerable<CarEngineTypeDTORead>>(It.IsAny<IEnumerable<CarEngineType>>()))
                       .Returns(expectedDTOs);

            
            var result = await _service.GetCarEngineTypesPagedAsync(pageNumber, pageSize, sortBy, filter);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Type == "FirstType");
            Assert.Contains(result, x => x.Type == "SecondType");
        }

        [Fact]
        public async Task GetCarEngineTypeByIdAsync_ReturnsCorrectCarEngineTypeDTO()     
        {
            var id = 1;
            var carEngineType = new CarEngineType { Id = id, Type = "Hybrid", Abrv = "HBR"};
            var expectedDto = new CarEngineTypeDTORead(id, "Hybrid", "HBR");

            _mockUnitOfWork.Setup(u => u.CarEngineTypeRepository.GetByIdAsync(id)).ReturnsAsync(carEngineType);
            _mockMapper.Setup(m => m.Map<CarEngineTypeDTORead>(carEngineType)).Returns(expectedDto);

            
            var result = await _service.GetCarEngineTypeByIdAsync(id);
            
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Type, result.Type);
            Assert.Equal(expectedDto.Abrv, result.Abrv);
            
        }
    }
}
