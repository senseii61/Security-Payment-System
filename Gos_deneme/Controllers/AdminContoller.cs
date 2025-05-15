using Gos_deneme.Data;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly MyDbContext _context;

    public AdminController(MyDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var kullanicilar = _context.Kullaniciler.ToList();
        var siparisler = _context.Siparisler.ToList();
        var odemeler = _context.Odemeler.ToList();
        var anlasmazliklar = _context.Anlasmazliklar.ToList();

        ViewBag.Kullanicilar = kullanicilar;
        ViewBag.Siparisler = siparisler;
        ViewBag.Odemeler = odemeler;
        ViewBag.Anlasmazliklar = anlasmazliklar;

        return View();
    }
}

