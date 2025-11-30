using Microsoft.AspNetCore.Mvc;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
