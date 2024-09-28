using Microsoft.Extensions.Logging;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YTdeerCS.YouTubeDownloaders;

public class YouTubeDownloader : IYouTubeDownloader
{
    private ILogger<YouTubeDownloader> _logger;

    public YouTubeDownloader(ILogger<YouTubeDownloader> logger)
    {
        _logger = logger;
    }   

    public async Task<string> Download(string videoUrl)
    {
        DateTime downloadStartTime = DateTime.Now;
        string url = videoUrl;

        var youtube = new YoutubeClient();

        _logger.LogInformation($"Attempt to download {videoUrl}");

        var video = await youtube.Videos.GetAsync(url);

        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();

        using (var httpClient = new HttpClient())
        using (var stream = await httpClient.GetStreamAsync(audioStreamInfo.Url))
        using (var fileStream = new FileStream($"downloads/{video.Title}.mp3", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await stream.CopyToAsync(fileStream);
        }

        var duration =DateTime.Now - downloadStartTime;
        _logger.LogInformation($"Audio saved as {video.Title}.mp3, it spent {duration}");
        return $"downloads/{video.Title}.mp3";
    }
}
