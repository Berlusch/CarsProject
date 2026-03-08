using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service;
using Moq;
using MockQueryable;

namespace CarsProject.Tests.Service
{
    public class CarRegistrationServiceTests
    {
        private readonly Mock<IGenericRepository<CarRegistration>> _carRegistrationRepoMock;
        private readonly Mock<IGenericRepository<CarOwner>> _carOwnerRepoMock;
        private readonly Mock<IGenericRepository<CarModel>> _carModelRepoMock;
        private readonly CarRegistrationService _sut;

        public CarRegistrationServiceTests()
        {
            _carRegistrationRepoMock = new Mock<IGenericRepository<CarRegistration>>();
            _carOwnerRepoMock = new Mock<IGenericRepository<CarOwner>>();
            _carModelRepoMock = new Mock<IGenericRepository<CarModel>>();

            _sut = new CarRegistrationService(
                _carRegistrationRepoMock.Object,
                _carOwnerRepoMock.Object,
                _carModelRepoMock.Object
            );
        }

        // ─────────────────────────────────────────────────────────
        // Helper – build an IQueryable<CarRegistration> mock
        // ─────────────────────────────────────────────────────────
        private static IQueryable<CarRegistration> BuildQueryable(IEnumerable<CarRegistration> data)
        {
            var list = data.ToList();
            return list.BuildMock<CarRegistration>();
        }

        private static CarRegistration CreateCarRegistration(int id = 1) => new()
        {
            Id = id,
            RegistrationNumber = $"ZG-{id:D4}-AA",
            CarOwnerId = 10,
            CarOwner = new CarOwner { Id = 10, FirstName = "Test", LastName = "Owner" },
            CarModelId = 20,
            CarModel = new CarModel { Id = 20, Name = "Test Model", CarMake = new CarMake(), CarEngineType = new CarEngineType() }
        };

        // ─────────────────────────────────────────────────────────
        // GetCarRegistrationsAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task GetCarRegistrationsAsync_ReturnsPagedResult_WithCorrectTotalCount()
        {
            // Arrange
            var registrations = new List<CarRegistration>
            {
                CreateCarRegistration(1),
                CreateCarRegistration(2),
                CreateCarRegistration(3)
            };

            var pfs = new PFSParameters { Paging = new PagingParameters { PageNumber = 1, PageSize = 2 } };

            var queryable = BuildQueryable(registrations);

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(pfs))
                .Returns(queryable);

            // Act
            var result = await _sut.GetCarRegistrationsAsync(pfs);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task GetCarRegistrationsAsync_NoPaging_ReturnsAllItems()
        {
            // Arrange
            var registrations = Enumerable.Range(1, 5)
                .Select(i => CreateCarRegistration(i))
                .ToList();

            var pfs = new PFSParameters { Paging = new PagingParameters { PageSize = 0 } };

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(pfs))
                .Returns(BuildQueryable(registrations));

            // Act
            var result = await _sut.GetCarRegistrationsAsync(pfs);

            // Assert
            Assert.Equal(5, result.Items.Count);
            Assert.Equal(5, result.TotalCount);
        }

