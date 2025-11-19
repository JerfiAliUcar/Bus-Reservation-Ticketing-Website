using System;
using System.Collections.Generic;
using Bus_Reservation_Ticketing_Website.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Route = Bus_Reservation_Ticketing_Website.Data.Entity.Route;
namespace Bus_Reservation_Ticketing_Website.Data;

public partial class AppDbContext : IdentityDbContext<AppUser>
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=BusReservationDB;User Id=sa;Password=(Albatros1905);TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); //Identity framework için ekledik


        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD0DEF3CD6");

            entity.HasIndex(e => e.Pnr, "UQ__Bookings__C5773DD36195211F").IsUnique();

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BookingStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Confirmed");
            entity.Property(e => e.Pnr)
                .HasMaxLength(10)
                .HasColumnName("PNR");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PK__Buses__6A0F60950824D4F2");

            entity.Property(e => e.BusId).HasColumnName("BusID");
            entity.Property(e => e.FirmName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.PlateNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__Routes__80979AAD7053CA87");

            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.Destination).HasMaxLength(50);
            entity.Property(e => e.Origin).HasMaxLength(50);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B69B357C956");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.BusId).HasColumnName("BusID");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");

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
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC627B1C36A3D");

            entity.HasIndex(e => new { e.ScheduleId, e.SeatNumber }, "UQ_Schedule_Seat").IsUnique();

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PassengerGender).HasMaxLength(1);
            entity.Property(e => e.PassengerName).HasMaxLength(100);
            entity.Property(e => e.PassengerTckn)
                .HasMaxLength(11)
                .HasColumnName("PassengerTCKN");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");

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
            entity.HasKey(e => e.LogId).HasName("PK__TicketAu__5E5499A89E344435");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.OldPassengerName).HasMaxLength(100);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
