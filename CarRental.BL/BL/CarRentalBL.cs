using CarRental.BL.Interfaces;
using CarRental.Cache;
using CarRental.Common.ConfigurationDTOs;
using CarRental.Common.DbEntities;
using CarRental.Common.DTOs;
using CarRental.Common.Enumerations;

namespace CarRental.BL.BL
{
    public class CarRentalBL : ICarRental
    {
        public CarRentalBL() { }
        public async Task<BaseResponseWithData<List<Car>>> GetAllCarsAsync(CancellationToken cancellationToken)
        {
            var carsQuery = CarCache.GetAllCars().AsQueryable();

            if (carsQuery == null)
                return new BaseResponseWithData<List<Car>>
                {
                    IsSuccess = true,
                    ErrorMessages = new List<string> { "Not found any cars." },
                    Data = null
                };

            carsQuery = carsQuery.Where(c=> c.Status == true);

            return new BaseResponseWithData<List<Car>>
            {
                IsSuccess = true,
                ErrorMessages = new List<string>(),
                Data = carsQuery.Count() > 0 ? carsQuery.ToList() : null
            };
        }
        public async Task<BaseResponseWithData<List<Car>>> SearchCarsByParamatersAsync(SearchCarsByParamatersDTO searchCarsByParamatersDTO, CancellationToken cancellationToken)
        {
            var validationErrors = ValidateSearchParamaters(searchCarsByParamatersDTO);
            if (validationErrors != null && validationErrors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, validationErrors), (int)HttpStatusCode.BadRequest);

            var carsQuery = CarCache.GetAllCars().AsQueryable();

            if (searchCarsByParamatersDTO.CarGroup.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.CarGroup == searchCarsByParamatersDTO.CarGroup);
            }

            if (searchCarsByParamatersDTO.Location.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.Location == searchCarsByParamatersDTO.Location);
            }

            if (searchCarsByParamatersDTO.DriversAgeGroup.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.MinimumDriverAge >= (int)searchCarsByParamatersDTO.DriversAgeGroup);
            }

            if (searchCarsByParamatersDTO.StartDate != null && searchCarsByParamatersDTO.EndDate != null)
            {
                var availabilityQuery = AvailabilityCache.GetAllAvailability().AsQueryable();
                availabilityQuery = availabilityQuery.Where(a => a.StartDate <= searchCarsByParamatersDTO.StartDate &&
                                           a.EndDate >= searchCarsByParamatersDTO.EndDate);

                var unavailableCarIds = availabilityQuery.Select(a => a.CarId).Distinct();
                carsQuery = carsQuery.Where(c => !unavailableCarIds.Contains(c.CarId));
            }

            if(carsQuery.Count() > 0)
            {
                return new BaseResponseWithData<List<Car>>
                {
                    IsSuccess = true,
                    ErrorMessages = new List<string>(),
                    Data = carsQuery.ToList()
                };
            }

            return new BaseResponseWithData<List<Car>>
            {
                IsSuccess = true,
                ErrorMessages = new List<string> { "Not found any cars." },
                Data = carsQuery.ToList()
            };
        }
        public async Task<BaseResponse> ReantACarByCarIdAsync(RentCarDTO rentCarDTO, CancellationToken cancellationToken)
        {
            var validationErrors = ValidateRentCarDTO(rentCarDTO);
            if (validationErrors != null && validationErrors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, validationErrors), (int)HttpStatusCode.BadRequest);

            var availabilitiesCars = AvailabilityCache.GetAvailabilitiesByCarId(rentCarDTO.CarId);

            var isAvailability = !availabilitiesCars.Any(c => c.StartDate <= rentCarDTO.StartDate &&
                                           c.EndDate >= rentCarDTO.EndDate);

            if (isAvailability)
            {
                var availability = new Availability
                {
                    AvailableId = Guid.NewGuid(),
                    CarId = rentCarDTO.CarId,
                    StartDate = rentCarDTO.StartDate,
                    EndDate = rentCarDTO.EndDate
                };
                AvailabilityCache.AddAvailability(availability);

                return new BaseResponse
                {
                    IsSuccess = true,
                    ErrorMessages = new List<string> { $"Car with carId {rentCarDTO.CarId} for the dates {rentCarDTO.StartDate.ToShortDateString()} - {rentCarDTO.EndDate.ToShortDateString()} has been successfully booked." }
                };
            }

            return new BaseResponse
            {
                IsSuccess = true,
                ErrorMessages = new List<string> { $"Car with carId {rentCarDTO.CarId} for the dates {rentCarDTO.StartDate.ToShortDateString()} - {rentCarDTO.EndDate.ToShortDateString()} is not available." }
            };
        }
        public async Task<BaseResponseWithData<List<CarHistory>>> GetAllCarHistoriesAsync(CancellationToken cancellationToken) {
            var carsHistoriesQuery = CarHistoryCache.GetAllCarHistories().AsQueryable();

            if (carsHistoriesQuery == null)
                return new BaseResponseWithData<List<CarHistory>>
                {
                    IsSuccess = true,
                    ErrorMessages = new List<string> { "Not found any CarHistories." },
                    Data = null
                };

            return new BaseResponseWithData<List<CarHistory>>
            {
                IsSuccess = true,
                ErrorMessages = new List<string>(),
                Data = carsHistoriesQuery.ToList()
            };
        }
        private List<string>? ValidateSearchParamaters(SearchCarsByParamatersDTO searchCarsByParamatersDTO)
        {
            var validationErrors = new List<string>();

            if (!DateTime.TryParse(searchCarsByParamatersDTO.StartDate.ToString(), out DateTime parsedStartDate))
                validationErrors.Add("StartDate is not in a valid format");
            else if (parsedStartDate < DateTime.Today)
                validationErrors.Add("StartDate cannot be in the past");

            if (!DateTime.TryParse(searchCarsByParamatersDTO.EndDate.ToString(), out DateTime parsedEndDate))
                validationErrors.Add("EndDate is not in a valid format");
            else if (parsedEndDate < DateTime.Today)
                validationErrors.Add("EndDate cannot be in the past");

            if (!Enum.IsDefined(typeof(Locations), searchCarsByParamatersDTO.Location))
                validationErrors.Add("Locations is not valid");

            if (!Enum.IsDefined(typeof(DriverAgeGroups), searchCarsByParamatersDTO.DriversAgeGroup))
                validationErrors.Add("DriversAgeGroup is not valid");

            if (!Enum.IsDefined(typeof(CarGroups), searchCarsByParamatersDTO.CarGroup))
                validationErrors.Add("CarGroups is not valid");

            return validationErrors;
        }
        private List<string> ValidateRentCarDTO(RentCarDTO rentCarDTO)
        {
            var validationErrors = new List<string>();
            DateTime startDateParsed = default;
            DateTime endDateParsed = default;

            if (rentCarDTO.CarId == null)
                validationErrors.Add("Car Id should not be null");

            if (!Guid.TryParse(rentCarDTO.CarId.ToString(), out _))
                validationErrors.Add("Car Id is not a valid guid");

            if ((!DateTime.TryParse(rentCarDTO.StartDate.ToString(), out startDateParsed)) ||
                (!DateTime.TryParse(rentCarDTO.EndDate.ToString(), out endDateParsed)))
                validationErrors.Add("Date is not in a valid format");

            if ((startDateParsed < DateTime.Today) ||
                (endDateParsed < DateTime.Today) ||
                (startDateParsed > endDateParsed))
                validationErrors.Add("Dates must be valid and cannot be in the past or have an end date before the start date");

            return validationErrors;
        }

    }
}
