using Discord.Commands;

namespace DiscordChatbot
{
    public class GeneralCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong!");
        }

        [Command("hello")]
        public async Task HelloAsync()
        {
            await ReplyAsync($"Olá, {Context.User.Username}!");
        }

        [Command("info")]
        public async Task InfoAsync()
        {
            var user = Context.User;
            await ReplyAsync($"Usuário: {user.Username}#{user.Discriminator}\nID: {user.Id}\nCriado em: {user.CreatedAt}");
        }
    }

}
