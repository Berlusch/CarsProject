using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests

{
    public class CarOwnerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICarOwnerRepository> _mockCarOwnerRepository;
        private readonly CarOwnerService _service;

        public CarOwnerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCarOwnerRepository = new Mock<ICarOwnerRepository>();
            
            _mockUnitOfWork.Setup(uow => uow.CarOwnerRepository).Returns(_mockCarOwnerRepository.Object);
            
            _service = new CarOwnerService(_mockUnitOfWork.Object, _mockMapper.Object);
        }                   

        [Fact]
        public async Task GetCarOwnersPagedAsync_ShouldReturnEmpty_WhenNoCarOwners()
        {           
            var carOwners = new List<CarOwner>();
            var carOwnersDto = new List<CarOwnerDTORead>();
            
            _mockCarOwnerRepository.Setup(repo => repo.GetAllCarOwnersAsync()).ReturnsAsync(carOwners);
            
            _mockMapper.Setup(m => m.Map<IEnumerable<CarOwnerDTORead>>(It.IsAny<IEnumerable<CarOwner>>())).Returns(carOwnersDto);
            
            var result = await _service.GetCarOwnersPagedAsync(1, 2, "name", "");
            
            result.Should().BeEmpty();
        }                      
                
        [Fact]
        public async Task DeleteCarOwnerAsync_ShouldReturnTrue_WhenDeleted()
        {
            var id = 1;
            var carOwner = new CarOwner { Id = id };

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetByIdAsync(id)).ReturnsAsync(carOwner);
            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteCarOwnerAsync(id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCarOwnerAsync_ShouldReturnFalse_WhenDeleteFails()
        {
            var id = 1;
            var carOwner = new CarOwner { Id = id };

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetByIdAsync(id)).ReturnsAsync(carOwner);
            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.DeleteAsync(id)).ReturnsAsync(false);

            var result = await _service.DeleteCarOwnerAsync(id);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetCarOwnerByIdAsync_ReturnsCorrectCarOwnerDTO()
        {            
            var id = 1;
            var carOwner = new CarOwner { Id = id, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 5, 10) };
            var expectedDto = new CarOwnerDTORead(id, "John", "Doe", new DateOnly(1990, 5, 10));

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetByIdAsync(id)).ReturnsAsync(carOwner);
            _mockMapper.Setup(m => m.Map<CarOwnerDTORead>(carOwner)).Returns(expectedDto);
            
            var result = await _service.GetCarOwnerByIdAsync(id);
            
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.FirstName, result.FirstName);
            Assert.Equal(expectedDto.LastName, result.LastName);
            Assert.Equal(expectedDto.DateOfBirth, result.DateOfBirth);
        }


        [Fact]
        public async Task GetCarOwnersPagedAsync_ReturnsPagedFilteredSortedCarOwners()
        {            
            var pageNumber = 1;
            var pageSize = 2;
            var sortBy = "name";
            var filter = "f";

            var carOwners = new List<CarOwner>
    {
        new CarOwner { Id = 1, FirstName = "FirstName1", LastName = "LastName1", DateOfBirth = new DateOnly(2000, 2, 2) },
        new CarOwner { Id = 2, FirstName = "FirstName2", LastName = "LastName2", DateOfBirth = new DateOnly(1995, 6, 1) }
    };

            var expectedDTOs = new List<CarOwnerDTORead>
    {
        new CarOwnerDTORead(1, "FirstName1", "LastName1", new DateOnly(2000, 2, 2)),
        new CarOwnerDTORead(2, "FirstName2", "LastName2", new DateOnly(1995, 6, 1))
    };

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetAllCarOwnersAsync()).ReturnsAsync(carOwners);
            _mockMapper.Setup(m => m.Map<IEnumerable<CarOwnerDTORead>>(It.IsAny<IEnumerable<CarOwner>>()))
                       .Returns(expectedDTOs);
                        
            var result = await _service.GetCarOwnersPagedAsync(pageNumber, pageSize, sortBy, filter);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.FirstName == "FirstName1");
            Assert.Contains(result, x => x.FirstName == "FirstName2");
        }


        [Fact]
        public async Task AddCarOwnerAsync_ReturnsAddedCarOwnerDTO()
        {            
            var carOwnerDto = new CarOwnerDTOInsertUpdate("UpdatedOwner", "UPD", new DateOnly(1990, 5, 10));
            var carOwner = new CarOwner { Id = 1, FirstName = "UpdatedOwner", LastName = "UPD", DateOfBirth = new DateOnly(1990, 5, 10) };
            var expectedDto = new CarOwnerDTORead(1, "UpdatedOwner", "UPD", new DateOnly(1990, 5, 10));

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.AddAsync(It.IsAny<CarOwner>())).ReturnsAsync(carOwner);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CarOwnerDTORead>(carOwner)).Returns(expectedDto);
                        
            var result = await _service.AddCarOwnerAsync(carOwnerDto);
                        
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.FirstName, result.FirstName);
            Assert.Equal(expectedDto.LastName, result.LastName);
            Assert.Equal(expectedDto.DateOfBirth, result.DateOfBirth);
        }


        [Fact]
        public async Task UpdateCarOwnerAsync_ReturnsUpdatedCarOwnerDTO()
        {            
            var id = 1;
            var carOwnerDto = new CarOwnerDTOInsertUpdate("UpdatedOwner", "UPD", new DateOnly(1990, 5, 10));
            var existingCarOwner = new CarOwner { Id = id, FirstName = "OldOwner", LastName = "OLD", DateOfBirth = new DateOnly(1985, 1, 1) };
            var updatedCarOwner = new CarOwner { Id = id, FirstName = "UpdatedOwner", LastName = "UPD", DateOfBirth = new DateOnly(1990, 5, 10) };
            var expectedDto = new CarOwnerDTORead(id, "UpdatedOwner", "UPD", new DateOnly(1990, 5, 10));

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetByIdAsync(id)).ReturnsAsync(existingCarOwner);
            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.UpdateAsync(It.IsAny<CarOwner>())).ReturnsAsync(updatedCarOwner);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CarOwnerDTORead>(updatedCarOwner)).Returns(expectedDto);
                       
            var result = await _service.UpdateCarOwnerAsync(id, carOwnerDto);
                        
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.FirstName, result.FirstName);
            Assert.Equal(expectedDto.LastName, result.LastName);
            Assert.Equal(expectedDto.DateOfBirth, result.DateOfBirth);
        }


        [Fact]
        public async Task DeleteCarOwnerAsync_ReturnsTrue_WhenCarOwnerIsDeleted()
        {            
            var id = 1;
            var carOwner = new CarOwner { Id = id, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 5, 10) };

            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetByIdAsync(id)).ReturnsAsync(carOwner);
            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.DeleteAsync(id)).ReturnsAsync(true);
                        
            var result = await _service.DeleteCarOwnerAsync(id);
                        
            Assert.True(result);
            _mockUnitOfWork.Verify(u => u.CarOwnerRepository.DeleteAsync(id), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

    }
}

