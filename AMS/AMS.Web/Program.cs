using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using AMS.Data;
using AMS.Data.Models;

using AMS.Services.Models;
using AMS.Services.Infrastructure;

using AMS.Web.Infrastrucutre.Extensions;

using AMS.Controllers.Hubs;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AMSDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<AppSettingsServiceModel>(builder
    .Configuration
    .GetSection("AdministrationDetails"));

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();

builder.AddTransient();

builder.Services.AddDefaultIdentity<User>(options => {

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AMSDbContext>();

builder.Services.AddControllersWithViews(options
    => options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>()
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.PrepareDatabase();

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

app.MapDefaultAreaRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<BidHub>("/bidHub");

app.Run();
