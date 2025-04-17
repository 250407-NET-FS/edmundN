namespace modals;

public class AddToHistoryRequest
{
    public required Guid VideoId { get; set; }
    public required string Title { get; set; }
}