using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform.Services;
using Online_Learning_Platform.DataContext;
using Online_Learning_Platform.Models;
using Online_Learning_Platform.CustomMiddleware;
using Online_Learning_Platform.Interfaces;
using Online_Learning_Platform.Repositories;

namespace Online_Learning_Platform
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();


			builder.Services.AddDbContext<DBContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


			builder.Services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<DBContext>();

            builder.Services.AddScoped<PermissionService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
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

            app.UseMiddleware<CheckUserActiveMiddleware>();

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
