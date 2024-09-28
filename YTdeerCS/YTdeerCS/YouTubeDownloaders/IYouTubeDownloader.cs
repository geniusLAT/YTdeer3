namespace YTdeerCS.YouTubeDownloaders;

public interface IYouTubeDownloader
{
    public Task<string> Download(string videoUrl);
}
