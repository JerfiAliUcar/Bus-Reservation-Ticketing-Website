using Bus_Reservation_Ticketing_Website.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class TicketController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchPnr, string searchName)
        {
            var ticketsQuery = _context.Tickets
                .Include(t => t.Booking)
                .Include(t => t.Schedule).ThenInclude(s => s.Route)
                .Include(t => t.Schedule).ThenInclude(s => s.Bus)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchPnr))
            {
                ticketsQuery = ticketsQuery.Where(t => t.Booking.Pnr.Contains(searchPnr));
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                ticketsQuery = ticketsQuery.Where(t => t.PassengerName.Contains(searchName));
            }

            var tickets = await ticketsQuery
                .OrderByDescending(t => t.Booking.BookingDate)
                .ToListAsync();

            ViewBag.SearchPnr = searchPnr;
            ViewBag.SearchName = searchName;

            return View(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var ticket = await _context.Tickets.Include(t => t.Booking).FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket != null)
            {
                ticket.Booking.BookingStatus = "Cancelled";

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
