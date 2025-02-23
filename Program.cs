using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordChatbot;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;
    private static readonly string Token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

    static async Task Main()
    {
        var bot = new Program();
        await bot.RunBotAsync();
    }

    public Program()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            MessageCacheSize = 100
        });

        _commands = new CommandService();

        _services = ConfigureServices();
    }

    private IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton<CommandHandler>();

        return services.BuildServiceProvider();
    }

    public async Task RunBotAsync()
    {
        _client.Log += Log;

        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();

        var commandHandler = _services.GetRequiredService<CommandHandler>();
        await commandHandler.InitializeAsync();

        Console.WriteLine("Bot iniciado com sucesso!");
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }
}
