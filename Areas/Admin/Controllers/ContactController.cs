using Bus_Reservation_Ticketing_Website.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Areas.Admin.Controllers
{
    public class ContactController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public ContactController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _dbContext.ContactMessages.OrderByDescending(m => m.SentDate).ToListAsync();
            return View(messages);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var message = await _dbContext.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _dbContext.ContactMessages.Remove(message);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _dbContext.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
