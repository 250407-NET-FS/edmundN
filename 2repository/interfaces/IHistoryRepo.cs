using modals;

namespace repository;

public interface IHistoryRepo
{
    void Add(VideoHistory videoHistory);
    IEnumerable<VideoHistory> GetUserHistory(string username);
    void ClearUserHistory(string username);
    VideoHistory GetEntry(string username, Guid videoId);
    void Remove(VideoHistory videoHistory);
    IEnumerable<VideoHistory> GetAllHistory();

}