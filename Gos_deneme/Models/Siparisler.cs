using Gos_deneme.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gos_deneme.Models;

public class Siparisler
{
    [Key]
    public int? SiparisID { get; set; }

    public int? MusteriID { get; set; }
    public int? SaticiID { get; set; }

    public DateTime SiparisTarih { get; set; }
    public string? SiparisDurum { get; set; }
    public string? OdemeDurumu { get; set; }
    public decimal? OdemeTutar { get; set; }
    public string? UrunAdi { get; set; }

    // 👇 Bunlar net, ayrı ayrı navigation'lar
    [ForeignKey("MusteriID")]
    public Kullaniciler? Musteri { get; set; }
    [ForeignKey("SaticiID")]
    public Kullaniciler? Satici { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public ICollection<Odemeler>? Odemeler { get; set; }
    public ICollection<Anlasmazliklar>? Anlasmazliklar { get; set; }
}
