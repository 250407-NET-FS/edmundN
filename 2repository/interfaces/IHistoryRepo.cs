using modals;

namespace repository;

public interface IHistoryRepository
{
    List<VideoHistory> LoadHistory();
    void SaveHistory(List<VideoHistory> history);

    void UpdateHistory(VideoHistory history);
}