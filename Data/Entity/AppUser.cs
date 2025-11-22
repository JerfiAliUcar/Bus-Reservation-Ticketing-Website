using System;
using Microsoft.AspNetCore.Identity;


namespace Bus_Reservation_Ticketing_Website.Data.Entity;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

}
