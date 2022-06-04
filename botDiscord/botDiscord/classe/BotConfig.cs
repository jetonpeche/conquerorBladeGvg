using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace botDiscord.classe
{
    internal class BotConfig
    {
        public static DiscordSocketClient Client { get; private set; } = null!;
        public static CommandService Commande { get; private set; } = null!;

        IServiceProvider serviceProvider;

        public async Task DemarrerBot()
        {
            Client = new(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                GatewayIntents = Discord.GatewayIntents.All
            });

            Commande = new();

            await Client.LoginAsync(Discord.TokenType.Bot, Token.Get());
            await Client.StartAsync();
            serviceProvider = new ServiceCollection().AddSingleton<BasicCommande>().BuildServiceProvider();
            Client.Ready += BotPret;

            await Task.Delay(-1);
        }

        private async Task BotPret()
        {
            await Commande.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
            await Client.SetGameAsync("tuto discordNet");

            Client.MessageReceived += Message;
        }

        public async Task Message(SocketMessage _message)
        {
            int commandePos = 0;

            SocketUserMessage message = (SocketUserMessage)_message;

            if (message.Author.IsBot)
                return;

            // cherche si la commande existe et renvoie la position de la fonction
            // fonction n°1 => [command("test")]
            // fonction n°2 => etc
            if(message.HasStringPrefix("!", ref commandePos))
            {
                SocketCommandContext context = new(Client, message);
                await Commande.ExecuteAsync(context, commandePos, serviceProvider);
            }
        }
    }
}
