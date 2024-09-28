﻿using System;
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


class Program
{
    static async Task Main(string[] args)
    {
        var YtdlLogger = new ConsoleLogger<YouTubeDownloader>();

        YouTubeDownloader downloader = new(YtdlLogger);
        var task = Task.Run(()=>downloader.Download("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));

        var logger = new ConsoleLogger<TelegramBot>();

        var bot = new TelegramBot(logger);
        var botTask = Task.Run(() => bot.Main());

        while(true) {; }

    } 
}

