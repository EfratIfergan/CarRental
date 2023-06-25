using CarRental.Common.Enumerations;

namespace CarRental.Common.DbEntities
{
    public class CarHistory
    {
        public Guid CarHistoryId { get; set; }
        public Guid CarId { get; set; }
        public ActivityType Activity { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
