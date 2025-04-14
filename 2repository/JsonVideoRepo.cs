using modals;
using System.Text.Json;

namespace repository;

public class JsonVideoRepo : IVideoRepo
{
    private List<Video> _video;
    private string _filePath;

    public JsonVideoRepo(string filePath)
    {
        _filePath = Path.Combine("./4data/videos.json");
        _video = new List<Video>();
        LoadVideos();
    }
    public List<Users> LoadVideos()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
            using FileStream stream = File.OpenRead(_filePath);
            return JsonSerializer.Deserialize<List<Users>>(stream) ?? new List<Users>();

        }
        catch
        {
            throw new Exception("Error loading users from file");
        }
    }
    public Video addvideo(Video video)
    {
        _video.Add(video);
        SaveVideos();
        return video;
    }
    public void SaveVideos()
    {
        try
        {
            using FileStream stream = File.Create(_filePath);
            JsonSerializer.Serialize<List<Video>>(stream, _video);
        }
        catch
        {
            throw new Exception("Error saving users to file");
        }
    }
}