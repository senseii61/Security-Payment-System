using Gos_deneme.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gos_deneme.Models;
using Gos_deneme.Services;

var builder = WebApplication.CreateBuilder(args);

// ? Veritaban� ba�lant�s�
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//�ifre Hash'leme i�in gerekli olan servisleri ekle
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();


// ? MVC ve Session servislerini ekle
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // <-- EKLEND�

//Email servisi
builder.Services.AddScoped<EmailService>();


var app = builder.Build();

// ? Hata y�netimi ve g�venlik
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ? Session middleware'ini aktif et
app.UseSession(); // <-- EKLEND�

// ? Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
