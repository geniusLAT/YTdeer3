using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using NAudio;
using YoutubeExplode.Videos.Streams;
using YTdeerCS.YouTubeDownloaders;
using YTdeerCS.TelegramBot;
using YTdeerCS.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<ILogger<YouTubeDownloader>, ConsoleLogger<YouTubeDownloader>>()
            .AddSingleton<IYouTubeDownloader, YouTubeDownloader>()
            .AddSingleton<ILogger<TelegramBot>, ConsoleLogger<TelegramBot>>()
            .AddSingleton<ITelegramBot, TelegramBot>()
            .BuildServiceProvider();

        var bot = serviceProvider.GetService<TelegramBot>()!;
        await bot.PollAsync();
    } 
}

