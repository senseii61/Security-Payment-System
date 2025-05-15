using Microsoft.AspNetCore.Mvc;
using Gos_deneme.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Gos_deneme.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gos_deneme.Services;
using System;

//GİRİŞ- KAYITOL KONTROLLERİ 
namespace Gos_deneme.Controllers
{
    public class AuthController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly EmailService _emailService;

        public AuthController(MyDbContext context, IPasswordHasher<ApplicationUser> passwordHasher, EmailService emailService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;

        }

        // Giriş Sayfası
        public IActionResult GirisYap() => View();

        [HttpPost]
        public IActionResult GirisYap(string email, string sifre)
        {
            var kullanici = _context.Kullaniciler.FirstOrDefault(k => k.Email == email);

            if (kullanici == null)
            {
                ViewBag.Mesaj = "Kullanıcı bulunamadı.";
                return View();
            }

            // Şifreyi karşılaştır (hash kontrolü)
            var sonuc = _passwordHasher.VerifyHashedPassword(
                new ApplicationUser(),
                kullanici.Password, // veritabanındaki hash
                sifre // kullanıcının girişte yazdığı şifre
            );

            if (sonuc == PasswordVerificationResult.Success)
            {
                // Giriş başarılı, oturumu başlat
                HttpContext.Session.SetString("Rol", kullanici.Rol);
                HttpContext.Session.SetString("KullaniciID", kullanici.KullaniciID.ToString());
                HttpContext.Session.SetString("SaticiID", kullanici.KullaniciID.ToString());


                return kullanici.Rol switch
                {
                    "Admin" => RedirectToAction("Index", "Admin"),
                    "Satici" => RedirectToAction("Index", "Satici"),
                    "Musteri" => RedirectToAction("Index", "Musteri"),
                    _ => RedirectToAction("GirisYap")
               
                };
            }
            else
            {
                

                ViewBag.Mesaj = "Şifre hatalı.";
                return View();
            }

        }


        // Kayıt Ol
        public IActionResult KayitOl() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KayitOl(Kullaniciler model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Hata = "Form bilgilerinde eksik var.";
                return View(model);
            }

            try
            {
                // Şifreyi hash'le
                var hashedPassword = _passwordHasher.HashPassword(new ApplicationUser(), model.Password);

                model.Password = hashedPassword;

                _context.Kullaniciler.Add(model);
                _context.SaveChanges();

                TempData["Mesaj"] = "Kayıt başarılı. Giriş yapabilirsiniz.";
                return RedirectToAction("GirisYap");
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Kayıt sırasında bir hata oluştu: " + ex.Message;
                return View(model);

            }
        }

        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("GirisYap");
        }
    
    
// GET: Şifremi Unuttum sayfası
[HttpGet]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }

        // POST: E-posta ile sıfırlama bağlantısı gönder
        [HttpPost]
        public async Task<IActionResult> SifremiUnuttum(string email)
        {
            var kullanici = await _context.Kullaniciler.FirstOrDefaultAsync(k => k.Email == email);

            if (kullanici == null)
            {
                ViewBag.Hata = "Bu e-posta adresiyle eşleşen bir kullanıcı bulunamadı.";
                return View();
            }

            // Yeni şifre oluştur (GUID tabanlı)
            var yeniSifre = Guid.NewGuid().ToString("N").Substring(0, 6);
            
            // ✉️ E-posta gönder
                        await _emailService.SifreSifirlaGonder(email, yeniSifre);
            
            // 🔐 Şifreyi hash'le
            var hashedPassword = _passwordHasher.HashPassword(new ApplicationUser(), yeniSifre);
            kullanici.Password = hashedPassword;
            _context.Kullaniciler.Update(kullanici);
            await _context.SaveChangesAsync();

            

            ViewBag.Basarili = "Yeni şifreniz e-posta adresinize gönderildi.";
            return View();
        }

    } 
}
