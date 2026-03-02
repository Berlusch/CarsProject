using AutoMapper;
using CarsProject.Model;
using CarsProject.Repository.Common;
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
