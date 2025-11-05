using GymSystemBLL;
using GymSystemDAL.Data.Context;
using GymSystemDAL.Data.DataSeed;
using GymSystemDAL.Repositories.Classes;
using GymSystemDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<GymSystemDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));

            builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ISessionRepository, SessionRepository>();

            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));


            var app = builder.Build();

            #region Data Seed

            var Scope = app.Services.CreateScope();

            var dbContext = Scope.ServiceProvider.GetRequiredService<GymSystemDbContext>();

            //var PendingMigrations = dbContext.Database.GetPendingMigrations();
            //if (PendingMigrations?.Any() ?? false)
            //    dbContext.Database.Migrate();

            GymDbContextSeeding.SeedData(dbContext);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
