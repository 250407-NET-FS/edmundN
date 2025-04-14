using System.Text.Json;
using modals;

namespace repository;

public class JsonHistoryRepo : IHistoryRepository
{
    private List<VideoHistory> _history;
    private string _filePath;

    public JsonHistoryRepo(string filePath)
    {
        _filePath = Path.Combine("./4data/history.json");
        _history = new List<VideoHistory>();
        LoadHistory();
    }
    public List<VideoHistory> LoadHistory()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
            using FileStream stream = File.OpenRead(_filePath);
            return JsonSerializer.Deserialize<List<VideoHistory>>(stream) ?? new List<VideoHistory>();

        }
        catch
        {
            throw new Exception("Error loading users from file");
        }
    }
    public void SaveHistory()
    {
        string json = JsonSerializer.Serialize(_history);
        File.WriteAllText(_filePath, json);
    }
    public void updateHistory(VideoHistory history)
    {
        int index = _history.FindIndex(u => u.Url == history.Url);
        if (index != -1)
        {
            _history[index] = history;
            SaveHistory();
        }
    }
}