using System;
using System.Collections.Generic;

namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public partial class Route
{
    public int RouteId { get; set; }

    public string Origin { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public int? DistanceKm { get; set; }

    public int DurationMinutes { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
