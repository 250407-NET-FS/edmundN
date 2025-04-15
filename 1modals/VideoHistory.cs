namespace modals;

public class VideoHistory
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string VideoUrl { get; set; }
    public required string Title { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime WatchedAt { get; set; }
}
