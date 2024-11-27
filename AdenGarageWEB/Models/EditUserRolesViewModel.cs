public class EditUserRolesViewModel
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public List<string> UserRoles { get; set; } = new List<string>();
    public List<string> AllRoles { get; set; } = new List<string>();
    public List<string> SelectedRoles { get; set; } = new List<string>();
}
