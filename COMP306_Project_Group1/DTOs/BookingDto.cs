namespace COMP306_Project_Group1.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
