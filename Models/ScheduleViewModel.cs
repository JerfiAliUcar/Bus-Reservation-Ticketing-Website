using Bus_Reservation_Ticketing_Website.Data.Entity;

namespace Bus_Reservation_Ticketing_Website.Models
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string DepartureLocation { get; set; }
        public decimal Price { get; set; }
        public DateTime DepartureTime { get; set; }
        public virtual Bus Bus { get; set; } = null!;
        public virtual Bus_Reservation_Ticketing_Website.Data.Entity.Route Route { get; set; } = null!;

    }
}
