using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ad soyad alanı gereklidir.")]
    [StringLength(100, ErrorMessage = "Ad soyad 100 karakterden uzun olamaz.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "E-posta alanı gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre alanı gereklidir.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; }
}
