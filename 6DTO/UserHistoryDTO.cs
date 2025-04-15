using modals;
public class UserHistoryResponse
{
    public Users User { get; set; }
    public IEnumerable<VideoHistory> VideoHistory { get; set; }
}
