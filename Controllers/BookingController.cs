using System.Security.Claims; // Kullanıcı ID'sini almak için
using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Reservation_Ticketing_Website.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar bilet alabilir
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // 1. ÖDEME SAYFASI (GET)
        // HTML Formundan gelen verileri otomatik alıyoruz
        public async Task<IActionResult> Create(int scheduleId, List<int> selectedSeats)
        {
            if (selectedSeats == null || !selectedSeats.Any())
            {
                TempData["Error"] = "Lütfen en az bir koltuk seçiniz.";
                return RedirectToAction("Details", "Schedule", new { id = scheduleId });
            }

            var schedule = await _context.Schedules
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null) return NotFound();

            // Verileri View'a taşı
            ViewBag.SelectedSeats = selectedSeats;
            // Listeyi string'e çevirip (5,6 gibi) POST metoduna saklamak için Viewbag'e atıyoruz
            ViewBag.SeatString = string.Join(",", selectedSeats);
            ViewBag.TotalPrice = selectedSeats.Count * schedule.Price;

            return View(schedule);
        }
        // 2. ÖDEMEYİ TAMAMLA (POST)
        // Kart bilgisi almıyoruz, sadece yolcu bilgilerini alıyoruz.
        [HttpPost]
        public async Task<IActionResult> Create(int scheduleId, string selectedSeats, string passengerName, string passengerTCKN, string passengerPhone, string passengerEmail)
        {
            // 1. Kullanıcı ID'sini bul (Identity'den)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account"); // Hata varsa logine at
            }

            // 2. Seferi bul (Fiyatı almak için)
            var schedule = await _context.Schedules.FindAsync(scheduleId);
            if (schedule == null) return NotFound();

            var seatList = selectedSeats.Split(',').Select(int.Parse).ToList();
            decimal totalAmount = seatList.Count * schedule.Price;

            // 3. TRANSACTION BAŞLATIYORUZ (Veritabanı Dersi İçin Kritik!)
            // Eğer Booking oluşur ama Ticket oluşurken hata verirse, her şeyi geri alacağız.
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // A) Booking (Satış) Kaydını Oluştur
                var booking = new Booking
                {
                    UserId = userId,
                    BookingDate = DateTime.Now,
                    BookingStatus = "Confirmed",
                    TotalAmount = totalAmount,
                    Pnr = GeneratePNR() // Rastgele PNR üret
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync(); // ID oluşması için kaydediyoruz

                // B) Her koltuk için Ticket Oluştur
                foreach (var seatNo in seatList)
                {
                    var ticket = new Ticket
                    {
                        BookingId = booking.BookingId, // Oluşan Booking ID
                        ScheduleId = scheduleId,
                        SeatNumber = seatNo,
                        PassengerName = passengerName, // Formdan gelen isim
                        PassengerTckn = passengerTCKN,
                        PassengerGender = "U", // Şimdilik Unisex (veya formdan alabilirsin)
                        PaidAmount = schedule.Price
                    };
                    _context.Tickets.Add(ticket);
                }

                await _context.SaveChangesAsync();

                // C) Her şey yolundaysa onayla
                await transaction.CommitAsync();

                // Başarılı sayfasına (veya Biletlerim sayfasına) git
                return RedirectToAction("MyBookings", "Booking"); // Bunu birazdan yapacağız
            }
            catch (Exception ex)
            {
                // Hata olursa işlemi geri al
                await transaction.RollbackAsync();
                // Loglama yapılabilir...
                return BadRequest("İşlem sırasında bir hata oluştu: " + ex.Message);
            }
        }

        // 3. BİLETLERİM SAYFASI (Şimdilik basit bir placeholder)
        public async Task<IActionResult> MyBookings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId)) return RedirectToAction("Login", "Account");

            var bookings = await _context.Bookings
                .Include(b => b.Tickets)
                .ThenInclude(t => t.Schedule)
                .ThenInclude(s => s.Route)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // Yardımcı Metot: Rastgele PNR Üretici (Örn: X92B4A)
        private string GeneratePNR()
        {
            return Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        }
    }
}