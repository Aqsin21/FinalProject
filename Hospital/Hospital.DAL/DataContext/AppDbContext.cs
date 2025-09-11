using Hospital.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Hospital.DAL.DataContext
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<News> Newss { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}
