using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YTdeerCS.YouTubeDownloaders;

public class YouTubeDownloader
{

    public async Task<string> Download(string videoUrl)
    {
        string url = videoUrl;

        var youtube = new YoutubeClient();

        
        var video = await youtube.Videos.GetAsync(url);

        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();

        using (var httpClient = new HttpClient())
        using (var stream = await httpClient.GetStreamAsync(audioStreamInfo.Url))
        using (var fileStream = new FileStream($"{video.Title}.mp3", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await stream.CopyToAsync(fileStream);
        }

        Console.WriteLine($"Аудио сохранено как {video.Title}.mp3");
        return $"{video.Title}.mp3";
    }
}
