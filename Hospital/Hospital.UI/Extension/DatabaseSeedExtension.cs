using Hospital.DAL.DataContext;
using Hospital.DAL.DataInitialize;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Hospital.UI.Extensions
{
    public static class DatabaseSeedExtensions
    {
        public static void SeedDatabase(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                DataInitializer.Seed(context);
                AdminSeed.SeedAdmins(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB init failed: {ex.Message}");
            }
        }
    }
}