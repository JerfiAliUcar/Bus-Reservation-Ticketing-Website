using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bus_Reservation_Ticketing_Website.Models;
using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Data.Entity;

namespace Bus_Reservation_Ticketing_Website.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
    {        
        if (!ModelState.IsValid)
        {
            ViewBag.Message = "Mesajýnýz kaydedilemedi. Lütfen alanlarý kontrol edip tekrar deneyiniz.";
            ViewBag.MessageType = "danger";
            return View(contactViewModel);
        }

        var contactMessage = new ContactMessage
        {
            Name = contactViewModel.Name,
            Email = contactViewModel.Email,
            Subject = contactViewModel.Subject,
            Message = contactViewModel.Message,            
        };
        _dbContext.ContactMessages.Add(contactMessage);
        await _dbContext.SaveChangesAsync();

        ViewBag.Message = "Mesajýnýz baþarýyla gönderildi!";
        ViewBag.MessageType = "success";
       
        return View();
    }


}



