using modals;

namespace repository;

public interface IHistoryService
{
    void AddToHistory(string username, string videoUrl, string title);
    IEnumerable<VideoHistory> GetUserHistory(string username);
    void ClearHistory(string username);
    void RemoveFromHistory(string username, string videoUrl);
}