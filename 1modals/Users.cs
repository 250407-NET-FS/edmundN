namespace modals;

public class Users
{
    public Guid Id { get; set; }
    public required string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public required string Email { get; set; } = string.Empty;

}