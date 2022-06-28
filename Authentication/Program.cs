using Microsoft.AspNetCore.Authorization;
using Authentication.Requirements;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Authentication.Data;
using Microsoft.AspNetCore.Identity;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AuthenticationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //builder.Services.AddDbContext<AuthenticationContext>(options =>
            //    options.UseSqlServer(connectionString));
            ////builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddUserStore<AuthenticationContext>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            #region Cookies
            // add cookies to the browser and give it a name and options
            builder.Services.AddAuthentication("GHE56S85HF647SNE7GLX72NH69DT35LD537Z").AddCookie("GHE56S85HF647SNE7GLX72NH69DT35LD537Z", options =>
            {
                options.Cookie.Name = "GHE56S85HF647SNE7GLX72NH69DT35LD537Z";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccesDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            });
            #endregion
            #region Policy's
            // All the policy's
            builder.Services.AddAuthorization(options =>
            {
                // admin policy
                options.AddPolicy("AdminOnly", policy => policy
                .RequireClaim("Admin"));

                // HR department policy
                options.AddPolicy("MustBelongToHRDepartment", policy => policy
                .RequireClaim("Department", "HR"));

                //HR Department Manager policy
                options.AddPolicy("HRManagerOnly", policy => policy
                .RequireClaim("Department", "HR")
                .RequireClaim("Manager")
                .Requirements.Add(new HRManagerProbationRequirement(3)));

                // user policy's
                options.AddPolicy("User", policy => policy
                .RequireClaim("Employee", "true"));
            });
            #endregion
            #region Injection handlers
            // add the dependency injection for the handler
            builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, MinimumWorkYearsRequirementHandler>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Index}/{id?}");

            // responsible for calling the authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}