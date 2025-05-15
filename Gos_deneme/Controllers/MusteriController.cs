using Microsoft.AspNetCore.Mvc;
using Gos_deneme.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Gos_deneme.Controllers
{
    public class MusteriController : Controller
    {
        private readonly MyDbContext _context;

        public MusteriController(MyDbContext context)
        {
            _context = context;
        }

        // 🟢 Giriş sonrası ana panel
        public IActionResult Index()
        {
            var musteriId = int.Parse(HttpContext.Session.GetString("KullaniciID"));
            var siparisler = _context.Siparisler
                .Where(s => s.MusteriID == musteriId)
                .ToList();

            return View(siparisler);
        }

        // 🟢 Ödeme bekleyen siparişler
        public IActionResult OdemeBekleyenler()
        {
            var musteriId = int.Parse(HttpContext.Session.GetString("KullaniciID"));
            var bekleyenSiparisler = _context.Siparisler
                .Where(s => s.MusteriID == musteriId && s.SiparisDurum == "Ödeme Bekleniyor")
                .ToList();

            return View(bekleyenSiparisler);
        }
        public IActionResult Detay(int id)
        {
            var musteriId = int.Parse(HttpContext.Session.GetString("KullaniciID"));

            var siparis = _context.Siparisler.FirstOrDefault(s => s.SiparisID == id && s.MusteriID == musteriId);
            if (siparis == null)
            {
                return NotFound();
            }

            // Siparişe ait ödeme bilgisini al
            var odeme = _context.Odemeler.FirstOrDefault(o => o.SiparisID == id);

            // ViewModel oluştur (sipariş + ödeme bilgisi)
            var model = new
            {
                Siparis = siparis,
                OdemeTutar = odeme?.OdemeTutar ?? 0,
                
            };

            return View(model);
        }


    }
}
