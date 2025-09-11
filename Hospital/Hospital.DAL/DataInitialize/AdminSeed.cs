using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using System.Linq;

namespace Hospital.DAL.DataInitialize
{
    public static class AdminSeed
    {
        public static void SeedAdmins(AppDbContext context)
        {
            // SuperAdmin ekle
            if (!context.AdminUsers.Any(u => u.Role == AdminRole.SuperAdmin))
            {
                var superAdmin = new AdminUser
                {
                    UserName = "superadmin",
                    Password = "Super123!", // düz şifre
                    Role = AdminRole.SuperAdmin
                };
                context.AdminUsers.Add(superAdmin);
            }

            // Admin ekle
            if (!context.AdminUsers.Any(u => u.Role == AdminRole.Admin))
            {
                var admin = new AdminUser
                {
                    UserName = "admin1",
                    Password = "Admin123!", // düz şifre
                    Role = AdminRole.Admin
                };
                context.AdminUsers.Add(admin);
            }

            context.SaveChanges();
        }
    }
}