using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests

{
    public class CarMakeServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICarMakeRepository> _mockCarMakeRepository;
        private readonly CarMakeService _service;

        public CarMakeServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCarMakeRepository = new Mock<ICarMakeRepository>();

            // Mockanje povratne vrijednosti iz IUnitOfWork
            _mockUnitOfWork.Setup(uow => uow.CarMakeRepository).Returns(_mockCarMakeRepository.Object);

            // Kreiranje instance servisa
            _service = new CarMakeService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCarMakesPagedAsync_ShouldReturnPagedCarMakes_WhenValidParams()
        {
            // Arrange
            var carMakes = new List<CarMake>
        {
            new CarMake { Id = 1, Name = "Ford" },
            new CarMake { Id = 2, Name = "Toyota" }
        };

            // DTO verzija
            var carMakesDto = new List<CarMakeDTORead>
        {
            new CarMakeDTORead(1, "Ford", "FOR"),
            new CarMakeDTORead(2, "Toyota", "TOY")
        };

            // Mockiranje repozitorija za vraćanje popisa automobila
            _mockCarMakeRepository.Setup(repo => repo.GetAllCarMakesAsync()).ReturnsAsync(carMakes);

            // Mockiranje AutoMapper-a za mapiranje CarMake u CarMakeDTORead
            _mockMapper.Setup(m => m.Map<IEnumerable<CarMakeDTORead>>(It.IsAny<IEnumerable<CarMake>>()))
                       .Returns((IEnumerable<CarMake> carMakes) => carMakes.Select(carMake => new CarMakeDTORead(carMake.Id, carMake.Name, carMake.Name.Substring(0, 3).ToUpper())));

            // Act
            var result = await _service.GetCarMakesPagedAsync(1, 2, "name", "");

            // Assert
            result.Should().BeEquivalentTo(carMakesDto);
        }

        [Fact]
        public async Task GetCarMakesPagedAsync_ShouldReturnEmpty_WhenNoCarMakes()
        {
            // Arrange
            var carMakes = new List<CarMake>();
            var carMakesDto = new List<CarMakeDTORead>();

            // Mockiranje repozitorija za vraćanje praznog popisa automobila
            _mockCarMakeRepository.Setup(repo => repo.GetAllCarMakesAsync()).ReturnsAsync(carMakes);

            // Mockiranje AutoMapper-a
            _mockMapper.Setup(m => m.Map<IEnumerable<CarMakeDTORead>>(It.IsAny<IEnumerable<CarMake>>())).Returns(carMakesDto);

            // Act
            var result = await _service.GetCarMakesPagedAsync(1, 2, "name", "");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCarMakeByIdAsync_ShouldReturnCarMake_WhenExists()
        {
            var id = 1;
            var carMake = new CarMake { Id = id, Name = "Ford", Abrv = "FRD" };
            var carMakeDto = new CarMakeDTORead(id, "Ford", "FRD");

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(carMake);
            _mockMapper.Setup(m => m.Map<CarMakeDTORead>(carMake)).Returns(carMakeDto);

            var result = await _service.GetCarMakeByIdAsync(id);

            result.Should().BeEquivalentTo(carMakeDto);
        }

        
        [Fact]
        public async Task AddCarMakeAsync_ShouldAddCarMake_WhenValid()
        {
            var dto = new CarMakeDTOInsertUpdate("Toyota", "TOY");
            var carMakes = new List<CarMake>(); 
            var carMakeEntity = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" };
            var carMakeDtoRead = new CarMakeDTORead(1, "Toyota", "TOY");

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetAllCarMakesAsync()).ReturnsAsync(carMakes);
            _mockMapper.Setup(m => m.Map<CarMake>(dto)).Returns(carMakeEntity);
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.AddAsync(carMakeEntity)).ReturnsAsync(carMakeEntity);
            _mockMapper.Setup(m => m.Map<CarMakeDTORead>(carMakeEntity)).Returns(carMakeDtoRead);

            var result = await _service.AddCarMakeAsync(dto);

            result.Should().BeEquivalentTo(carMakeDtoRead);
        }

        [Fact]
        public async Task AddCarMakeAsync_ShouldThrowException_WhenNameExists()
        {
            var dto = new CarMakeDTOInsertUpdate("Toyota", "TOY");
            var existing = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" };

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetAllCarMakesAsync()).ReturnsAsync(new List<CarMake> { existing });

            Func<Task> act = async () => await _service.AddCarMakeAsync(dto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("CarMake with the name Toyota already exists.");
        }

        [Fact]
        public async Task UpdateCarMakeAsync_ShouldUpdateCarMake_WhenValid()
        {
            var id = 1;
            var dto = new CarMakeDTOInsertUpdate("Honda", "HND");
            var existing = new CarMake { Id = id, Name = "Old", Abrv = "OLD" };
            var updated = new CarMake { Id = id, Name = "Honda", Abrv = "HND" };
            var updatedDto = new CarMakeDTORead(id, "Honda", "HND");

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(existing);
            _mockMapper.Setup(m => m.Map(dto, existing));
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.UpdateAsync(existing)).ReturnsAsync(updated);
            _mockMapper.Setup(m => m.Map<CarMakeDTORead>(updated)).Returns(updatedDto);

            var result = await _service.UpdateCarMakeAsync(id, dto);

            result.Should().BeEquivalentTo(updatedDto);
        }
        

        [Fact]
        public async Task DeleteCarMakeAsync_ShouldReturnTrue_WhenDeleted()
        {
            var id = 1;
            var carMake = new CarMake { Id = id };

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(carMake);
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteCarMakeAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCarMakeAsync_ShouldReturnFalse_WhenCarMakeNotFound()
        {
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new CarMake { Id = 1, Name = "Ford" });

            var result = await _service.DeleteCarMakeAsync(1);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteCarMakeAsync_ShouldReturnFalse_WhenDeleteFails()
        {
            var id = 1;
            var carMake = new CarMake { Id = id };

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(carMake);
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _service.DeleteCarMakeAsync(id);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetCarMakeByIdAsync_ReturnsCorrectCarMakeDTO()
        {
            // Arrange
            var id = 1;
            var carMake = new CarMake { Id = id, Name = "TestMake" };
            var expectedDto = new CarMakeDTORead (1,"Ford","FOR" );

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(carMake);
            _mockMapper.Setup(m => m.Map<CarMakeDTORead>(carMake)).Returns(expectedDto);

            // Act
            var result = await _service.GetCarMakeByIdAsync(id);

            // Assert
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
            Assert.Equal(expectedDto.Abrv, result.Abrv);
        }

        [Fact]
        public async Task GetCarMakesPagedAsync_ReturnsPagedFilteredSortedCarMakes()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 2;
            var sortBy = "name";
            var filter = "f";

            var carMakes = new List<CarMake>
    {
        new CarMake { Id = 1, Name = "Ford" },
        new CarMake { Id = 2, Name = "Fiat" },
        new CarMake { Id = 3, Name = "BMW" } // ne bi trebao biti u rezultatu zbog filtera
    };

            var expectedDTOs = new List<CarMakeDTORead>
    {
        new CarMakeDTORead (2, "Fiat",  "FIA" ),
        new CarMakeDTORead ( 1,"Ford", "FOR" )
    };

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetAllCarMakesAsync()).ReturnsAsync(carMakes);
            _mockMapper.Setup(m => m.Map<IEnumerable<CarMakeDTORead>>(It.IsAny<IEnumerable<CarMake>>()))
                       .Returns(expectedDTOs);

            // Act
            var result = await _service.GetCarMakesPagedAsync(pageNumber, pageSize, sortBy, filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Name == "Ford");
            Assert.Contains(result, x => x.Name == "Fiat");
        }

        [Fact]
        public async Task AddCarMakeAsync_ReturnsAddedCarMakeDTO()
        {
            // Arrange
            var carMakeDto = new CarMakeDTOInsertUpdate ("Tesla","TES" );
            var carMake = new CarMake { Id = 4, Name = "Tesla", Abrv = "TES" };

            var expectedDto = new CarMakeDTORead (4, "Tesla", "TES" );

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetAllCarMakesAsync()).ReturnsAsync(new List<CarMake>());
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.AddAsync(It.IsAny<CarMake>())).ReturnsAsync(carMake);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CarMakeDTORead>(carMake)).Returns(expectedDto);

            // Act
            var result = await _service.AddCarMakeAsync(carMakeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
            Assert.Equal(expectedDto.Abrv, result.Abrv);
        }

        [Fact]
        public async Task UpdateCarMakeAsync_ReturnsUpdatedCarMakeDTO()
        {
            // Arrange
            var id = 1;
            var carMakeDto = new CarMakeDTOInsertUpdate ( "UpdatedMake", "UPD" );
            var existingCarMake = new CarMake { Id = id, Name = "OldMake", Abrv = "OLD" };
            var updatedCarMake = new CarMake { Id = id, Name = "UpdatedMake", Abrv = "UPD" };

            var expectedDto = new CarMakeDTORead (id, "UpdatedMake", "UPD" );

            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(existingCarMake);
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.UpdateAsync(It.IsAny<CarMake>())).ReturnsAsync(updatedCarMake);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CarMakeDTORead>(updatedCarMake)).Returns(expectedDto);

            // Act
            var result = await _service.UpdateCarMakeAsync(id, carMakeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
            Assert.Equal(expectedDto.Abrv, result.Abrv);
        }

        [Fact]
        public async Task DeleteCarMakeAsync_ReturnsTrue_WhenCarMakeIsDeleted()
        {
            // Arrange
            var id = 1;
            var carMake = new CarMake { Id = id, Name = "Fiat", Abrv = "FIA" };

            // Postavljanje mocka za GetByIdAsync
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.GetByIdAsync(id)).ReturnsAsync(carMake);
            _mockUnitOfWork.Setup(u => u.CarMakeRepository.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteCarMakeAsync(id);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(u => u.CarMakeRepository.DeleteAsync(id), Times.Once); 
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once); 
        }


    }
}
