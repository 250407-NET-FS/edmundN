using modals;

namespace repository;

public interface IHistoryRepo
{
    void Add(VideoHistory videoHistory);
    IEnumerable<VideoHistory> GetUserHistory(string username);
    void ClearUserHistory(string username);
    VideoHistory GetEntry(string username, string videoUrl);
    void Remove(VideoHistory videoHistory);
}
