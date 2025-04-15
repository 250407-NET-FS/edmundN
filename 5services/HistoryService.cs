using modals;
using repository;

namespace services;
public class HistoryService : IHistoryService
{
    private readonly IVideoRepo _VideoRepo;
    private readonly IUserRepo _UserRepo;
    private readonly IHistoryRepo _HistoryRepo;

    public HistoryService(
        IVideoRepo videoRepo,
        IUserRepo userRepo,
        IHistoryRepo historyRepo
    )
    {
        _VideoRepo = videoRepo;
        _UserRepo = userRepo;
        _HistoryRepo = historyRepo;
    }

    public void AddToHistory(string username, string videoUrl, string title)
    {
        var user = _UserRepo.GetUser(username);
        var video = _VideoRepo.GetByUrl(videoUrl);

        if (user == null)
        {
            throw new ArgumentException($"User '{username}' not found");
        }
        if (video == null)
        {
            throw new ArgumentException($"Video with URL '{videoUrl}' not found");
        }

        var videoHistory = new VideoHistory
        {
            Id = Guid.NewGuid(),
            Username = username,
            VideoUrl = videoUrl,
            Title = title,
            AddedAt = DateTime.UtcNow,
            WatchedAt = DateTime.UtcNow
        };

        _HistoryRepo.Add(videoHistory);
    }

    public IEnumerable<VideoHistory> GetUserHistory(string username)
    {
        var user = _UserRepo.GetUser(username);
        if (user == null)
        {
            throw new ArgumentException($"User '{username}' not found");
        }

        return _HistoryRepo.GetUserHistory(username);
    }

    public void ClearHistory(string username)
    {
        var user = _UserRepo.GetUser(username);
        if (user == null)
        {
            throw new ArgumentException($"User '{username}' not found");
        }

        _HistoryRepo.ClearUserHistory(username);
    }

    public void RemoveFromHistory(string username, string videoUrl)
    {
        var videoHistory = _HistoryRepo.GetEntry(username, videoUrl);
        if (videoHistory == null)
        {
            throw new ArgumentException($"History entry not found for user '{username}' and video '{videoUrl}'");
        }

        _HistoryRepo.Remove(videoHistory);
    }
}


