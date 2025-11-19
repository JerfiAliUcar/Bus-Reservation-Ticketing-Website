using System;
using System.Collections.Generic;

namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public partial class Bus
{
    public int BusId { get; set; }

    public string PlateNumber { get; set; } = null!;

    public string FirmName { get; set; } = null!;

    public string? Model { get; set; }

    public int TotalSeats { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
