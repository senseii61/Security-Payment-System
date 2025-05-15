using System.ComponentModel.DataAnnotations;

namespace Gos_deneme.Models
{
    public class Odemeler
    {
        [Key]
        public int OdemeID { get; set; }
        public int SiparisID { get; set; }
        public DateTime OdemeGun { get; set; }
        public decimal OdemeTutar { get; set; }
        public Siparisler Siparisler { get; set; }
    }
}
