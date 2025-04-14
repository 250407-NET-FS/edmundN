using modals;

namespace repository;

public interface IVideoRepo
{
    public List<Video> LoadVideos();
    public Video addvideo(Video video);
    public Video SaveVideos();

}