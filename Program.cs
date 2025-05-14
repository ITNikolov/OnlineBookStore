using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using OnlineBookStore.Models.OnlineBookStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("DevConnection")
    ?? throw new InvalidOperationException("Connection string 'DevConnection' not found.");

// Register a single DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Use Identity with this DbContext
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddRazorPages() 
    .AddRazorPagesOptions(options => {
        options.Conventions.AuthorizeAreaFolder("Admin", "/", "AdminOnly");
   });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Error/403";
});

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
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

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseAuthentication();  // Ensure authentication is called before authorization
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();