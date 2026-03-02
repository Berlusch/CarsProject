using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarEngineTypeServiceTests
    {
        private readonly Mock<ICarEngineTypeRepository> _mockRepo;
        private readonly CarEngineTypeService _service;
       
        private static readonly CarEngineType[] TestEngineTypes = new[]
        {
            new CarEngineType { Id = 1, Type = "FirstType", Abrv = "FT" },
            new CarEngineType { Id = 2, Type = "SecondType", Abrv = "ST" }
        };

        private static readonly string[] TestEngineTypeNames = new[] { "FirstType", "SecondType" };

        public CarEngineTypeServiceTests()
        {
            _mockRepo = new Mock<ICarEngineTypeRepository>();
            _service = new CarEngineTypeService(_mockRepo.Object);
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

            _mockRepo.Setup(r => r.GetQuery(pfs)).Returns(TestEngineTypes.AsQueryable());

            var result = await _service.GetCarEngineTypesAsync(pfs);

            result.Items.Should().HaveCount(2);
            result.Items.Select(x => x.Type).Should().Contain(TestEngineTypeNames);
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetCarEngineTypeByIdAsync_ReturnsCorrectCarEngineType()
        {
            var id = 1;
            var carEngineType = new CarEngineType { Id = id, Type = "Hybrid", Abrv = "HBR" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carEngineType);

            var result = await _service.GetCarEngineTypeByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(carEngineType.Id);
            result.Type.Should().Be(carEngineType.Type);
            result.Abrv.Should().Be(carEngineType.Abrv);
        }
    }
}