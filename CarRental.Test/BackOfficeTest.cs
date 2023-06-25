using CarRental.BL.BL;
using CarRental.Cache;
using CarRental.Common.ConfigurationDTOs;
using CarRental.Common.DbEntities;
using CarRental.Common.Enumerations;

namespace CarRental.test
{
    public class BackOfficeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateNewCarAsync_ValidCar_ReturnsSuccessResponse()
        {
            var cancellationToken = CancellationToken.None;
            var carToCreate = new Car
            {
                CarId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Description = "Test Car",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 100.0m,
                Discounts = 10.0m,
                MinimumDriverAge = 18,
                Status = true,
                CarGroup = CarGroups.Midsize,
                DriverAgeGroup = DriverAgeGroups.From18,
                Location = Locations.TelAviv
            };

            var bl = new BackOfficeBL();

            var actualResponse = await bl.CreateNewCarAsync(carToCreate, cancellationToken);

            Assert.IsTrue(actualResponse.IsSuccess);
        }

        [Test]
        public async Task DeleteCarAsync_ExistingCarId_ReturnsSuccessResponse()
        {
            var cancellationToken = CancellationToken.None;
            var car = new Car
            {
                CarId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Description = "Test Car",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 100.0m,
                Discounts = 10.0m,
                MinimumDriverAge = 18,
                Status = true,
                CarGroup = CarGroups.Midsize,
                DriverAgeGroup = DriverAgeGroups.From18,
                Location = Locations.TelAviv
            };

            CarCache.AddCar(car);

            var bl = new BackOfficeBL();
            var actualResponse = await bl.DeleteCarAsync(car.CarId, cancellationToken);

            Assert.IsTrue(actualResponse.IsSuccess);
        }

        [Test]
        public async Task DeleteCarAsync_NonexistentCarId_ReturnsErrorResponse()
        {
            var cancellationToken = CancellationToken.None;
            var carId = Guid.NewGuid();

            var bl = new BackOfficeBL();

            try
            {
                var actualResponse = await bl.DeleteCarAsync(carId, cancellationToken);

                Assert.Fail("Expected BadRequestException was not thrown.");
            }
            catch (BadRequestException ex)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ex.StatusCode);
                Assert.AreEqual($"Car with Car Id ({carId}) not found", ex.Message);
            }
        }

        [Test]
        public async Task UpdateCarAsync_ValidCar_ReturnsSuccessResponse()
        {
            var cancellationToken = CancellationToken.None;
            var carToUpdate = new Car
            {
                CarId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Description = "Update Car 1 ",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 100.0m,
                Discounts = 10.0m,
                MinimumDriverAge = 18,
                Status = true,
                CarGroup = CarGroups.Midsize,
                DriverAgeGroup = DriverAgeGroups.From18,
                Location = Locations.TelAviv
            };

            var bl = new BackOfficeBL();

            var actualResponse = await bl.UpdateCarAsync(carToUpdate, cancellationToken);

            Assert.IsTrue(actualResponse.IsSuccess);
        }

        [Test]
        public async Task UpdateCarAsync_InvalidCar_ThrowsBadRequestException()
        {
            var cancellationToken = CancellationToken.None;
            var carToUpdate = new Car
            {
                CarId = Guid.Empty,
                Description = "Car 1",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 100.0m,
                Discounts = 10.0m,
                MinimumDriverAge = 18,
                Status = true,
                CarGroup = CarGroups.Midsize,
                DriverAgeGroup = DriverAgeGroups.From18,
                Location = Locations.TelAviv
            };

            var bl = new BackOfficeBL();

            try
            {
                var actualResponse = await bl.UpdateCarAsync(carToUpdate, cancellationToken);

                Assert.Fail("Expected BadRequestException was not thrown.");
            }
            catch (BadRequestException ex)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, ex.StatusCode);
                Assert.AreEqual($"Car with Car Id ({carToUpdate.CarId}) not found", ex.Message);
            }
        }
    }
}