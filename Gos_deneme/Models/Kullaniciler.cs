using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Gos_deneme.Models
{
    public class Kullaniciler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? KullaniciID { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Rol { get; set; }

        public string? Ad { get; set; }
        public string? Soyad { get; set; }

        
    }
}
