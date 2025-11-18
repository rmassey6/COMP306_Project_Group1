using System;
using System.Collections.Generic;

namespace FlightLibrary.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int FlightId { get; set; }

    public int PassengerId { get; set; }

    public DateTime BookingTime { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;
}
