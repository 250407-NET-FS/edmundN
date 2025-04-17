using System.Text.Json;
using modals;

namespace repository;

public class JsonHistoryRepo : IHistoryRepo
{
    private List<VideoHistory> _history;
    private readonly string _filePath;

    public JsonHistoryRepo()
    {
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "4data");
        _filePath = Path.Combine(directoryPath, "history.json");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        _history = new List<VideoHistory>();
        LoadHistory();
    }

    private List<VideoHistory> LoadHistory()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<VideoHistory>();
            }

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<VideoHistory>();
            }

            var history = JsonSerializer.Deserialize<List<VideoHistory>>(json);
            _history = history ?? new List<VideoHistory>();
            return _history;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading history: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");

            try
            {
                File.WriteAllText(_filePath, "[]");
                _history = new List<VideoHistory>();
                return _history;
            }
            catch (Exception resetEx)
            {
                Console.WriteLine($"Failed to reset history file: {resetEx.Message}");
                throw new Exception($"Error loading history from file: {ex.Message}", ex);
            }
        }
    }

    private void SaveHistory()
    {
        try
        {
            string json = JsonSerializer.Serialize(_history);
            File.WriteAllText(_filePath, json);
        }
        catch
        {
            throw new Exception("Error saving history to file");
        }
    }

    public void Add(VideoHistory videoHistory)
    {
        var existingEntry = GetEntry(videoHistory.Username, videoHistory.VideoId);
        if (existingEntry != null)
        {
            existingEntry.WatchedAt = videoHistory.WatchedAt;
        }
        else
        {
            _history.Add(videoHistory);
        }
        SaveHistory();
    }

    public IEnumerable<VideoHistory> GetUserHistory(string username)
    {
        return _history.Where(h => h.Username == username)
                      .OrderByDescending(h => h.WatchedAt)
                      .ToList();
    }

    public void ClearUserHistory(string username)
    {
        _history.RemoveAll(h => h.Username == username);
        SaveHistory();
    }

    public VideoHistory GetEntry(string username, Guid videoId)
    {
        return _history.FirstOrDefault(h =>
            h.Username == username &&
            h.VideoId == videoId);
    }

    public void Remove(VideoHistory videoHistory)
    {
        _history.RemoveAll(h =>
            h.Username == videoHistory.Username &&
            h.VideoId == videoHistory.VideoId);
        SaveHistory();
    }

    public IEnumerable<VideoHistory> GetAllHistory()
    {
        return _history.ToList();
    }
}
