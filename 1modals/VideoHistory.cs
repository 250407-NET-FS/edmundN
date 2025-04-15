namespace modals;

public class VideoHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string VideoUrl { get; set; } = string.Empty;
    public DateTime WatchedAt { get; set; }

}