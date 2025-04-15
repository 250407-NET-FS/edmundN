using System.Text.Json;
using modals;

namespace repository;

public class JsonHistoryRepo : IHistoryRepo
{
    private List<VideoHistory> _history;
    private readonly string _filePath;

    public JsonHistoryRepo()
    {
        _filePath = Path.Combine("./4data/history.json");
        _history = new List<VideoHistory>();
        LoadHistory();
    }

    private void LoadHistory()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
                _history = new List<VideoHistory>();
                return;
            }

            using FileStream stream = File.OpenRead(_filePath);
            _history = JsonSerializer.Deserialize<List<VideoHistory>>(stream) ?? new List<VideoHistory>();
        }
        catch
        {
            throw new Exception("Error loading history from file");
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
        var existingEntry = GetEntry(videoHistory.Username, videoHistory.VideoUrl);
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

    public VideoHistory GetEntry(string username, string videoUrl)
    {
        return _history.FirstOrDefault(h =>
            h.Username == username &&
            h.VideoUrl == videoUrl);
    }

    public void Remove(VideoHistory videoHistory)
    {
        _history.RemoveAll(h =>
            h.Username == videoHistory.Username &&
            h.VideoUrl == videoHistory.VideoUrl);
        SaveHistory();
    }
}
