using Gos_deneme.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gos_deneme.Models;
using Gos_deneme.Services;

var builder = WebApplication.CreateBuilder(args);

// ? Veritabaný baðlantýsý
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Þifre Hash'leme için gerekli olan servisleri ekle
builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();


// ? MVC ve Session servislerini ekle
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // <-- EKLENDÝ

//Email servisi
builder.Services.AddScoped<EmailService>();


var app = builder.Build();

// ? Hata yönetimi ve güvenlik
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
app.UseSession(); // <-- EKLENDÝ

// ? Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
