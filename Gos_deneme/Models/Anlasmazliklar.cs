using System.ComponentModel.DataAnnotations;

namespace Gos_deneme.Models
{
    public class Anlasmazliklar
    {
        [Key]
        public int AnlasmazlikID { get; set; }
        public int SiparisID { get; set; }
        public DateTime AnlasmazlikTarih { get; set; }
        public string? AnlasmazlikNedeni { get; set; }
        public string? AnlasmazlikDurumu { get; set; }
        public Siparisler? Siparisler { get; set; }
    }
}
