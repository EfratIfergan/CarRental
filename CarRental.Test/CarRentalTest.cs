using CarRental.BL.BL;
using CarRental.Cache;
using CarRental.Common.DbEntities;
using CarRental.Common.DTOs;
using CarRental.Common.Enumerations;
using Nest;

namespace CarRental.Test
{
    public class CarRentalTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SearchCarsByParamatersAsync_ValidParameters_ReturnsMatchingCars()
        {
            var cancellationToken = CancellationToken.None;
            var searchCarsByParamatersDTO = new SearchCarsByParamatersDTO
            {
                CarGroup = CarGroups.FullSize,
                Location = Locations.TelAviv,
                DriversAgeGroup = DriverAgeGroups.From26,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            var car1 = new Car
            {
                CarId = new Guid(),
                Description = "Car 1",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 100.0m,
                Discounts = 10.0m,
                MinimumDriverAge = 26,
                Status = true,
                CarGroup = CarGroups.FullSize,
                DriverAgeGroup = DriverAgeGroups.From26,
                Location = Locations.TelAviv
            };

            var car2 = new Car
            {
                CarId = new Guid(),
                Description = "Car 2",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 160.0m,
                Discounts = 80.0m,
                MinimumDriverAge = 20,
                Status = true,
                CarGroup = CarGroups.Economy,
                DriverAgeGroup = DriverAgeGroups.From26,
                Location = Locations.Herzliya
            };

            var carsList = new List<Car> { car1, car2 };
            CarCache.AddListOfCars(carsList);

            var bl = new CarRentalBL(); 

            var actualResult = await bl.SearchCarsByParamatersAsync(searchCarsByParamatersDTO, cancellationToken);

            Assert.IsTrue(actualResult.IsSuccess);
        }

        [Test]
        public async Task GetAllCarsAsync_CarsExist_ReturnsListOfCars()
        {
            var cancellationToken = CancellationToken.None;

            var car1 = new Car
            {
                CarId = new Guid(),
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

            var car2 = new Car
            {
                CarId = new Guid(),
                Description = "Car 2",
                AvailableExtras = "Extra 1, Extra 2",
                Price = 160.0m,
                Discounts = 80.0m,
                MinimumDriverAge = 20,
                Status = true,
                CarGroup = CarGroups.Economy,
                DriverAgeGroup = DriverAgeGroups.From26,
                Location = Locations.Herzliya
            };

            var carsList = new List<Car> { car1, car2 };
            CarCache.AddListOfCars(carsList);

            var bl = new CarRentalBL(); 

            var actualResult = await bl.GetAllCarsAsync(cancellationToken);

            Assert.IsTrue(actualResult.IsSuccess);
            Assert.IsTrue(actualResult.Data?.Count() > 0);
        }

        [Test]
        public async Task GetAllCarsAsync_NoCarsExist_ReturnsEmptyList()
        {
            var cancellationToken = CancellationToken.None;
            CarCache.ClearCache();

            var bl = new CarRentalBL();

            var actualResult = await bl.GetAllCarsAsync(cancellationToken);

            Assert.IsTrue(actualResult.IsSuccess);
            Assert.IsTrue(actualResult.Data == null);
        }
    }
}
