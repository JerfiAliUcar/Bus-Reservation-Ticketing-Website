using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class ScheduleController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public ScheduleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var schedules = await _dbContext.Schedules
                .Include(s => s.Route)
                .Include(s => s.Bus)
                .OrderBy(s => s.DepartureTime)
                .ToListAsync();

            return View(schedules);
        }

        public IActionResult Create()
        {
            
            ViewBag.Buses = new SelectList(_dbContext.Buses.ToList(), "BusId", "PlateNumber");

            var routes = _dbContext.Routes.Select(r => new
            {
                r.RouteId,
                Display = $"{r.Origin} > {r.Destination} ({r.DistanceKm} km)"
            }).ToList();

            ViewBag.Routes = new SelectList(routes, "RouteId", "Display");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            ModelState.Remove("Bus");
            ModelState.Remove("Route");

            if (ModelState.IsValid)
            {
                _dbContext.Add(schedule);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa dropdownları tekrar doldur (Yoksa sayfa patlar.

            ViewBag.Buses = new SelectList(_dbContext.Buses.Where(b => b.IsActive == true).ToList(), "BusId", "PlateNumber", schedule.BusId);

            var routes = _dbContext.Routes.Select(r => new
            {
                r.RouteId,
                Display = $"{r.Origin} > {r.Destination} ({r.DistanceKm} km)"
            }).ToList();

            ViewBag.Routes = new SelectList(routes, "RouteId", "Display", schedule.RouteId);

            return View(schedule);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _dbContext.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            ViewBag.Buses = new SelectList(
                _dbContext.Buses.Where(b => b.IsActive == true),
                "BusId",
                "PlateNumber",
                schedule.BusId); // Seçili Otobüs BOŞ GELİYO BUNA BAK

            var routes = _dbContext.Routes.Select(r => new
            {
                r.RouteId,
                Display = $"{r.Origin} > {r.Destination} ({r.DistanceKm} km)"
            }).ToList();

            ViewBag.Routes = new SelectList(routes, "RouteId", "Display", schedule.RouteId); 

            return View(schedule);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }
            ModelState.Remove("Bus");
            ModelState.Remove("Route");

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(schedule);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dbContext.Schedules.Any(e => e.ScheduleId == schedule.ScheduleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Hata olursa dropdownları tekrar doldurması için (Seçili değerleri koruyarak )
            ViewBag.Buses = new SelectList(_dbContext.Buses.Where(b => b.IsActive == true), "BusId", "PlateNumber", schedule.BusId);

            var routeList = _dbContext.Routes.Select(r => new
            {
                r.RouteId,
                Display = $"{r.Origin} > {r.Destination}"
            }).ToList();
            ViewBag.Routes = new SelectList(routeList, "RouteId", "Display", schedule.RouteId);

            return View(schedule);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _dbContext.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _dbContext.Schedules.Remove(schedule);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
