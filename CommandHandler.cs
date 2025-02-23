using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordChatbot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += async (msg) => await HandleCommandAsync(msg);
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message && !message.Author.IsBot)) return;

            int argPos = 0;
            var context = new SocketCommandContext(_client, message);

            if (message.HasCharPrefix('!', ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                    await message.Channel.SendMessageAsync($"Erro: {result.ErrorReason}");
            }
        }
    }
}
