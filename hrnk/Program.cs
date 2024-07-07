using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using hrnk.Data;
using hrnk.utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<hrnkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("hrnkContext") ?? throw new InvalidOperationException("Connection string 'hrnkContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AuthHelpers, AuthHelpers>();
builder.Services.AddSession(options =>
{
    // Cấu hình timeout và các tùy chọn khác
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
