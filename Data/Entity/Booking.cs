using System;
using System.Collections.Generic;

namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public partial class Booking
{
    public int BookingId { get; set; }

    public string? UserId { get; set; }

    public DateTime? BookingDate { get; set; }

    public string Pnr { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string? BookingStatus { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
