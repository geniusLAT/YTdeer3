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
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private ITelegramBotClient _botClient;

    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private ReceiverOptions _receiverOptions;

    private string _botToken;

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

        _botClient = new TelegramBotClient("7673939994:AAG8IpfGn6uxNIKr4jSLfGgIq8u53oW2clk"); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        // UpdateHander - обработчик приходящих Update`ов
        // ErrorHandler - обработчик ошибок, связанных с Bot API
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }

    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        // эта переменная будет содержать в себе все связанное с сообщениями
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

                        // From - это от кого пришло сообщение (или любой другой Update)
                        var user = message.From;

                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        if(message.Text == null)
                        {
                            await botClient.SendTextMessageAsync(
                            chat.Id,
                            "Что? Я же говорю, надо прислать ссылку на YouTube. Тут 0 букв. НОЛЬ. Карл", // отправляем то, что написал пользователь
                            replyToMessageId: message.MessageId // по желанию можем поставить этот параметр, отвечающий за "ответ" на сообщение
                            );
                        }

                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            message.Text!, // отправляем то, что написал пользователь
                            replyToMessageId: message.MessageId // по желанию можем поставить этот параметр, отвечающий за "ответ" на сообщение
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
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
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