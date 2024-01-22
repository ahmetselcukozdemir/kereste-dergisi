using Kereste.BLL.Services.Abstract;
using Kereste.BLL.Services.Concrete;
using Kereste.DATA.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();


var connectionString = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<KeresteDBContext>(t => t.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	  .AddCookie(options =>
	  {
		  options.LoginPath = "/admin/login/"; 
	  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "cikis-yap",
    pattern: "/admin/cikis-yap/",
    defaults: new { controller = "Admin", action = "Logout" });

app.MapControllerRoute(
    name: "profile",
    pattern: "/admin/profile/",
    defaults: new { controller = "Admin", action = "Profile" });

app.MapControllerRoute(
	name: "login-homepage",
	pattern: "/admin/homepage/",
	defaults: new { controller = "Admin", action = "Index" });

app.MapControllerRoute(
	name: "login",
	pattern: "/admin/login/",
	defaults: new { controller = "Admin", action = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
