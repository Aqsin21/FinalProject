using Hospital.Business.Services.Abstract;
using Hospital.Business.Services.Concrete;
using Hospital.DAL.Repositories.Abstract;
using Hospital.DAL.Repositories.Concret;
using Mailing;
using Mailing.MailKitImplementations;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services)
        {
            // Repository & Service
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomService, RoomService>();

            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorService, DoctorService>();

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();

            // Generic
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Mail
            services.AddTransient<IMailService, MailKitMailService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}