using Bus_Reservation_Ticketing_Website.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Reservation_Ticketing_Website.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Register()
        {

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout() 
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
