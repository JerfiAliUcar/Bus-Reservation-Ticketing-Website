using System;
using System.Collections.Generic;

namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int BookingId { get; set; }

    public int ScheduleId { get; set; }

    public int SeatNumber { get; set; }

    public string PassengerName { get; set; } = null!;

    public string? PassengerTckn { get; set; }

    public string? PassengerGender { get; set; }

    public decimal PaidAmount { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Schedule Schedule { get; set; } = null!;
}