        [Fact]
        public async Task GetCarRegistrationsAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var pfs = new PFSParameters { Paging = new PagingParameters { PageNumber = 1, PageSize = 10 } };

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(pfs))
                .Returns(BuildQueryable(new List<CarRegistration>()));

            // Act
            var result = await _sut.GetCarRegistrationsAsync(pfs);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }

        // ─────────────────────────────────────────────────────────
        // GetCarRegistrationByIdAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task GetCarRegistrationByIdAsync_ExistingId_ReturnsCarRegistration()
        {
            // Arrange
            var registration = CreateCarRegistration(1);

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarRegistration> { registration }));

            // Act
            var result = await _sut.GetCarRegistrationByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(registration.RegistrationNumber, result.RegistrationNumber);
        }

        [Fact]
        public async Task GetCarRegistrationByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarRegistration>()));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _sut.GetCarRegistrationByIdAsync(999));
        }

        // ─────────────────────────────────────────────────────────
        // AddCarRegistrationAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task AddCarRegistrationAsync_NewRegistration_ReturnsAddedWithFKs()
        {
            // Arrange
            var newReg = new CarRegistration
            {
                RegistrationNumber = "ZG-9999-AB",
                CarOwnerId = 10,
                CarModelId = 20
            };

            var savedReg = CreateCarRegistration(5);
            savedReg.RegistrationNumber = "ZG-9999-AB";

            // No existing registration with that number
            _carRegistrationRepoMock
                .SetupSequence(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarRegistration>()))   // duplicate check
                .Returns(BuildQueryable(new List<CarRegistration> { savedReg })); // re-fetch after insert

            _carRegistrationRepoMock
                .Setup(r => r.AddAsync(newReg))
                .ReturnsAsync(savedReg);

            // Act
            var result = await _sut.AddCarRegistrationAsync(newReg);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ZG-9999-AB", result.RegistrationNumber);
            Assert.NotNull(result.CarOwner);
            Assert.NotNull(result.CarModel);

            _carRegistrationRepoMock.Verify(r => r.AddAsync(newReg), Times.Once);
        }

        [Fact]
        public async Task AddCarRegistrationAsync_DuplicateRegistrationNumber_ThrowsException()
        {
            // Arrange
            var existing = CreateCarRegistration(1);
            var duplicate = new CarRegistration { RegistrationNumber = existing.RegistrationNumber };

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarRegistration> { existing }));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _sut.AddCarRegistrationAsync(duplicate));

            _carRegistrationRepoMock.Verify(r => r.AddAsync(It.IsAny<CarRegistration>()), Times.Never);
        }

        [Fact]
        public async Task AddCarRegistrationAsync_DuplicateRegistrationNumber_CaseInsensitive_ThrowsException()
        {
            // Arrange
            var existing = CreateCarRegistration(1);
            existing.RegistrationNumber = "zg-0001-aa";

            var duplicate = new CarRegistration { RegistrationNumber = "ZG-0001-AA" };

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarRegistration> { existing }));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _sut.AddCarRegistrationAsync(duplicate));
        }

        [Fact]
        public async Task AddCarRegistrationAsync_NullArgument_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _sut.AddCarRegistrationAsync(null!));
        }

        // ─────────────────────────────────────────────────────────
        // UpdateCarRegistrationAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task UpdateCarRegistrationAsync_ValidData_ReturnsUpdatedRegistration()
        {
            // Arrange
            var existing = CreateCarRegistration(1);
            var updatedData = new CarRegistration
            {
                RegistrationNumber = "ZG-NEW-AB",
                CarOwnerId = 10,
                CarModelId = 20
            };
            var updatedResult = CreateCarRegistration(1);
            updatedResult.RegistrationNumber = "ZG-NEW-AB";

            _carRegistrationRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            _carOwnerRepoMock
                .Setup(r => r.GetByIdAsync(updatedData.CarOwnerId))
                .ReturnsAsync(existing.CarOwner);

            _carModelRepoMock
                .Setup(r => r.GetByIdAsync(updatedData.CarModelId))
                .ReturnsAsync(existing.CarModel);

            _carRegistrationRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<CarRegistration>()))
                .Returns(Task.FromResult(existing));

            _carRegistrationRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarRegistration> { updatedResult }));

            // Act
            var result = await _sut.UpdateCarRegistrationAsync(1, updatedData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ZG-NEW-AB", result.RegistrationNumber);

            _carRegistrationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<CarRegistration>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCarRegistrationAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _carRegistrationRepoMock
    .Setup(r => r.GetByIdAsync(999))
    .Returns(Task.FromResult<CarRegistration>(null!));

            var updateData = new CarRegistration { RegistrationNumber = "ZG-0000-XX", CarOwnerId = 1, CarModelId = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _sut.UpdateCarRegistrationAsync(999, updateData));
        }

        [Fact]
        public async Task UpdateCarRegistrationAsync_NonExistingCarOwner_ThrowsException()
        {
            // Arrange
            var existing = CreateCarRegistration(1);
            var updateData = new CarRegistration { RegistrationNumber = "ZG-0000-XX", CarOwnerId = 999, CarModelId = 20 };

            _carRegistrationRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            _carOwnerRepoMock
                .Setup(r => r.GetByIdAsync(999))
                .Returns(Task.FromResult<CarOwner>(null!));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _sut.UpdateCarRegistrationAsync(1, updateData));
        }

        [Fact]
        public async Task UpdateCarRegistrationAsync_NonExistingCarModel_ThrowsException()
        {
            // Arrange
            var existing = CreateCarRegistration(1);
            var updateData = new CarRegistration { RegistrationNumber = "ZG-0000-XX", CarOwnerId = 10, CarModelId = 999 };

            _carRegistrationRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            _carOwnerRepoMock
                .Setup(r => r.GetByIdAsync(10))
                .ReturnsAsync(existing.CarOwner);

            _carModelRepoMock
                .Setup(r => r.GetByIdAsync(999))
                .Returns(Task.FromResult<CarModel>(null!));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _sut.UpdateCarRegistrationAsync(1, updateData));
        }

        [Fact]
        public async Task UpdateCarRegistrationAsync_NullArgument_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _sut.UpdateCarRegistrationAsync(1, null!));
        }

        // ─────────────────────────────────────────────────────────
        // DeleteCarRegistrationAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteCarRegistrationAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _carRegistrationRepoMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteCarRegistrationAsync(1);

            // Assert
            Assert.True(result);
            _carRegistrationRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCarRegistrationAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _carRegistrationRepoMock
                .Setup(r => r.DeleteAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteCarRegistrationAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}