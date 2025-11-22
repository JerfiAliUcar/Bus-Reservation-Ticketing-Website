using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Bu kod, "DefaultConnection" ismini hem appsettings.json'da hem de User Secrets'ta arar.
// User Secrets'ta bulursa (ki sen ekledin), oradaki GERÇEK şifreyi kullanır.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Bus_Reservation_Ticketing_Website.Data.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<Bus_Reservation_Ticketing_Website.Data.Entity.AppUser, Microsoft.AspNetCore.Identity.IdentityRole<int>>(options=>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
    .AddEntityFrameworkStores<Bus_Reservation_Ticketing_Website.Data.AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
