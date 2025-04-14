namespace modals;

public class Users
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<string> UserHistory { get; set; } = new List<string>();
}