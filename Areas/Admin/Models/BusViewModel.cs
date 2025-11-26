namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Models
{
    public class BusViewModel
    {
        public int BusId { get; set; }

        public string PlateNumber { get; set; } = null!;

        public string FirmName { get; set; } = null!;

        public string? Model { get; set; }

        public bool? IsActive { get; set; }
    }
}
