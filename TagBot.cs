// copyright notice

using TagBot.Commands;

using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;

using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;


namespace TagBot {
    public class TagBot {
        static async Task MainAsync() {
            var client = new DiscordClient(new DiscordConfiguration() {
                Token = Config.TOKEN,
                TokenType = TokenType.Bot,

                AutoReconnect = true,

                Intents = DiscordIntents.AllUnprivileged,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            });

            var commands = client.UseCommandsNext(new CommandsNextConfiguration(){
                StringPrefixes = new string[] { Config.PREFIX },

                EnableDefaultHelp = true,
                EnableMentionPrefix = true,
                DmHelp = false
            });

            client.UseInteractivity(new InteractivityConfiguration()  { 
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout       = TimeSpan.FromSeconds(120)
            });


            client.Ready          += OnReady;
            client.MessageCreated += OnMessage;

            commands.SetHelpFormatter<TBHelpFormatter>();

            await client.ConnectAsync();
            await Task.Delay(-1);

            async Task OnReady   (DiscordClient sender, ReadyEventArgs e) => await client.UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity("with Tags!", DSharpPlus.Entities.ActivityType.Playing), DSharpPlus.Entities.UserStatus.DoNotDisturb);
            async Task OnMessage (DiscordClient sender, MessageCreateEventArgs e) { if(e.Message.Content.StartsWith("#")) TagsModule.RecieveTag(e); }
        }   

        static void Main(string[] args) { MainAsync().GetAwaiter().GetResult(); }

    }

    public class TBHelpFormatter : BaseHelpFormatter {
        protected DiscordEmbedBuilder builder;
        protected Command command;

        public TBHelpFormatter(CommandContext ctx) : base(ctx) { 
        builder = new DiscordEmbedBuilder()
            .WithColor(new DiscordColor(Config.COLORLESS))
            .WithFooter(Config.FOOTER);
        }

        public override BaseHelpFormatter WithCommand(Command command) {
            this.command = command;
            StringBuilder aliases = new StringBuilder();

            foreach (var i in command.Aliases) {
                aliases.Append(i);
                aliases.Append(", ");
            }

            var aliasString = aliases.ToString();

            try { builder.AddField("Aliases", aliasString.Substring(0, aliasString.Length - 2)); }
            catch {} // ignore exceptions

            builder
                .WithTitle($"Help with {command.Name} command:")
                .WithDescription(command.Description);

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds) {
            builder
                .WithTitle("Here ya go!")
                .WithDescription("Please visit **[our wiki](https://discord.feralra.in/tb/wiki)** for a full list of commands. Use `/help <command>` for help with a specific command.");

            return this;
        }

        public override CommandHelpMessage Build() => new CommandHelpMessage(embed: builder);
    }
}