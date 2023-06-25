using CarRental.Common.Enumerations;

namespace CarRental.Common.DTOs
{
    public class SearchCarsByParamatersDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Locations? Location { get; set; }
        public DriverAgeGroups? DriversAgeGroup { get; set; }
        public CarGroups? CarGroup { get; set; }
    }
}
