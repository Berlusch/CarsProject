using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarEngineTypeServiceTests
    {
        private readonly Mock<ICarEngineTypeRepository> _mockRepo;
        private readonly CarEngineTypeService _service;

        public CarEngineTypeServiceTests()
        {
            _mockRepo = new Mock<ICarEngineTypeRepository>();

            _service = new CarEngineTypeService(_mockRepo.Object); // SAMO JEDAN ARGUMENT
        }

        [Fact]
        public async Task GetCarEngineTypesAsync_ReturnsPagedFilteredSortedCarEngineTypes()
        {
            var pfs = new PFSParameters
            {
                Paging = new PagingParameters { PageNumber = 1, PageSize = 2 },
                Sorting = new SortingParameters { OrderBy = "Type" },
                Filter = new FilterParameters { PropertyName = "Type", Filter = "F" }
            };

            var carEngineTypes = new List<CarEngineType>
        {
            new CarEngineType { Id = 1, Type = "FirstType", Abrv = "FT" },
            new CarEngineType { Id = 2, Type = "SecondType", Abrv = "ST" }
        };

            _mockRepo.Setup(r => r.GetAllCarEngineTypesAsync(pfs))
                     .ReturnsAsync(carEngineTypes);

            var result = await _service.GetCarEngineTypesAsync(pfs);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Type == "FirstType");
            Assert.Contains(result, x => x.Type == "SecondType");
        }

        [Fact]
        public async Task GetCarEngineTypeByIdAsync_ReturnsCorrectCarEngineType()
        {
            var id = 1;
            var carEngineType = new CarEngineType { Id = id, Type = "Hybrid", Abrv = "HBR" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carEngineType);

            var result = await _service.GetCarEngineTypeByIdAsync(id);

            Assert.Equal(carEngineType.Id, result.Id);
            Assert.Equal(carEngineType.Type, result.Type);
            Assert.Equal(carEngineType.Abrv, result.Abrv);
        }
    }
}
