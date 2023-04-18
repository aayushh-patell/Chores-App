using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using FinalProject.Areas.Identity.Data;

namespace FinalProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<FinalProjectIdentityDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<FinalProjectIdentityDbContext>();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Chore}/{action=Index}/{item1?}/{item2?}/{item3?}");

        //app.MapControllerRoute(
        //    name: "Chores",
        //    pattern: "Chores/{item1?}/{item2?}/{item3?}",
        //    defaults: new { controller = "Chore", action = "Index" }
        //);

        app.MapControllerRoute(
            name: "Chores2",
            pattern: "chores/{item1?}/{item2?}/{item3?}",
            defaults: new { controller = "Chore", action = "Index" }
        );

        app.MapRazorPages();

        app.Run();
    }
}