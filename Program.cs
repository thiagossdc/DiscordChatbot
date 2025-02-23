using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _services;
    private const string Token = "seu_token_aqui";

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

        _services = ConfigureServices();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton(_client);

        services.AddSingleton<CommandService>();

        return services.BuildServiceProvider();
    }

    public async Task RunBotAsync()
    {
        _client.Log += Log;
        _client.MessageReceived += async (msg) => await HandleMessageAsync(msg);

        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();

        Console.WriteLine("Bot iniciado com sucesso!");
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(SocketMessage msg)
    {
        var message = msg as SocketUserMessage;
        if (message == null || message.Author.IsBot) return;

        int argPos = 0;
        if (message.HasCharPrefix('!', ref argPos))
        {
            string command = message.Content.Substring(argPos).ToLower();
            await ExecuteCommandAsync(command, message);
        }
    }

    private async Task ExecuteCommandAsync(string command, SocketUserMessage message)
    {
        switch (command)
        {
            case "ping":
                await message.Channel.SendMessageAsync("Pong!");
                break;
            case "hello":
                await message.Channel.SendMessageAsync($"Olá, {message.Author.Username}!");
                break;
            case "info":
                var user = message.Author;
                await message.Channel.SendMessageAsync($"Usuário: {user.Username}#{user.Discriminator}\nID: {user.Id}\nCriado em: {user.CreatedAt}");
                break;
            default:
                await message.Channel.SendMessageAsync("Comando não reconhecido. Tente !ping, !hello ou !info.");
                break;
        }
    }
}
