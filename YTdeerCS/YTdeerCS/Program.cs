using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using NAudio;
using YoutubeExplode.Videos.Streams;

class Program
{
    static async Task Main(string[] args)
    {
        // Console.Write("Введите ссылку на YouTube: ");
        string url = "https://www.youtube.com/watch?v=bnCT0GXgLJg";//Console.ReadLine();

        var youtube = new YoutubeClient();

        // Получаем информацию о видео
        var video = await youtube.Videos.GetAsync(url);

        // Получаем аудиопотоки
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        //var audioStreamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
        var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();

        // Скачиваем аудиопоток
        using (var httpClient = new HttpClient())
        using (var stream = await httpClient.GetStreamAsync(audioStreamInfo.Url))
        using (var fileStream = new FileStream($"{video.Title}.mp3", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await stream.CopyToAsync(fileStream);
        }

        Console.WriteLine($"Аудио сохранено как {video.Title}.mp3");
    }
}
