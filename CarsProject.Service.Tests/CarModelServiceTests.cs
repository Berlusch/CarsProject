using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.Service;
using MockQueryable;
using Moq;
using Xunit;

namespace CarsProject.Tests.Service
{
    public class CarModelServiceTests
    {
        private readonly Mock<IGenericRepository<CarModel>> _carModelRepoMock;
        private readonly CarModelService _sut;

        public CarModelServiceTests()
        {
            _carModelRepoMock = new Mock<IGenericRepository<CarModel>>();
            _sut = new CarModelService(_carModelRepoMock.Object);
        }

        // ─────────────────────────────────────────────────────────
        // Helper
        // ─────────────────────────────────────────────────────────

        private static IQueryable<CarModel> BuildQueryable(IEnumerable<CarModel> data)
        {
            var list = data.ToList();
            return list.BuildMock<CarModel>();
        }

        private static CarModel CreateCarModel(int id = 1) => new()
        {
            Id = id,
            Name = $"Model {id}",
            Abrv = $"M{id}",
            CarMakeId = 10,
            CarMake = new CarMake { Id = 10, Name = "Test Make" },
            CarEngineTypeId = 20,
            CarEngineType = new CarEngineType { Id = 20, Type = "Test Engine" }
        };

        // ─────────────────────────────────────────────────────────
        // GetCarModelsAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task GetCarModelsAsync_ReturnsPagedResult_WithCorrectTotalCount()
        {
            // Arrange
            var models = new List<CarModel>
            {
                CreateCarModel(1),
                CreateCarModel(2),
                CreateCarModel(3)
            };

            var pfs = new PFSParameters { Paging = new PagingParameters { PageNumber = 1, PageSize = 2 } };

            _carModelRepoMock
                .Setup(r => r.GetQuery(pfs))
                .Returns(BuildQueryable(models));

            // Act
            var result = await _sut.GetCarModelsAsync(pfs);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalCount);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task GetCarModelsAsync_NoPaging_ReturnsAllItems()
        {
            // Arrange
            var models = Enumerable.Range(1, 5).Select(i => CreateCarModel(i)).ToList();
            var pfs = new PFSParameters { Paging = new PagingParameters { PageSize = 0 } };

            _carModelRepoMock
                .Setup(r => r.GetQuery(pfs))
                .Returns(BuildQueryable(models));

            // Act
            var result = await _sut.GetCarModelsAsync(pfs);

            // Assert
            Assert.Equal(5, result.Items.Count);
            Assert.Equal(5, result.TotalCount);
        }

        [Fact]
        public async Task GetCarModelsAsync_EmptyRepository_ReturnsEmptyPagedResult()
        {
            // Arrange
            var pfs = new PFSParameters { Paging = new PagingParameters { PageNumber = 1, PageSize = 10 } };

            _carModelRepoMock
                .Setup(r => r.GetQuery(pfs))
                .Returns(BuildQueryable(new List<CarModel>()));

            // Act
            var result = await _sut.GetCarModelsAsync(pfs);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
        }

        // ─────────────────────────────────────────────────────────
        // GetCarModelByIdAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task GetCarModelByIdAsync_ExistingId_ReturnsCarModel()
        {
            // Arrange
            var model = CreateCarModel(1);

            _carModelRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarModel> { model }));

            // Act
            var result = await _sut.GetCarModelByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(model.Name, result.Name);
        }

        [Fact]
        public async Task GetCarModelByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _carModelRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarModel>()));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _sut.GetCarModelByIdAsync(999));
        }

        // ─────────────────────────────────────────────────────────
        // AddCarModelAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task AddCarModelAsync_NewModel_ReturnsAddedWithFKs()
        {
            // Arrange
            var newModel = new CarModel
            {
                Name = "New Model",
                Abrv = "NM",
                CarMakeId = 10,
                CarMake = new CarMake { Id = 10, Name = "Test Make" },
                CarEngineTypeId = 20,
                CarEngineType = new CarEngineType { Id = 20, Type = "Test Engine" }
            };

            var savedModel = CreateCarModel(5);
            savedModel.Name = "New Model";

            _carModelRepoMock
                .SetupSequence(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarModel>()))           // duplicate check
                .Returns(BuildQueryable(new List<CarModel> { savedModel })); // re-fetch after insert

            _carModelRepoMock
                .Setup(r => r.AddAsync(newModel))
                .ReturnsAsync(savedModel);

            // Act
            var result = await _sut.AddCarModelAsync(newModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Model", result.Name);
            Assert.NotNull(result.CarMake);
            Assert.NotNull(result.CarEngineType);

            _carModelRepoMock.Verify(r => r.AddAsync(newModel), Times.Once);
        }

        [Fact]
        public async Task AddCarModelAsync_DuplicateName_ThrowsException()
        {
            // Arrange
            var existing = CreateCarModel(1);
            var duplicate = new CarModel
            {
                Name = existing.Name,
                CarMake = new CarMake { Id = 10, Name = "Test Make" },
                CarEngineType = new CarEngineType { Id = 20, Type = "Test Engine" }
            };

            _carModelRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarModel> { existing }));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _sut.AddCarModelAsync(duplicate));

            _carModelRepoMock.Verify(r => r.AddAsync(It.IsAny<CarModel>()), Times.Never);
        }

        [Fact]
        public async Task AddCarModelAsync_NullArgument_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _sut.AddCarModelAsync(null!));
        }

        // ─────────────────────────────────────────────────────────
        // UpdateCarModelAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task UpdateCarModelAsync_ValidData_ReturnsUpdatedModel()
        {
            // Arrange
            var existing = CreateCarModel(1);
            var updateData = new CarModel
            {
                Name = "X",
                Abrv = "X",
                CarMakeId = 1,
                CarEngineTypeId = 1,
                CarMake = new CarMake { Id = 1, Name = "Test Make" },
                CarEngineType = new CarEngineType { Id = 1, Type = "Test Engine" }
            };
            var updatedResult = CreateCarModel(1);
            updatedResult.Name = "Updated Model";

            _carModelRepoMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existing);

            _carModelRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<CarModel>()))
                .Returns(Task.FromResult(existing));

            _carModelRepoMock
                .Setup(r => r.GetQuery(It.IsAny<PFSParameters>()))
                .Returns(BuildQueryable(new List<CarModel> { updatedResult }));

            // Act
            var result = await _sut.UpdateCarModelAsync(1, updateData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Model", result.Name);

            _carModelRepoMock.Verify(r => r.UpdateAsync(It.IsAny<CarModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCarModelAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            _carModelRepoMock
                .Setup(r => r.GetByIdAsync(999))
                .Returns(Task.FromResult<CarModel>(null!));

            var updateData = new CarModel
            {
                Name = "X",
                Abrv = "X",
                CarMakeId = 1,
                CarEngineTypeId = 1,
                CarMake = new CarMake { Id = 1, Name = "Make" },
                CarEngineType = new CarEngineType { Id = 1, Type = "Engine" }
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _sut.UpdateCarModelAsync(999, updateData));
        }

        [Fact]
        public async Task UpdateCarModelAsync_NullArgument_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _sut.UpdateCarModelAsync(1, null!));
        }

        // ─────────────────────────────────────────────────────────
        // DeleteCarModelAsync
        // ─────────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteCarModelAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            _carModelRepoMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteCarModelAsync(1);

            // Assert
            Assert.True(result);
            _carModelRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCarModelAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            _carModelRepoMock
                .Setup(r => r.DeleteAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteCarModelAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}