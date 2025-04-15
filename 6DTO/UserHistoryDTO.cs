using modals;
public class UserHistoryResponse
{
    public required Users User { get; set; }
    public required IEnumerable<VideoHistory> VideoHistory { get; set; }
}
