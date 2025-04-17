using modals;
using System.Text.Json;

namespace repository;

public class JsonVideoRepo : IVideoRepo
{
    private List<Video> _video;
    private string _filePath;

    public JsonVideoRepo()
    {
        _filePath = Path.Combine("./4data/videos.json");
        _video = LoadVideos();
    }

    public List<Video> LoadVideos()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                return new List<Video>();
            }
            using FileStream stream = File.OpenRead(_filePath);
            var videos = JsonSerializer.Deserialize<List<Video>>(stream);
            return videos ?? new List<Video>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading videos from file: {ex.Message}");
            throw new Exception("Error loading videos from file");
        }
    }
    public Video addvideo(Video video)
    {
        if (video.Id == Guid.Empty)
        {
            video.Id = Guid.NewGuid();
        }

        if (_video.Any(v => v.Id == video.Id))
        {
            return video;
        }

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

    public Video GetById(Guid id)
    {
        return _video.FirstOrDefault(v => v.Id == id);
    }

    public List<Video> GetAll()
    {
        return _video;
    }
}