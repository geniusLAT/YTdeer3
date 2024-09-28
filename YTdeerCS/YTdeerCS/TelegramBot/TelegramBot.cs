using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using System.Configuration;

namespace YTdeerCS.TelegramBot;

public class TelegramBot
{
    private ITelegramBotClient? _botClient;

    private ReceiverOptions? _receiverOptions;

    private string _botToken;

    //code example: https://habr.com/ru/articles/756814/

    public TelegramBot()
    {
        _botToken = ConfigurationManager.AppSettings["BotToken"]!;

        if (string.IsNullOrEmpty(_botToken))
        {
            throw new Exception("Bot token is not configured in App.config");
        }
    }


    public async Task Main()
    {

        _botClient = new TelegramBotClient("7673939994:AAG8IpfGn6uxNIKr4jSLfGgIq8u53oW2clk"); 
        _receiverOptions = new ReceiverOptions 
        {
            AllowedUpdates = new[] 
            {
                UpdateType.Message,
            },
        };

        using var cts = new CancellationTokenSource();

        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

        var me = await _botClient.GetMeAsync(); 
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); 
    }

    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message!;
                        var chat = message.Chat;

                        if (message.Text == "/start")
                        {
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Автор не несёт никакой юридической ответсвенности, ваши персональные " +
                                "данные гарантировано будут переданы рептилоидам жидомассонам, а потом украдены " +
                                "всеми, кому будет не лень. Все совпадения случайны, все песни вымышленны. " +
                                "Продолжая использование бота вы подтверждаете согласие на то, что вы старше 21 года" +
                                " и являетесь гражданином Зимбавбве." +
                                "\n\n" +
                                "Как пользоваться?\n" +
                                "Пришли в сообщении ссылку на YouTube и бот отошлёт вам mp3 файл, совпадающий с аудиодорожкой " +
                                "указанного в сообщении видео. Не работает с Rutube и прочей нечестью."
                                );
                            return;
                        }

                        var user = message.From;

                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        if(message.Text == null)
                        {
                            await botClient.SendTextMessageAsync(
                            chat.Id,
                            "Что? Я же говорю, надо прислать ссылку на YouTube. Тут 0 букв. НОЛЬ. Карл", 
                            replyToMessageId: message.MessageId 
                            );
                        }

                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            message.Text!, 
                            replyToMessageId: message.MessageId 
                            );

                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}