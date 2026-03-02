using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarMakeServiceTests
    {
        private readonly Mock<IGenericRepository<CarMake>> _mockRepo;
        private readonly CarMakeService _service;

        public CarMakeServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<CarMake>>();
            _service = new CarMakeService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCarMakesAsync_ShouldReturnPagedCarMakes_WhenValidParams()
        {
            var pfs = new PFSParameters
            {
                Paging = new PagingParameters { PageNumber = 1, PageSize = 2 },
                Sorting = new SortingParameters { OrderBy = "name" },
                Filter = new FilterParameters { PropertyName = "", Filter = "" }
            };

            var carMakes = new List<CarMake>
            {
                new CarMake { Id = 1, Name = "Ford", Abrv = "FRD" },
                new CarMake { Id = 2, Name = "Toyota", Abrv = "TOY" }
            };

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PFSParameters>())).Returns(carMakes.AsQueryable());

            var result = await _service.GetCarMakesAsync(pfs);

            result.Items.Should().HaveCount(2);
            result.Items.Select(c => c.Name).Should().Contain(new[] { "Ford", "Toyota" });
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task GetCarMakeByIdAsync_ShouldReturnCarMake_WhenExists()
        {
            var id = 1;
            var carMake = new CarMake { Id = id, Name = "Ford", Abrv = "FRD" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carMake);

            var result = await _service.GetCarMakeByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Name.Should().Be("Ford");
        }

        [Fact]
        public async Task AddCarMakeAsync_ShouldAddCarMake_WhenValid()
        {
            var carMake = new CarMake { Name = "Honda", Abrv = "HND" };

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PFSParameters>())).Returns(new List<CarMake>().AsQueryable());
            _mockRepo.Setup(r => r.AddAsync(carMake)).ReturnsAsync(() =>
            {
                carMake.Id = 1;
                return carMake;
            });

            var result = await _service.AddCarMakeAsync(carMake);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Honda");
        }

        [Fact]
        public async Task AddCarMakeAsync_ShouldThrowException_WhenNameExists()
        {
            var existing = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" };
            var newCarMake = new CarMake { Name = "Toyota", Abrv = "TOY" };

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PFSParameters>())).Returns(new List<CarMake> { existing }.AsQueryable());

            Func<Task> act = async () => await _service.AddCarMakeAsync(newCarMake);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("CarMake with the name Toyota already exists.");
        }

        [Fact]
        public async Task UpdateCarMakeAsync_ShouldUpdateCarMake_WhenValid()
        {
            var id = 1;
            var existing = new CarMake { Id = id, Name = "OldName", Abrv = "OLD" };
            var updated = new CarMake { Id = id, Name = "NewName", Abrv = "NEW" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);

            var result = await _service.UpdateCarMakeAsync(id, updated);

            result.Name.Should().Be("NewName");
            result.Abrv.Should().Be("NEW");
        }

        [Fact]
        public async Task DeleteCarMakeAsync_ShouldReturnTrue_WhenDeleted()
        {
            var id = 1;
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteCarMakeAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCarMakeAsync_ShouldReturnFalse_WhenNotDeleted()
        {
            var id = 1;
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _service.DeleteCarMakeAsync(id);

            result.Should().BeFalse();
        }
    }
}