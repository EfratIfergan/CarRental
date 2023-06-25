using CarRental.Common.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Cache
{
    public static class CarHistoryCache
    {
        private static readonly Dictionary<Guid, CarHistory> _cache;

        static CarHistoryCache()
        {
            _cache = new Dictionary<Guid, CarHistory>();
        }
        public static List<CarHistory> GetAllCarHistories()
        {
            return _cache.Values.ToList();
        }
        public static void AddCarHistoryLog(Guid carHistoryId, CarHistory carHistory)
        {
            if (!_cache.ContainsKey(carHistoryId))
            {
                _cache.Add(carHistoryId, carHistory);
            }
        }
        public static void ClearCache()
        {
            _cache.Clear();
        }
    }
}
