using modals;

namespace repository;

public interface IVideoRepo
{
    List<Video> LoadVideos();
    Video addvideo(Video video);

    Video GetByUrl(string url);
    void SaveVideos();

}