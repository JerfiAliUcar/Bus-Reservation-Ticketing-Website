using System;
using System.Collections.Generic;

namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public partial class TicketAuditLog
{
    public int LogId { get; set; }

    public int? TicketId { get; set; }

    public string? ActionType { get; set; }

    public DateTime? ActionDate { get; set; }

    public string? OldPassengerName { get; set; }
}
