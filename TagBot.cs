// copyright notice

using TagBot.Commands;

using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using DSharpPlus.SlashCommands;


namespace TagBot {
    public class DapperBot {

        // called from Main() to start the bot asynchronously
        static async Task MainAsync() {

            // create the client object to represent the user account tied to the bot application
            var client = new DiscordClient(new DiscordConfiguration() {
                Token = Config.TOKEN,
                TokenType = TokenType.Bot,

                AutoReconnect = true, // automatically reconnect to the websocket when connection is lost

                Intents = DiscordIntents.AllUnprivileged, // we don't need any privilleged intents
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            });

            var commands = client.UseSlashCommands();

            // register our events with our client object
            client.Ready          += OnReady; 

            
            // register all commands from each of our modules
            commands.RegisterCommands<TagsModule>();

            // connect to the websocket and start our async processes
            await client.ConnectAsync();
            await Task.Delay(-1);

            // set our playing status on boot and trigger our tags when a message is sent that starts with "#"
            async Task OnReady   (DiscordClient sender, ReadyEventArgs e) => await client.UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity("over your servers!", DSharpPlus.Entities.ActivityType.Watching), DSharpPlus.Entities.UserStatus.DoNotDisturb);
        }   

        static void Main(string[] args) { MainAsync().GetAwaiter().GetResult(); } // trigger our async start method on boot
    }
}