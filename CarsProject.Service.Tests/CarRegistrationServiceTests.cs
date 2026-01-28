using AutoMapper;
using CarsProject.Common;
using CarsProject.Model;
using CarsProject.Repository.Common;
using CarsProject.WebApi.DTO;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarRegistrationServiceTests
    {
        private readonly Mock<IGenericRepository<CarRegistration>> _mockRepoRegistration;
        private readonly Mock<IGenericRepository<CarOwner>> _mockRepoOwner;
        private readonly Mock<IGenericRepository<CarModel>> _mockRepoModel;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CarRegistrationService _service;

        public CarRegistrationServiceTests()
        {
            _mockRepoRegistration = new Mock<IGenericRepository<CarRegistration>>();
            _mockRepoOwner = new Mock<IGenericRepository<CarOwner>>();
            _mockRepoModel = new Mock<IGenericRepository<CarModel>>();
            _mockMapper = new Mock<IMapper>();

            _service = new CarRegistrationService(
                _mockRepoRegistration.Object,
                _mockRepoOwner.Object,
                _mockRepoModel.Object
            );
        }

        [Fact]
        public async Task GetCarRegistrationByIdAsync_ReturnsCorrectDTO()
        {
            var id = 1;

            var carRegistration = new CarRegistration
            {
                Id = id,
                RegistrationNumber = "Registration 123",
                CarOwner = new CarOwner { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 5, 10) },
                CarModel = new CarModel
                {
                    Id = 1,
                    Name = "Corolla",
                    Abrv = "COR",
                    CarMake = new CarMake { Id = 1, Name = "Make X", Abrv = "MX" },
                    CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
                }
            };

            var expectedDto = new CarRegistrationReadDto(id, "Registration 123", "John Doe", "Corolla");

            _mockRepoRegistration.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carRegistration);
            _mockMapper.Setup(m => m.Map<CarRegistrationReadDto>(carRegistration)).Returns(expectedDto);

            var result = await _service.GetCarRegistrationByIdAsync(id);

            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task GetCarRegistrationsPagedAsync_ReturnsPagedFilteredSorted()
        {
            var carRegistrations = new List<CarRegistration>
            {
                new CarRegistration
                {
                    Id = 1,
                    RegistrationNumber = "Registration 123",
                    CarOwner = new CarOwner { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 5, 10) },
                    CarModel = new CarModel
                    {
                        Id = 1,
                        Name = "Corolla",
                        Abrv = "COR",
                        CarMake = new CarMake { Id = 1, Name = "Make X", Abrv = "MX" },
                        CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
                    }
                },
                new CarRegistration
                {
                    Id = 2,
                    RegistrationNumber = "Registration 456",
                    CarOwner = new CarOwner { Id = 2, FirstName = "Jane", LastName = "Joe", DateOfBirth = new DateOnly(1990, 7, 10) },
                    CarModel = new CarModel
                    {
                        Id = 2,
                        Name = "Civic",
                        Abrv = "CIV",
                        CarMake = new CarMake { Id = 3, Name = "Make X", Abrv = "MX" },
                        CarEngineType = new CarEngineType { Id = 4, Type = "Electric", Abrv = "EV" }
                    }
                }
            };

            var expectedDTOs = new List<CarRegistrationReadDto>
            {
                new CarRegistrationReadDto(1, "Registration 123", "John Doe", "Corolla"),
                new CarRegistrationReadDto(2, "Registration 456", "Jane Joe", "Civic")
            };

            _mockRepoRegistration.Setup(r => r.GetQuery(It.IsAny<PSFParameters>())).Returns(carRegistrations.AsQueryable());
            _mockMapper.Setup(m => m.Map<IEnumerable<CarRegistrationReadDto>>(It.IsAny<IEnumerable<CarRegistration>>()))
                       .Returns(expectedDTOs);

            var pfs = new PSFParameters
            {
                Paging = new PagingParameters { PageNumber = 1, PageSize = 2 }
            };

            var result = await _service.GetCarRegistrationsAsync(pfs);

            result.Should().BeEquivalentTo(expectedDTOs);
        }

        [Fact]
        public async Task AddCarRegistrationAsync_ReturnsAddedDTO()
        {
            var carOwner = new CarOwner { Id = 1, FirstName = "John", LastName = "Travolta", DateOfBirth = new DateOnly(1990, 7, 10) };
            var carModel = new CarModel
            {
                Id = 2,
                Name = "Golf",
                Abrv = "GLF",
                CarMake = new CarMake { Id = 3, Name = "Make X", Abrv = "MX" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
            };
            var carRegistration = new CarRegistration { Id = 1, RegistrationNumber = "Registration A", CarOwner = carOwner, CarModel = carModel };
            var expectedDto = new CarRegistrationReadDto(1, "Registration A", "John Travolta", "Golf");

            _mockRepoOwner.Setup(r => r.GetByIdAsync(carOwner.Id)).ReturnsAsync(carOwner);
            _mockRepoModel.Setup(r => r.GetByIdAsync(carModel.Id)).ReturnsAsync(carModel);
            _mockRepoRegistration.Setup(r => r.AddAsync(It.IsAny<CarRegistration>())).ReturnsAsync(carRegistration);
            _mockMapper.Setup(m => m.Map<CarRegistrationReadDto>(carRegistration)).Returns(expectedDto);

            var result = await _service.AddCarRegistrationAsync(carRegistration);

            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task UpdateCarRegistrationAsync_ReturnsUpdatedDTO()
        {
            var id = 1;
            var carOwner = new CarOwner { Id = 1, FirstName = "Ana", LastName = "Anić" };
            var carModel = new CarModel
            {
                Id = 2,
                Name = "Golf",
                Abrv = "GLF",
                CarMake = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DIE" }
            };
            var existing = new CarRegistration { Id = id, RegistrationNumber = "OldReg", CarOwner = carOwner, CarModel = carModel };
            var expectedDto = new CarRegistrationReadDto(id, "ZG1234AA", "Ana Anić", "Golf");

            _mockRepoRegistration.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
            _mockRepoOwner.Setup(r => r.GetByIdAsync(carOwner.Id)).ReturnsAsync(carOwner);
            _mockRepoModel.Setup(r => r.GetByIdAsync(carModel.Id)).ReturnsAsync(carModel);
            _mockRepoRegistration.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(existing);
            _mockMapper.Setup(m => m.Map<CarRegistrationReadDto>(existing)).Returns(expectedDto);

            existing.RegistrationNumber = "ZG1234AA";

            var result = await _service.UpdateCarRegistrationAsync(id, existing);

            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task DeleteCarRegistrationAsync_ReturnsTrue_WhenDeleted()
        {
            var id = 1;
            var carRegistration = new CarRegistration
            {
                Id = id,
                RegistrationNumber = "Registration A",
                CarOwner = new CarOwner { Id = 1, FirstName = "Jennifer", LastName = "Aniston", DateOfBirth = new DateOnly(1970, 7, 10) },
                CarModel = new CarModel
                {
                    Id = 2,
                    Name = "Golf",
                    Abrv = "GLF",
                    CarMake = new CarMake { Id = 3, Name = "Make X", Abrv = "MX" },
                    CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
                }
            };

            _mockRepoRegistration.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(carRegistration);
            _mockRepoRegistration.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteCarRegistrationAsync(id);

            result.Should().BeTrue();
        }
    }
}
