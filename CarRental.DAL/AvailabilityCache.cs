using CarRental.Common.DbEntities;
using System.Collections.Concurrent;

namespace CarRental.Cache
{
    public static class AvailabilityCache
    {
        private static readonly ConcurrentDictionary<Guid, Availability> _cache;

        static AvailabilityCache()
        {
            _cache = new ConcurrentDictionary<Guid, Availability>();

            var availability1 = new Availability
            {
                AvailableId = Guid.NewGuid(),
                CarId = new Guid("585c4b9b-2576-403b-9081-2ddfcc3d3664"),
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(7)
            };
            _cache.TryAdd(availability1.AvailableId, availability1);    

            var availability2 = new Availability
            {
                AvailableId = Guid.NewGuid(),
                CarId = new Guid("c3aa2311-3c28-49db-a92f-1da39c8d7580"),
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(10)
            };
            _cache.TryAdd(availability2.AvailableId, availability2);
        }

        public static Availability GetAvailability(Guid availabilityId)
        {
            if (_cache.ContainsKey(availabilityId))
            {
                _cache.TryGetValue(availabilityId, out Availability availability);
                return availability;
            }

            return null;
        }
        public static List<Availability> GetAllAvailability()
        {
            return _cache.Values.ToList();
        }
        public static List<Availability> GetAvailabilitiesByCarId(Guid carId)
        {
            var availabilities = new List<Availability>();

            foreach (Availability availability in _cache.Values)
            {
                if (availability.CarId == carId)
                {
                    availabilities.Add(availability);
                }
            }

            return availabilities;
        }

        public static void AddAvailability(Availability availability)
        {
            if (!_cache.ContainsKey(availability.AvailableId))
            {
                _cache.TryAdd(availability.AvailableId, availability);
            }
        }
        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}
