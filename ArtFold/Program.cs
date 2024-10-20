using ArtFold.Data;
using ArtFold.Models;
using ArtFold.Services;
using CloudinaryDotNet;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace ArtFold
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Cấu hình DbContext
            builder.Services.AddDbContext<ArtFoldDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ArtFold")));

            // Cấu hình Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<ArtFoldDbContext>()
            .AddDefaultTokenProviders();

            // Cấu hình RoleManager và UserManager
            builder.Services.AddScoped<RoleManager<IdentityRole>>();
            builder.Services.AddScoped<UserManager<User>>();

            // Cấu hình cache và session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Cấu hình EmailSender
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("MailSetting"));

            // Cấu hình Cloudinary
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("cloudinary://732414891615852:5TXmd8o8OtXr2ki_skeFtxMlB8o@dfpvoiwxi"));
            cloudinary.Api.Secure = true;
            builder.Services.AddSingleton(cloudinary);

            // Cấu hình Google Authentication
            builder.Services.AddAuthentication()
            .AddCookie()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration["Google:ClientId"];
                options.ClientSecret = builder.Configuration["Google:ClientSecret"];
                options.CallbackPath = builder.Configuration["Google:CallbackPath"];
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Sắp xếp thứ tự Middleware
            app.UseRouting();
            app.UseSession();

            // Đảm bảo authentication và authorization
            app.UseAuthentication(); // Đảm bảo xác thực trước khi kiểm tra quyền truy cập
            app.UseAuthorization();

            // Định tuyến
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
