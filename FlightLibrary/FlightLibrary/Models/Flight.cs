using System;
using System.Collections.Generic;

namespace FlightLibrary.Models;

public partial class Flight
{
    public int Id { get; set; }

    public int FlightNumber { get; set; }

    public string Origin { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
