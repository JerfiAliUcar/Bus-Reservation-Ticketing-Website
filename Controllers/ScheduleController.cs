using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ScheduleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public  IActionResult ListSchedules()
        {
            var scheduleList = new List<ScheduleViewModel>();
            scheduleList = _dbContext.Schedules
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .Select(s => new ScheduleViewModel
                {
                    Id = s.ScheduleId,
                    DepartureLocation = s.Route.Origin,
                    Price = s.Price,
                    DepartureTime = s.DepartureTime,
                    Bus = s.Bus,
                    Route = s.Route
                })
                .ToList();


            return View(scheduleList);
        }
    }
}
