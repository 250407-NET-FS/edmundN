namespace modals;

public class Users
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Email { get; set; } = string.Empty;
    public List<string> UserHistory { get; set; } = new List<string>();
}