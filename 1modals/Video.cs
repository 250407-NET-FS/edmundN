namespace modals;

public class Video
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<string> UploaderHistory { get; set; } = new List<string>();
}