using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;
using CarsProject.Repository.Common;
using FluentAssertions;
using Moq;

namespace CarsProject.Service.Tests
{
    public class CarRegistrationServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICarRegistrationRepository> _mockCarRegistrationRepository;
        private readonly CarRegistrationService _service;
        private readonly Mock<ICarOwnerRepository> _mockCarOwnerRepository;
        private readonly Mock<ICarModelRepository> _mockCarModelRepository;
        
        public CarRegistrationServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _mockCarRegistrationRepository = new Mock<ICarRegistrationRepository>();
            _mockCarOwnerRepository = new Mock<ICarOwnerRepository>();
            _mockCarModelRepository = new Mock<ICarModelRepository>();            

            _mockUnitOfWork.Setup(uow => uow.CarRegistrationRepository).Returns(_mockCarRegistrationRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.CarOwnerRepository).Returns(_mockCarOwnerRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.CarModelRepository).Returns(_mockCarModelRepository.Object);

            _service = new CarRegistrationService(_mockUnitOfWork.Object, _mockMapper.Object);
        }


        [Fact]
        public async Task GetCarRegistrationByIdAsync_ReturnsCorrectCarRegistrationDTO()
        {

            var id = 1;


            var carRegistration = new CarRegistration
            {
                Id = id,
                RegistrationNumber = "Registration 123",                
                CarOwner = new CarOwner { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 5, 10) },
                CarModel = new CarModel { Id = 1, Name = "Corolla", Abrv = "COR",
                CarMake = new CarMake { Id = 1, Name = "Make X", Abrv = "MX" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
                }

            };


            var expectedDto = new CarRegistrationDTORead(id, "Registration 123", "John Doe", "Corolla");


            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.GetByIdAsync(id)).ReturnsAsync(carRegistration);


            _mockMapper.Setup(m => m.Map<CarRegistrationDTORead>(carRegistration)).Returns(expectedDto);


            var result = await _service.GetCarRegistrationByIdAsync(id);


            result.Should().BeEquivalentTo(expectedDto);
        }


        [Fact]
        public async Task GetCarRegistrationsPagedAsync_ReturnsPagedFilteredSortedCarRegistrations()
        {

            var pageNumber = 1;
            var pageSize = 2;
            var sortBy = "registration number";
            var filter = "id";


            var carRegistrations = new List<CarRegistration>
    {
        new CarRegistration
        {
            Id = 1,
            RegistrationNumber = "Registration 123",            
            CarOwner = new CarOwner { Id = 1, FirstName="John", LastName="Doe", DateOfBirth=new DateOnly(1990, 5, 10) },
            CarModel = new CarModel { Id = 1, Name = "Corolla", Abrv = "COR", CarMake = new CarMake { Id = 1, Name = "Make X", Abrv = "MX" }, 
                CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" } }

        },
        new CarRegistration
        {
            Id = 2,
            RegistrationNumber = "Registration 456",            
            CarOwner = new CarOwner { Id = 2, FirstName="Jane", LastName="Joe", DateOfBirth=new DateOnly(1990, 7, 10) },
            CarModel = new CarModel { Id = 2, Name = "Civic", Abrv="CIV", CarMake=new CarMake { Id = 3, Name = "Make X", Abrv = "MX" }, 
                CarEngineType = new CarEngineType { Id = 4, Type = "Electric", Abrv = "EV" } }

        }
    };

            var expectedDTOs = new List<CarRegistrationDTORead>
    {
        new CarRegistrationDTORead(1, "Registration 123", "John Doe", "Corolla"),
        new CarRegistrationDTORead(2, "Registration 456", "Jane Joe", "Civic")
    };


            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.GetAllCarRegistrationsAsync())
                .ReturnsAsync(carRegistrations);

            _mockMapper.Setup(m => m.Map<IEnumerable<CarRegistrationDTORead>>(It.IsAny<IEnumerable<CarRegistration>>()))
                .Returns(expectedDTOs);


            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.GetAllCarRegistrationsAsync()).ReturnsAsync(carRegistrations);
            _mockMapper.Setup(m => m.Map<IEnumerable<CarRegistrationDTORead>>(It.IsAny<IEnumerable<CarRegistration>>()))
                       .Returns(expectedDTOs);


            var result = await _service.GetCarRegistrationsPagedAsync(pageNumber, pageSize, sortBy, filter);


            result.Should().BeEquivalentTo(expectedDTOs);
        }


        [Fact]
        public async Task AddCarRegistrationAsync_ReturnsAddedCarRegistrationDTO()
        {
            var carRegistrationDto = new CarRegistrationDTOInsertUpdate("Registration A", 1, 2);
            var carOwner = new CarOwner { Id = 1, FirstName = "John", LastName = "Travolta", DateOfBirth = new DateOnly(1990, 7, 10) };
            var carModel = new CarModel { Id = 2, Name = "Golf", Abrv = "GLF", CarMake = new CarMake { Id = 3, Name = "Make X", Abrv = "MX" },
            CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
        };

            var carRegistration = new CarRegistration
            {
                Id = 1,
                RegistrationNumber = "Registration A",                
                CarOwner = carOwner,
                CarModel = carModel
            };

            var expectedDto = new CarRegistrationDTORead(1, "Registration A", "John Travolta", "Golf");
            
            var mockCarOwnerRepository = new Mock<ICarOwnerRepository>();
            mockCarOwnerRepository
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(carOwner);

            var mockCarModelRepository = new Mock<ICarModelRepository>();
            mockCarModelRepository
                .Setup(r => r.GetByIdAsync(2))
                .ReturnsAsync(carModel);

            _mockUnitOfWork
                .Setup(u => u.CarOwnerRepository)
                .Returns(mockCarOwnerRepository.Object);

            _mockUnitOfWork
                .Setup(u => u.CarModelRepository)
                .Returns(mockCarModelRepository.Object);
            
            _mockMapper.Setup(m => m.Map<CarRegistration>(carRegistrationDto)).Returns(carRegistration);
            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.AddAsync(carRegistration)).ReturnsAsync(carRegistration);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CarRegistrationDTORead>(carRegistration)).Returns(expectedDto);
            
            var result = await _service.AddCarRegistrationAsync(carRegistrationDto);
                        
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task UpdateCarRegistrationAsync_ValidInput_UpdatesAndReturnsDTO()
        {            
            var id = 1;

            var carRegistrationDto = new CarRegistrationDTOInsertUpdate(
                RegistrationNumber: "ZG1234AA",
                CarOwnerId: 1,
                CarModelId: 2
            );

            var existingCarRegistration = new CarRegistration { Id = id };
            var carOwner = new CarOwner { Id = 1, FirstName = "Ana", LastName = "Anić" };
            var carModel = new CarModel
            {
                Id = 2,
                Name = "Golf",
                Abrv = "GLF",
                CarMake = new CarMake { Id = 1, Name = "Toyota", Abrv = "TOY" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Diesel", Abrv = "DIE" }
            };

            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.GetByIdAsync(id))
                .ReturnsAsync(existingCarRegistration);
            _mockUnitOfWork.Setup(u => u.CarOwnerRepository.GetByIdAsync(carRegistrationDto.CarOwnerId))
                .ReturnsAsync(carOwner);
            _mockUnitOfWork.Setup(u => u.CarModelRepository.GetByIdAsync(carRegistrationDto.CarModelId))
                .ReturnsAsync(carModel);

            _mockMapper.Setup(m => m.Map(carRegistrationDto, existingCarRegistration));
            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.UpdateAsync(existingCarRegistration))
                .ReturnsAsync(existingCarRegistration);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1); 

            var expectedDto = new CarRegistrationDTORead(
                Id: id,
                RegistrationNumber: "ZG1234AA",
                CarOwnerFirstNameLastName: "Ana Anić",
                CarModelName: "Golf"
            );

            _mockMapper.Setup(m => m.Map<CarRegistrationDTORead>(existingCarRegistration))
                .Returns(expectedDto);
            
            var result = await _service.UpdateCarRegistrationAsync(id, carRegistrationDto);
            
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.RegistrationNumber, result.RegistrationNumber);
            Assert.Equal(expectedDto.CarOwnerFirstNameLastName, result.CarOwnerFirstNameLastName);
            Assert.Equal(expectedDto.CarModelName, result.CarModelName);
        }
                


        [Fact]
        public async Task DeleteCarRegistrationAsync_ReturnsTrue_WhenCarRegistrationIsDeleted()
        {

            var id = 1;


            var carOwner = new CarOwner { Id = 1, FirstName = "Jennifer", LastName="Aniston", DateOfBirth= new DateOnly(1970, 7, 10) };
            var carModel = new CarModel
            {
                Id = 2,
                Name = "Golf",
                Abrv = "GLF",
                CarMake = new CarMake { Id = 3, Name = "Make X", Abrv = "MX" },
                CarEngineType = new CarEngineType { Id = 2, Type = "Electric", Abrv = "EV" }
            };


            var carRegistration = new CarRegistration
            {
                Id = id,
                RegistrationNumber = "Registration A",                
                CarOwner = carOwner,
                CarModel = carModel
            };


            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.GetByIdAsync(id)).ReturnsAsync(carRegistration);
            _mockUnitOfWork.Setup(u => u.CarRegistrationRepository.DeleteAsync(id)).ReturnsAsync(true);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);


            var result = await _service.DeleteCarRegistrationAsync(id);


            Assert.True(result);
            _mockUnitOfWork.Verify(u => u.CarRegistrationRepository.DeleteAsync(id), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


    }
}

