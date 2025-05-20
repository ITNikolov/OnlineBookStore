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
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
  opts.UseSqlite($"Data Source={builder.Environment.ContentRootPath}/bookstore.db"));


// Use Identity with this DbContext
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
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
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();

    var services = scope.ServiceProvider;
    await SeedRoles.Initialize(services);
}

app.UseStaticFiles();

app.UseRouting();  

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseAuthentication();  
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();