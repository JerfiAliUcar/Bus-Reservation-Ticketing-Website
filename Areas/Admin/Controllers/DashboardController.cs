using AspNetCoreGeneratedDocument;
using Bus_Reservation_Ticketing_Website.Areas.Admin.Models;
using Bus_Reservation_Ticketing_Website.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalRevenue = await _context.Bookings.Where(b => b.BookingStatus == "Confirmed").SumAsync(b => (decimal?)b.TotalAmount) ?? 0;

            var totalTickets = await _context.Tickets.CountAsync();

            var totalSchedules = await _context.Schedules.CountAsync();

            var unreadMessages = await _context.ContactMessages.Where(m => !m.IsRead).CountAsync();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalTickets = totalTickets;
            ViewBag.TotalSchedules = totalSchedules;
            ViewBag.UnreadMessages = unreadMessages;

            return View();
        }

        //public async Task<IActionResult> Index()
        //{
        //    var totalRenevueData = await _context.Database.SqlQuery<decimal>($"SELECT dbo.fn_GetTotalRevenue() as Value").FirstOrDefaultAsync();

        //    ViewBag.TotalRenevue = totalRenevueData;


        //    var stats = await _context.Database.SqlQuery<DashboardStatsViewModel>($"SELECT *  FROM v_DashboardStats").FirstOrDefaultAsync();
        //    if (stats != null)
        //    {
        //        ViewBag.TotalTickets = stats.TotalTickets;
        //        ViewBag.TotalSchedules = stats.ActiveSchedules;
        //        ViewBag.UnreadMessages = stats.UnreadMessages;
        //    }
        //    else
        //    {
        //        ViewBag.TotalTickets = 0;
        //        ViewBag.TotalSchedules = 0;
        //        ViewBag.UnreadMessages = 0;
        //    }

        //    return View();
        //}
    }
}
