namespace COMP306_Project_Group1.DTOs
{
    public class FlightForCreationDto
    {
        public int FlightNumber { get; set; }
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
