using modals;

namespace repository;

public interface IHistoryService
{
    void AddToHistory(string username, Guid videoId, string title);
    IEnumerable<VideoHistory> GetUserHistory(string username);
    void ClearHistory(string username);
    void RemoveFromHistory(string username, Guid videoId);
}