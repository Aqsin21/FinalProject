using Hospital.Business.Services.Abstract;
using Hospital.Business.Services.Concrete;
using Hospital.DAL.DataContext;
using Hospital.DAL.DataContext.Entities;
using Hospital.DAL.Repositories.Abstract;
using Hospital.DAL.Repositories.Concret;
using Mailing;
using Mailing.MailKitImplementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // 👈 login olmayan buraya gider
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;

                // Status code yerine direkt redirect olsun
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Redirect(context.RedirectUri); // redirect LoginPath
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<INewsRepository, NewsRepository>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddTransient<IMailService, MailKitMailService>();

            var app = builder.Build();

            // Production exception handler should be first
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/500");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            // Status code pages middleware should be early in the pipeline
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}