using BusinessObject;
using Repository.MailService;
using Repository.MailUtils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repository;
using System.Security.Claims;
namespace ShopWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                googleOptions.CallbackPath = "/login-google";
                googleOptions.Scope.Add("profile");
                googleOptions.Events.OnCreatingTicket = (context) =>
                {
                    var picture = context.User.GetProperty("picture").GetString();

                    context.Identity.AddClaim(new Claim("picture", picture));

                    return Task.CompletedTask;
                };
            });

            var connectionString = builder.Configuration.GetConnectionString("Project");

            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
            builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
            builder.Services.AddSingleton<IOtpService, OtpService>();

            var mailSettings = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailSettings);
            builder.Services.AddTransient<ISendGmailService, SendGmailService>();

            builder.Services.AddDbContext<ShopIdentityDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ShopIdentityDbContext>();



            builder.Services.AddSession((options) =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddDistributedMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
            app.MapHub<SignalRServer>("/signalrServer");
            app.MapRazorPages();

            app.Run();
        }
    }
}
