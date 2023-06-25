namespace CarRental.Common.DTOs
{
    public class RentCarDTO
    {
        public Guid CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
