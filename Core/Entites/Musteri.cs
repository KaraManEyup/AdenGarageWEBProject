using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Musteri
    {
        public int Id { get; set; }
        public string Isim { get; set; }
        [Required(ErrorMessage = "{0} gereklidir.")]
        public string? Soyisim { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
        public string Telefon { get; set; }

        // Navigation Property
        public ICollection<Araba> Arabalar { get; set; }

    }
}
