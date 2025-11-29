using Bus_Reservation_Ticketing_Website.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var users = _dbContext.Users.ToList();

            return View(users);
        }
    }
}
