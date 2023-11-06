using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManegementSystem.Areas.Identity.Data;
using TaskManegementSystem.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskManegementDbContextConnection") ?? throw new InvalidOperationException("Connection string 'TaskManegementDbContextConnection' not found.");

builder.Services.AddDbContext<TaskManegementDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<TaskManegementSystemUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TaskManegementDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
