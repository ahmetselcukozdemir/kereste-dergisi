using Kereste.BLL.Services.Abstract;
using Kereste.BLL.Services.Concrete;
using Kereste.DATA.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IContentService, ContentService>();


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
   name: "rating-update",
   pattern: "/rating-update/{newsID}",
   defaults: new { controller = "Home", action = "UpdateCountNews" });

app.MapControllerRoute(
    name: "team",
    pattern: "/dergi-ekibi/",
    defaults: new { controller = "Home", action = "Team" });


app.MapControllerRoute(
    name: "about",
    pattern: "/hakkimizda/",
    defaults: new { controller = "Home", action = "About" });

app.MapControllerRoute(
   name: "category-index",
   pattern: "{categorySelfLink}",
   defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
   name: "haber-detay",
   pattern: "{categoryName}/{selfLink}-{id}",
   defaults: new { controller = "Content", action = "Details" });

app.MapControllerRoute(
    name: "kategorilerim",
    pattern: "/admin/kategoriler/",
    defaults: new { controller = "Admin", action = "Category" });

app.MapControllerRoute(
    name: "yeni-yazi",
    pattern: "/admin/yeni-yazi/",
    defaults: new { controller = "Admin", action = "AddNews" });

app.MapControllerRoute(
    name: "yazilarim",
    pattern: "/admin/yazilarim/",
    defaults: new { controller = "Admin", action = "NewsList" });

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
