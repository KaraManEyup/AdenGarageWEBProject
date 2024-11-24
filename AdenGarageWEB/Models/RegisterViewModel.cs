using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ad Soyad gereklidir.")]
    [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Telefon numarası gereklidir.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Cinsiyet seçimi gereklidir.")]
    public string Gender { get; set; } 

    [Required(ErrorMessage = "Şifre gereklidir.")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} ve en fazla {1} karakter olabilir.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Şifre doğrulama gereklidir.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
    public string ConfirmPassword { get; set; }
}
