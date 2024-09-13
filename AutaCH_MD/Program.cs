using AutaCH_MD.Contexts;
using AutaCH_MD.Middlewares;
using AutaCH_MD.Models;
using AutaCH_MD.Services;
using Hangfire;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//configurare Hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddHangfireServer();



// Add services to the container.
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BidService>(); //serviciul BidService

builder.Services.AddDbContext<AppDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

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

app.UseAuthorization();

app.UseSession();

app.UseMiddleware<UserCookieMiddleware>();

// Configurarea Hangfire Dashboard și Server
app.UseHangfireDashboard();

app.UseHangfireServer();



// Adăugarea job-urilor recurente
app.MapGet("/configure-jobs", (BidService bidService) =>
{
    RecurringJob.AddOrUpdate("update-bid-status", () => bidService.UpdateBidStatus(), Cron.MinuteInterval(1));
    return Results.Ok("Job configured.");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
