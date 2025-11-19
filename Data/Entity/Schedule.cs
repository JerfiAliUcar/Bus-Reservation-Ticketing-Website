using System;
using System.Collections.Generic;

namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int RouteId { get; set; }

    public int BusId { get; set; }

    public DateTime DepartureTime { get; set; }

    public decimal Price { get; set; }

    public virtual Bus Bus { get; set; } = null!;

    public virtual Route Route { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
