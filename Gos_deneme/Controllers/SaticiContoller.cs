using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Gos_deneme.Data;
using Gos_deneme.Models;
using System.Linq;
using System;
using Gos_deneme.Services;


namespace Gos_deneme.Controllers
{
    public class SaticiController : Controller
    {
        private readonly MyDbContext _context;
        private readonly EmailService _emailService;

        public SaticiController(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
            {
                return View();
            }
        

        // Sipariş oluşturma formu
        [HttpGet]
        public IActionResult SiparisOlustur()
        {
            ViewBag.Musteriler = _context.Kullaniciler
                .Where(k => k.Rol == "Musteri")
                .ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SiparisOlustur(Siparisler siparis)
        {
            var saticiId = int.Parse(HttpContext.Session.GetString("SaticiID"));
            siparis.SaticiID = saticiId;
            siparis.SiparisTarih = DateTime.Now;

            _context.Siparisler.Add(siparis);
            await _context.SaveChangesAsync(); // SiparisID burada oluşur

            // SiparisID oluştu, şimdi e-posta gönder
            if (!string.IsNullOrEmpty(siparis.Email))
            {
                await _emailService.SendEmailAsync(
                    siparis.Email,
                    "Sipariş Bilginiz",
                    $"Merhaba, siparişiniz başarıyla oluşturuldu. Sipariş Numaranız: {siparis.SiparisID}"
                );
            }

            return RedirectToAction("Siparislerim");
        }





        // Satıcının oluşturduğu siparişleri listeler
        public IActionResult Siparislerim()
        {
            var saticiId = int.Parse(HttpContext.Session.GetString("SaticiID"));

            var siparisler = _context.Siparisler
                .Where(s => s.SaticiID == saticiId)
                .ToList();

            return View(siparisler);
        }

        // Durum güncelleme (form ve işlemi ayrı yazacağım)
        [HttpGet]
        public IActionResult DurumGuncelle(int id)
        {
            var siparis = _context.Siparisler.FirstOrDefault(s => s.SiparisID == id);
            if (siparis == null)
                return NotFound();

            return View(siparis);
        }

        [HttpPost]
        public IActionResult DurumGuncelle(int SiparisID, string SiparisDurum)
        {
            var siparis = _context.Siparisler.FirstOrDefault(s => s.SiparisID == SiparisID);
            if (siparis != null)
            {
                siparis.SiparisDurum = SiparisDurum;
                _context.SaveChanges();
            }
            return RedirectToAction("Siparislerim");
        }
    }
}
