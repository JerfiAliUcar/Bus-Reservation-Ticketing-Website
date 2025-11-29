using System;
using System.Collections.Generic;
using Bus_Reservation_Ticketing_Website.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Route = Bus_Reservation_Ticketing_Website.Data.Entity.Route; // İsim çakışması çözümü
using Microsoft.AspNetCore.Identity;

namespace Bus_Reservation_Ticketing_Website.Data;

public partial class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Bus> Buses { get; set; }
    public virtual DbSet<Route> Routes { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<TicketAuditLog> TicketAuditLogs { get; set; }
    public virtual DbSet<ContactMessage> ContactMessages { get; set; }

    // GÜVENLİK VE ÇAKIŞMA ÖNLEMİ:
    // OnConfiguring metodunu kapatıyoruz çünkü ayarları Program.cs dosyasından alacağız.
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("..."); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity framework tabloları için ŞART!





        //  TABLO İSİMLERİNİ DÜZELT 
        modelBuilder.Entity<AppUser>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");


        // --- 2. USERS TABLOSU AYARLARI ---
        modelBuilder.Entity<AppUser>(b =>
        {
            b.ToTable("Users");

            // DİKKAT: b.Ignore(u => u.PhoneNumber); SATIRINI SİLDİK! (Artık oluşacak)

            b.Ignore(u => u.PhoneNumberConfirmed);
            b.Ignore(u => u.TwoFactorEnabled);
            b.Ignore(u => u.LockoutEnd);
            b.Ignore(u => u.LockoutEnabled);
            b.Ignore(u => u.AccessFailedCount);
        });


        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("trg_BookingCancellationAudit"));
            // ---------------------
            entity.HasKey(e => e.BookingId);
            entity.HasIndex(e => e.Pnr, "UQ_Bookings_PNR").IsUnique();
            entity.Property(e => e.BookingDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.BookingStatus).HasMaxLength(20).HasDefaultValue("Confirmed");
            entity.Property(e => e.Pnr).HasMaxLength(10).HasColumnName("PNR");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId).HasColumnType("int");
           
        });

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.BusId);

            entity.Property(e => e.FirmName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.PlateNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId);

            entity.Property(e => e.Destination).HasMaxLength(50);
            entity.Property(e => e.Origin).HasMaxLength(50);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId);

            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Bus).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedules_Buses");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedules_Routes");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId);

            entity.HasIndex(e => new { e.ScheduleId, e.SeatNumber }, "UQ_Schedule_Seat").IsUnique();

            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PassengerGender).HasMaxLength(1);
            entity.Property(e => e.PassengerName).HasMaxLength(100);
            entity.Property(e => e.PassengerTckn).HasMaxLength(11).HasColumnName("PassengerTCKN");

            entity.HasOne(d => d.Booking).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Bookings");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Schedules");
        });

        modelBuilder.Entity<TicketAuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.Property(e => e.ActionDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.OldPassengerName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}