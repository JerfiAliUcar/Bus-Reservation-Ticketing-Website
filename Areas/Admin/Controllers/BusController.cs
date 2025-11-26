using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class BusController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public BusController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var buses = await _dbContext.Buses.ToListAsync();
            return View(buses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Bus bus)
        {
            if (ModelState.IsValid)
            {
                var addedBus = new Bus
                {
                    FirmName=bus.FirmName,
                    Model=bus.Model,
                    PlateNumber=bus.PlateNumber,
                    IsActive=bus.IsActive
                };
                _dbContext.Add(bus);
                await _dbContext.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(bus);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bus = await _dbContext.Buses.FindAsync(id);
            if (bus != null)
            {
                _dbContext.Buses.Remove(bus);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var busDb = _dbContext.Buses.Find(id);
            if (busDb == null)
            {
                return NotFound();
            }
            return View(busDb);
        }

        [HttpPost]
        public IActionResult Edit(Bus bus)
        {
            if (!ModelState.IsValid)
            {
                return View(bus);
            }

            var busDb = _dbContext.Buses.Find(bus.BusId);
            if (busDb == null)
            {
                return NotFound();
            }
            busDb.PlateNumber = bus.PlateNumber;
            busDb.FirmName = bus.FirmName;
            busDb.Model = bus.Model;
            busDb.IsActive = bus.IsActive;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
