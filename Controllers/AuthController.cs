using Bus_Reservation_Ticketing_Website.Data;
using Bus_Reservation_Ticketing_Website.Data.Entity;
using Bus_Reservation_Ticketing_Website.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bus_Reservation_Ticketing_Website.Controllers
{
    public class AuthController : Controller
    {
        //private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(/*AppDbContext db,*/ UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            //_db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email,  //Veritabanında username email olacak. Identity için username zorunluymuş
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true //E-posta doğrulamasını atlamak için
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {                
                await _userManager.AddToRoleAsync(user, "Member");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public IActionResult Login(string? redirectUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz e-posta");
                return View(model);
            }

            var results = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (results.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Geçersiz Email veya Şifre");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //[HttpPost]
        //public IActionResult Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userDb= _db.Users.FirstOrDefault(u=>u.Email == model.Email);
        //        if(userDb != null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Bu e-posta zaten kayıtlı.");
        //            return View(model);
        //        }
        //        if(model.Password != model.ConfirmPassword)
        //        {
        //            ModelState.AddModelError(string.Empty, "Şifreler uyuşmuyor.");
        //            return View(model);
        //        }

        //        var newUser = new AppUser
        //        {
        //            Email = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            PasswordHash = model.Password,
        //            PhoneNumber = model.PhoneNumber,
        //        };
        //        _db.Users.Add(newUser);
        //        _db.SaveChanges();
        //        return RedirectToAction("Login");
        //    }

        //    return View(model);
        //}


    }
}
