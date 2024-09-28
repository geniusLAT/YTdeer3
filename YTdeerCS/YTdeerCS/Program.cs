using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using NAudio;
using YoutubeExplode.Videos.Streams;
using YTdeerCS.YouTubeDownloaders;


class Program
{
    static async Task Main(string[] args)
    {
        YouTubeDownloader downloader = new();
        var task = Task.Run(()=>downloader.Download("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));

        while(true) {; }

    } 
}

