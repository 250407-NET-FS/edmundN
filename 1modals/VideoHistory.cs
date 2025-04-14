namespace modals;

public class VideoHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; } = string.Empty;
    public List<string> Usersadded { get; set; } = new List<string>();

}