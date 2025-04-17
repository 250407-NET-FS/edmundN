using modals;

namespace repository;

public interface IVideoRepo
{
    List<Video> LoadVideos();
    Video addvideo(Video video);

    Video GetById(Guid id);
    void SaveVideos();

    List<Video> GetAll();


}