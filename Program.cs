using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Models.OnlineBookStore.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DevConnection")
    ?? throw new InvalidOperationException("Connection string 'DevConnection' not found.");

// Register a single DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Use Identity with this DbContext
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.

builder.Services.AddRazorPages();
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles.Initialize(services);
}

app.UseStaticFiles();

app.UseRouting();  // Make sure this comes before endpoint mappings

app.UseAuthentication();  // Ensure authentication is called before authorization
app.UseAuthorization();

app.MapRazorPages();

app.Run();