using Bus_Reservation_Ticketing_Website.Data;
// Çakışmayı önlemek için 'Route' sınıfına 'RouteEntity' takma adını verdim
using RouteEntity = Bus_Reservation_Ticketing_Website.Data.Entity.Route;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers 
{
    public class RouteController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public RouteController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var routes = await _dbContext.Routes.ToListAsync();
            return View(routes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RouteEntity route) 
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(route);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(route);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var route = await _dbContext.Routes.FindAsync(id);
            if (route != null)
            {
                _dbContext.Routes.Remove(route);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var routeDb = await _dbContext.Routes.FindAsync(id); 
            if (routeDb == null)
            {
                return NotFound();
            }
            return View(routeDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RouteEntity route) 
        {
            if (!ModelState.IsValid)
            {
                return View(route);
            }

            var routeDb = await _dbContext.Routes.FindAsync(route.RouteId); 
            if (routeDb == null)
            {
                return NotFound();
            }

            routeDb.Origin = route.Origin;
            routeDb.Destination = route.Destination;
            routeDb.DistanceKm = route.DistanceKm;
            routeDb.DurationMinutes = route.DurationMinutes;

            await _dbContext.SaveChangesAsync(); 
            return RedirectToAction("Index");
        }
    }
}