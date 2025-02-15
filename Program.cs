using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

class Program
{
    private DiscordSocketClient _client;
    private const string Token = "token"; // Substitua pelo seu token

    static async Task Main()
    {
        Program bot = new Program();
        await bot.RunBotAsync();
    }

    public async Task RunBotAsync()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        _client.MessageReceived += async (message) => await MessageReceived(message as SocketUserMessage);

        await _client.LoginAsync(TokenType.Bot, Token);
        await _client.StartAsync();

        await Task.Delay(-1); // bot rodando indefinidamente
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }

    private async Task MessageReceived(SocketUserMessage message)
    {
        if (message == null || message.Author.IsBot) return;

        if (message.Content.ToLower() == "!ping")
        {
            await message.Channel.SendMessageAsync("Pong!");
        }
        else if (message.Content.ToLower() == "!hello")
        {
            await message.Channel.SendMessageAsync($"Olá, {message.Author.Username}!");
        }
    }
}
