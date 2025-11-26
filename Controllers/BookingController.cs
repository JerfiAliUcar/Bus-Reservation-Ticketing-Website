using Microsoft.AspNetCore.Mvc;

namespace Bus_Reservation_Ticketing_Website.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
