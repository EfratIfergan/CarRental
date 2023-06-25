using CarRental.BL.Interfaces;
using CarRental.Cache;
using CarRental.Common.ConfigurationDTOs;
using CarRental.Common.DbEntities;
using CarRental.Common.DTOs;
using CarRental.Common.Enumerations;

namespace CarRental.BL.BL
{
    public class BackOfficeBL : IBackOffice
    {
        public BackOfficeBL() { }
        public async Task<BaseResponse> CreateNewCarAsync(Car carToCreate, CancellationToken cancellationToken)
        {
            var validationErrors = ValidateCar(carToCreate);
            if (validationErrors != null && validationErrors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, validationErrors), (int)HttpStatusCode.BadRequest);

            carToCreate.CarId = Guid.NewGuid();
            CarCache.AddCar(carToCreate);

            var carHistoryLog = new CarHistory
            {
                CarHistoryId = Guid.NewGuid(),
                CarId = carToCreate.CarId,
                Activity = ActivityType.Create,
                ActivityDate = DateTime.Now
            };

            CarHistoryCache.AddCarHistoryLog(carHistoryLog.CarHistoryId, carHistoryLog);

            return new BaseResponse
            {
                IsSuccess = true
            };
        }
        public async Task<BaseResponse> DeleteCarAsync(Guid carId, CancellationToken cancellationToken)
        {
            var validationErrors = ValidateCarId(carId);
            if (validationErrors != null && validationErrors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, validationErrors), (int)HttpStatusCode.BadRequest);

            var existingCars = CarCache.GetCar(carId);
            if (existingCars == null)
                throw new BadRequestException($"Car with Car Id ({carId}) not found", (int)HttpStatusCode.BadRequest);

            existingCars.Status = false;

            CarCache.UpdateCar(existingCars);

            var carHistoryLog = new CarHistory
            {
                CarId = carId,
                Activity = ActivityType.Delete,
                ActivityDate = DateTime.Now,
                CarHistoryId = Guid.NewGuid()
            };

            CarHistoryCache.AddCarHistoryLog(carHistoryLog.CarHistoryId, carHistoryLog);

            return new BaseResponse
            {
                IsSuccess = true
            };
        }
        public async Task<BaseResponse> UpdateCarAsync(Car carToUpdate, CancellationToken cancellationToken)
        {
            var validationErrors = ValidateCar(carToUpdate);
            if (validationErrors != null && validationErrors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, validationErrors), (int)HttpStatusCode.BadRequest);

            var existingCars = CarCache.GetCar(carToUpdate.CarId);
            if (existingCars == null)
                throw new BadRequestException($"Car with Car Id ({carToUpdate.CarId}) not found", (int)HttpStatusCode.BadRequest);

            CarCache.UpdateCar(carToUpdate);

            var carHistoryLog = new CarHistory
            {
                CarHistoryId = Guid.NewGuid(),
                CarId = carToUpdate.CarId,
                Activity = ActivityType.Update,
                ActivityDate = DateTime.Now
            };

            CarHistoryCache.AddCarHistoryLog(carHistoryLog.CarHistoryId, carHistoryLog);

            return new BaseResponse
            {
                IsSuccess = true
            };

        }
        private List<string> ValidateCarId(Guid carId)
        {
            if (carId == null)
                return new List<string> { "Car Id should not be null" };

            if (!Guid.TryParse(carId.ToString(), out _))
                return new List<string> { "Car Id is not a valid guid" };

            return new List<string>();
        }
        private List<string> ValidateCar(Car car)
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(car.Description))
                validationErrors.Add("Description is required");

            if (car.Price <= 0)
                validationErrors.Add("Price should be a positive decimal value");

            if (car.Discounts < 0)
                validationErrors.Add("Discounts should be a non-negative decimal value");

            if (!Enum.IsDefined(typeof(CarGroups), car.CarGroup))
                validationErrors.Add("CarGroup is not valid");

            if (!Enum.IsDefined(typeof(Locations), car.Location))
                validationErrors.Add("Location is not valid");

            if (car.MinimumDriverAge > 120 || car.MinimumDriverAge < (int)DriverAgeGroups.Under18)
                validationErrors.Add("MinimumDriverAge is not valid");

            return validationErrors;
        }
    }
}
