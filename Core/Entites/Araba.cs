﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Araba
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka zorunludur.")]
        public string Marka { get; set; }


        [Required(ErrorMessage = "Plaka zorunludur.")]
        public string Plaka { get; set; }

        [Required(ErrorMessage = "Model zorunludur.")]
        public string Model { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Kilometre negatif olamaz.")]
        public int Km { get; set; }

        [Required(ErrorMessage = "İşlem bilgisi zorunludur.")]
        public string Islem { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime? Tarih { get; set; }

     
        public int? MusteriId { get; set; }
        // Navigation Property

        public Musteri? Musteri { get; set; }

       
    }
}
