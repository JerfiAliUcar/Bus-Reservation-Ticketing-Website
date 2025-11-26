using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Controllers;

public class ScheduleController : Controller
{
    private readonly AppDbContext _context;

    public ScheduleController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string from, string to, DateTime? date)
    {
        var query = _context.Schedules
            .Include(s => s.Route)
            .Include(s => s.Bus)
            .AsQueryable();

        if (!string.IsNullOrEmpty(from))
        {
            query = query.Where(s => s.Route.Origin.Contains(from));
        }

        if (!string.IsNullOrEmpty(to))
        {
            query = query.Where(s => s.Route.Destination.Contains(to));
        }

        if (date.HasValue)
        {
            query = query.Where(s => s.DepartureTime.Date == date.Value.Date);
        }

        var results = await query
            .Where(s => s.DepartureTime > DateTime.Now)
            .OrderBy(s => s.DepartureTime)
            .ToListAsync();

        ViewBag.From = from;
        ViewBag.To = to;
        ViewBag.Date = date?.ToShortDateString();

        return View(results);
    }


    public async Task<IActionResult> Details(int id)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Bus)
            .Include(s => s.Route)
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

        if (schedule == null) return NotFound();

        
        var soldSeats = await _context.Tickets
            .Where(t => t.ScheduleId == id)
            .Select(t => t.SeatNumber)
            .ToListAsync();

        ViewBag.SoldSeats = soldSeats;

        return View(schedule);
    }


    public IActionResult ListSchedules()
    {
        var scheduleList = new List<ScheduleViewModel>();
        scheduleList = _context.Schedules
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
