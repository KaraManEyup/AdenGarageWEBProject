using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}
