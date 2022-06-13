namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // add cookies to the browser and give it a name and options
            builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccesDenied";

                // add the scheme so the middleware can locate which authentication service we try to use.
            });

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
                .RequireClaim("Manager"));
            });

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

            // responsible for calling the authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}