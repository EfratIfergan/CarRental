using CarRental.Common.DbEntities;
using CarRental.Common.Enumerations;
using System.Collections.Concurrent;

namespace CarRental.Cache
{
    public static class CarCache
    {
        private readonly static ConcurrentDictionary<Guid, Car> _cache;

        static CarCache()
        {
            _cache = new ConcurrentDictionary<Guid, Car>();

            var car1 = new Car
            {
                CarId = new Guid("585c4b9b-2576-403b-9081-2ddfcc3d3664"),
                Description = "Economy car with basic features",
                AvailableExtras = "GPS, Bluetooth",
                Price = 50.00m,
                Discounts = 0.00m,
                MinimumDriverAge = 18,
                Status = true,
                CarGroup = CarGroups.Economy,
                DriverAgeGroup = DriverAgeGroups.From18,
                Location = Locations.Jerusalem
            };
            _cache.TryAdd(car1.CarId, car1);

            var car2 = new Car
            {
                CarId = new Guid("c3aa2311-3c28-49db-a92f-1da39c8d7580"),
                Description = "Luxury car with advanced features",
                AvailableExtras = "GPS, Bluetooth, Leather seats",
                Price = 150.00m,
                Discounts = 10.00m,
                MinimumDriverAge = 26,
                Status = true,
                CarGroup = CarGroups.Luxury,
                DriverAgeGroup = DriverAgeGroups.From26,
                Location = Locations.TelAviv
            };
            _cache.TryAdd(car2.CarId, car2);

            var car3 = new Car
            {
                CarId = new Guid("84f77cf9-0629-4034-89f2-9baffbd480f7"),
                Description = "SUV with spacious interior",
                AvailableExtras = "GPS, Bluetooth, Sunroof",
                Price = 80.00m,
                Discounts = 5.00m,
                MinimumDriverAge = 18,
                Status = true,
                CarGroup = CarGroups.SUV,
                DriverAgeGroup = DriverAgeGroups.From18,
                Location = Locations.Herzliya
            };
            _cache.TryAdd(car3.CarId, car3);
        }

        public static Car GetCar(Guid carId)
        {
            if (_cache.ContainsKey(carId))
            {
                _cache.TryGetValue(carId, out Car car);
                return car;
            }

            return null;
        }

        public static List<Car> GetAllCars()
        {
            return _cache.Values.ToList();
        }

        public static void AddCar(Car car)
        {
            if (!_cache.ContainsKey(car.CarId))
            {
                _cache.TryAdd(car.CarId, car);
            }
        }

        public static void AddListOfCars(List<Car> cars)
        {
            foreach (var car in cars)
            {
                if (!_cache.ContainsKey(car.CarId))
                {
                    _cache.TryAdd(car.CarId, car);
                }
            }
        }

        public static void UpdateCar(Car car)
        {
            if (_cache.ContainsKey(car.CarId))
            {
                _cache.TryUpdate(car.CarId, car, _cache[car.CarId]);
            }
        }
   
        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}