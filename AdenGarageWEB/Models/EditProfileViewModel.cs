using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required]
    [Display(Name = "Ad")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Soyad")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "E-posta")]
    public string Email { get; set; }

    [Display(Name = "Doğum Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Display(Name = "Adres")]
    public string Address { get; set; }

    [Display(Name = "Cinsiyet")]
    public string Gender { get; set; }
}
