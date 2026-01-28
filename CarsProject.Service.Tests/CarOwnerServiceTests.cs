using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarOwnerServiceTests
    {
        private readonly Mock<IGenericRepository<CarOwner>> _mockRepo;
        private readonly CarOwnerService _service;

        public CarOwnerServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<CarOwner>>();
            _service = new CarOwnerService(_mockRepo.Object); 
        }

        [Fact]
        public async Task GetCarOwnersPagedAsync_ReturnsEmpty_WhenNoCarOwners()
        {
            var pfs = new PSFParameters
            {
                Paging = new PagingParameters { PageNumber = 1, PageSize = 5 }
            };

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PSFParameters>()))
                     .Returns(new List<CarOwner>().AsQueryable());

            var result = await _service.GetCarOwnersAsync(pfs);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCarOwnerByIdAsync_ReturnsCorrectCarOwner()
        {
            var id = 1;
            var carOwner = new CarOwner
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1990, 5, 10)
            };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carOwner);

            var result = await _service.GetCarOwnerByIdAsync(id);

            result.Should().BeEquivalentTo(carOwner);
        }

        [Fact]
        public async Task GetCarOwnersPagedAsync_ReturnsPagedFilteredSortedCarOwners()
        {
            var pfs = new PSFParameters
            {
                Paging = new PagingParameters { PageNumber = 1, PageSize = 2 },
                Sorting = new SortingParameters { OrderBy = "FirstName" },
                Filter = new FilterParameters { PropertyName = "FirstName", Filter = "f" }
            };

            var carOwners = new List<CarOwner>
            {
                new CarOwner { Id = 1, FirstName = "FirstName1", LastName = "LastName1", DateOfBirth = new DateOnly(2000, 2, 2) },
                new CarOwner { Id = 2, FirstName = "FirstName2", LastName = "LastName2", DateOfBirth = new DateOnly(1995, 6, 1) }
            };

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PSFParameters>())).Returns(carOwners.AsQueryable());

            var result = await _service.GetCarOwnersAsync(pfs);

            result.Should().HaveCount(2);
            result.Should().Contain(x => x.FirstName == "FirstName1");
            result.Should().Contain(x => x.FirstName == "FirstName2");
        }

        [Fact]
        public async Task AddCarOwnerAsync_ReturnsAddedCarOwner()
        {
            var carOwner = new CarOwner
            {
                FirstName = "UpdatedOwner",
                LastName = "UPD",
                DateOfBirth = new DateOnly(1990, 5, 10)
            };

            _mockRepo.Setup(r => r.GetQuery(It.IsAny<PSFParameters>())).Returns(new List<CarOwner>().AsQueryable());
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<CarOwner>())).ReturnsAsync((CarOwner c) =>
            {
                c.Id = 1; 
                return c;
            });

            var result = await _service.AddCarOwnerAsync(carOwner);

            result.Id.Should().Be(1);
            result.FirstName.Should().Be("UpdatedOwner");
            result.LastName.Should().Be("UPD");
        }

        [Fact]
        public async Task UpdateCarOwnerAsync_ReturnsUpdatedCarOwner()
        {
            var id = 1;
            var existingCarOwner = new CarOwner
            {
                Id = id,
                FirstName = "OldOwner",
                LastName = "OLD",
                DateOfBirth = new DateOnly(1985, 1, 1)
            };
            var updatedCarOwner = new CarOwner
            {
                Id = id,
                FirstName = "UpdatedOwner",
                LastName = "UPD",
                DateOfBirth = new DateOnly(1990, 5, 10)
            };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingCarOwner);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<CarOwner>())).ReturnsAsync(updatedCarOwner);

            var result = await _service.UpdateCarOwnerAsync(id, updatedCarOwner);

            result.Should().BeEquivalentTo(updatedCarOwner);
        }

        [Fact]
        public async Task DeleteCarOwnerAsync_ReturnsTrue_WhenCarOwnerIsDeleted()
        {
            var id = 1;
            var carOwner = new CarOwner { Id = id };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carOwner);
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteCarOwnerAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCarOwnerAsync_ReturnsFalse_WhenDeleteFails()
        {
            var id = 1;
            var carOwner = new CarOwner { Id = id };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carOwner);
            _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _service.DeleteCarOwnerAsync(id);

            result.Should().BeFalse();
        }
    }
}
