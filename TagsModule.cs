// copyright notice

using System;
using System.Net;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Newtonsoft.Json.Linq;

namespace TagBot.Commands {
    public class TagsModule : ApplicationCommandModule {


        [SlashCommand("gif", "search for a gif")]
        public async Task GifTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();

            using (WebClient client = new WebClient()) {
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://g.tenor.com/v1/search?q={query}&key=LIVDSRZULELA&limit=15")); } catch { json = null; }

                var embed = new DiscordEmbedBuilder()
                    .WithTitle(query)
                    .WithImageUrl(Convert.ToString(json["results"][Config.random.Next(15)]["media"][0]["gif"]["url"]))
                    .WithColor(new DiscordColor(Config.COLORLESS))
                    .WithFooter(Config.FOOTER + " | Powered by Tenor")
                    .Build();

                response.AddEmbed(embed);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
            }  
        }

        [SlashCommand("image", "search for an image")]
        public async Task ImageTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("youtube", "search for a youtube video")]
        public async Task YoutubeTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false);   
        }

        [SlashCommand("wikipedia", "search for a wikipedia article")]
        public async Task WikipediaTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("gif", "get an AI answer from Wolfram alpha")]
        public async Task WolframAlphaTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();

            using (WebClient client = new WebClient()) {
                var embed = new DiscordEmbedBuilder()
                    .WithTitle(Config.UNIMPLEMENTED_ERROR)
                    .WithColor(new DiscordColor(Config.RED_COLOR))
                    .WithFooter(Config.FOOTER)
                    .Build();

                response.AddEmbed(embed);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
            }  
        }

        [SlashCommand("imdb", "search for a movie or show")]
        public async Task ImdbTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();

            using (WebClient client = new WebClient()) {                
                // ?s parameter is broken when using query variable. pagination is unsupported until fixed.


                // int page = 0;
                // int.TryParse(query.Split(" ")[query.Split(" ").Length - 1], out page); 

                // var search = JObject.Parse(client.DownloadString($"http://www.omdbapi.com/?s={query}&apikey={Config.Keys["omdb"]}"));
                // var id     = Convert.ToString(search["Search"][0]["imdbID"]);
                // var json   = JObject.Parse(client.DownloadString($"http://www.omdbapi.com/?i={id}&apikey={Config.Keys["omdb"]}"));
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"http://www.omdbapi.com/?t={query}&apikey={Config.KEYS["omdb"]}")); } catch { json = null; }

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER + " | Powered by OMDB")
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle($"    {Convert.ToString(json["Title"])} - {Convert.ToString(json["Production"])}"); } catch {}
                try { builder.WithDescription (Convert.ToString(json["Plot"])); }   catch {}
                try { builder.WithImageUrl    (Convert.ToString(json["Poster"])); } catch {}
                    

                try { builder.AddField("Rating",    Convert.ToString(json["Rated"]),        true); } catch {}
                try { builder.AddField("Runtime",   Convert.ToString(json["Runtime"]),      true); } catch {}
                try { builder.AddField("Director",  Convert.ToString(json["Director"]),     true); } catch {}
                try { builder.AddField("Score /10", Convert.ToString(json["imdbRating"]),   true); } catch {}
                try { builder.AddField("Awards",    Convert.ToString(json["Awards"]),       true); } catch {}
                try { builder.AddField("Languages", Convert.ToString(json["Language"]),     true); } catch {}
                try { builder.AddField("Profit",    Convert.ToString(json["BoxOffice"]),    true); } catch {}
                try { builder.AddField("Seasons",   Convert.ToString(json["TotalSeasons"]), true); } catch {}

                try { builder.AddField("Writers",   Convert.ToString(json["Writer"]), false); } catch {}
                try { builder.AddField("Starring",  Convert.ToString(json["Actors"]), false); } catch {}
                try { builder.AddField("Genres",    Convert.ToString(json["Genre"]),  false); } catch {}

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
            }  
        }

        [SlashCommand("lmgtfy", "show someone how to Google something")]
        public async Task LmgtfyTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(query)
                .WithUrl($"https://www.lmgtfy.app/?q={query.Replace(" ", "%20")}")
                .WithDescription("This service is not intended to empower arrogant assholes who believe they are better than you. Google is one of the most powerful tools available to us today, and this tool is designed solely to help remind you of when it can be helpful.")
                .WithColor(new DiscordColor(Config.COLORLESS));

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("mtg", "search for a Magic: The Gathering card")]
        public async Task MtgTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("hearthstone", "find a Hearthstone card")]
        public async Task HerthstoneTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("lol", "find a League of Legends champion")]
        public async Task LolTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("urban", "search for a word on Urban Dictionary")]
        public async Task UrbanTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder();

            using (var client = new WebClient()) {
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"http://api.urbandictionary.com/v0/define?term={query}"))["list"][0]; } catch { json = null; }

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER + " | Powered by Urban Dictionary")
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle       (Convert.ToString(json["word"]));       } catch {}
                try { builder.WithDescription (Convert.ToString(json["definition"]) + $"\n\nClick **[here]({Convert.ToString(json["permalink"])})** for more"); } catch {}
                    

                try { builder.AddField("Author", Convert.ToString(json["author"]),     true); } catch {}
                try { builder.AddField("Date",   Convert.ToString(json["written_on"]), true); } catch {}
                try { builder.AddField("ID",     Convert.ToString(json["defid"]),      true); } catch {}
                try { builder.AddField("Votes", $"👍 {Convert.ToString(json["thumbs_up"])} | 👎 {Convert.ToString(json["thumbs_down"])}", true); } catch {}

                try { builder.AddField("Example", Convert.ToString(json["example"]), false); } catch {}
                embed = builder;
            }

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("define", "find a definition for a word")]
        public async Task DefineTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("reverseimage", "perform a reverse image search")]
        public async Task ReverseimageTag(InteractionContext ctx, [Option("query", "attach an image")] DiscordMessageFile query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("steamgame", "find a steam game")]
        public async Task SteamgameTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("steamuser", "find a steam user")]
        public async Task SteamuserTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("anime", "search for information on an anime show")]
        public async Task AnimeTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();

            using (WebClient client = new WebClient()) {                
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://kitsu.io/api/edge/anime?filter[text]={query}"))["data"][0]["attributes"]; } catch { json = null; }

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER + " | Powered by Kitsu.io")
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle    ($"{Convert.ToString(json["titles"]["ja_jp"])} ({Convert.ToString(json["titles"]["en"])})"); } catch {}
                try { builder.WithDescription (Convert.ToString(json["description"])); }                                                  catch {}
                try { builder.WithImageUrl    (Convert.ToString(json["posterImage"]["original"])); }                                      catch {}

                try { builder.AddField("Date",       Convert.ToString(json["startDate"]),     true); }                                              catch {}
                try { builder.AddField("Popularity", Convert.ToString(json["averageRating"]) + "/100", true); }                                     catch {}
                try { builder.AddField("Episodes",   Convert.ToString(json["episodeCount"]),  true); }                                              catch {}
                try { builder.AddField("Rating", $" {Convert.ToString(json["ageRating"])} | ({Convert.ToString(json["ageRatingGuide"])})", true); } catch {}

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
            }  
        }

        [SlashCommand("crypto", "search for information on a cryptocurrency coin")]
        public async Task CryptoTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();

            using (WebClient client = new WebClient()) {                
                JToken metrics;
                JToken profile;

                try { metrics = JObject.Parse(client.DownloadString($"https://data.messari.io/api/v1/assets/{query}/metrics"))["data"]; } catch { return; }
                try { profile = JObject.Parse(client.DownloadString($"https://data.messari.io/api/v1/assets/{query}/profile"))["data"]; } catch { return; }

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER + " | Powered by Messari")
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle    ($"{Convert.ToString(profile["name"])} ({Convert.ToString(profile["symbol"])})"); } catch {}
                try { builder.WithDescription (Convert.ToString(profile["overview"])); } catch {}

                try { builder.AddField("Reward",       Convert.ToString(profile["block_reward"]),                                    true); } catch {}
                try { builder.AddField("Algorithm",    Convert.ToString(profile["mining_algorithm"]),                                true); } catch {}
                try { builder.AddField("Genesis Date", Convert.ToString(profile["genesis_block_date"]),                              true); } catch {}
                try { builder.AddField("Consensus",    Convert.ToString(profile["consensus_algorithm"]),                             true); } catch {}
                try { builder.AddField("Price",        Convert.ToString(metrics["market_data"]["price_usd"]) + " USD",               true); } catch {}
                try { builder.AddField("Last Hour",    Convert.ToString(metrics["market_data"]["percent_change_usd_last_1_hour"]),   true); } catch {}
                try { builder.AddField("Last Day",     Convert.ToString(metrics["market_data"]["percent_change_usd_last_24_hours"]), true); } catch {}
                try { builder.AddField("Market Cap",   Convert.ToString(metrics["marketcap"]["current_marketcap_usd"]),              true); } catch {}
                try { builder.AddField("Inflation",    Convert.ToString(metrics["supply"]["annual_inflation_percent"]) + "%",        true); } catch {}
                try { builder.AddField("Hashrate",     Convert.ToString(metrics["on_chain_data"]["hash_rate"]) + "%",                true); } catch {}

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
            }  
        }

        [SlashCommand("pokemon", "search for information on a Pokemon")]
        public async Task PokemonTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();

            using (WebClient client = new WebClient()) {                
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://some-random-api.ml/pokedex?pokemon={query}")); } catch { json = null; }

                string types       = string.Empty;
                string species     = string.Empty;
                string moves       = string.Empty;
                string groups      = string.Empty;
                string evolutions  = string.Empty;

                try { foreach (var i in JArray.Parse(Convert.ToString(json["type"])))                    types       += $"{i}, "; } catch {}
                try { foreach (var i in JArray.Parse(Convert.ToString(json["species"])))                 species     += $"{i}, "; } catch {}
                try { foreach (var i in JArray.Parse(Convert.ToString(json["abilities"])))               moves       += $"{i}, "; } catch {}
                try { foreach (var i in JArray.Parse(Convert.ToString(json["egg_groups"])))              groups      += $"{i}, "; } catch {}
                try { foreach (var i in JArray.Parse(Convert.ToString(json["family"]["evolutionLine"]))) evolutions  += $"{i}, "; } catch {}

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER)
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle       (Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(json["name"]))); } catch {}
                try { builder.WithDescription (Convert.ToString(json["description"])); } catch {}
                try { builder.WithThumbnail   (Convert.ToString(json["sprites"]["animated"])); } catch {}


                try { builder.AddField("Generation",  Convert.ToString(json["generation"]),      true); } catch {}
                try { builder.AddField("Height",      Convert.ToString(json["height"]),          true); } catch {}
                try { builder.AddField("Weight",      Convert.ToString(json["weight"]),          true); } catch {}
                try { builder.AddField("Base XP",     Convert.ToString(json["base_experience"]), true); } catch {}

                try { builder.AddField("HP",    Convert.ToString(json["stats"]["hp"]),    true); } catch {}
                try { builder.AddField("Speed", Convert.ToString(json["stats"]["speed"]), true); } catch {}

                try { builder.AddField("Attack",  $"{Convert.ToString(json["stats"]["attack"])} ({Convert.ToString(json["stats"]["sp_atk"])} special)",  true); } catch {}
                try { builder.AddField("Defense", $"{Convert.ToString(json["stats"]["defense"])} ({Convert.ToString(json["stats"]["sp_def"])} special)", true); } catch {}

                try { builder.AddField("Distribution", $"{Convert.ToString(json["gender"][0])} | {Convert.ToString(json["gender"][1])}", true); } catch {}


                try { builder.AddField("Type",       types      .Remove(types      .Length - 2, 2), false); } catch {}
                try { builder.AddField("Species",    species    .Remove(species    .Length - 2, 2), false); } catch {}
                try { builder.AddField("Moves",      moves      .Remove(moves      .Length - 2, 2), false); } catch {}
                try { builder.AddField("Egg Groups", groups     .Remove(groups     .Length - 2, 2), false); } catch {}
                try { builder.AddField("Evolutions", evolutions .Remove(evolutions .Length - 2, 2), false); } catch {}

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
            }  
        }

        [SlashCommand("xkcd", "find an XKCD panel by number")]
        public async Task XkcdTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            using (var client = new WebClient()) {
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://xkcd.com/{query}/info.0.json")); } catch { json = null; }

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER)
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle    ($"{Convert.ToString(json["safe_title"])} (#{Convert.ToString(json["num"])})"); } catch {}

                try { builder.WithDescription (Convert.ToString(json["alt"])); } catch {}
                try { builder.WithImageUrl    (Convert.ToString(json["img"])); } catch {}

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false);
            }
        }

        [SlashCommand("lyrics", "find lyrics for a given song")]
        public async Task LyricsTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            using (var client = new WebClient()) {
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://some-random-api.ml/lyrics?title={query}")); } catch { json = null; }

                string lyrics = Convert.ToString(json["lyrics"]);

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER + " | Powered by Genius")
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle ($"{Convert.ToString(json["title"])} - {Convert.ToString(json["author"])}"); } catch {}
                try { builder.WithDescription (lyrics.Length <= 775 ? lyrics : lyrics.Substring(0, 750) + $"...\n\n [see more]({Convert.ToString(json["links"]["genius"])})"); }  catch {}

                try { builder.WithUrl         (Convert.ToString(json["links"]["genius"])); }     catch {}
                try { builder.WithImageUrl    (Convert.ToString(json["thumbnail"]["genius"])); } catch {}

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false);
            }
        }

        [SlashCommand("stocks", "find info on a stock symbol")]
        public async Task StocksTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("yugioh", "find a Yugioh card")]
        public async Task YugiohTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("smash", "get stats for a Super Smash Bros: Ultimate character")]
        public async Task SmashTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("country", "find statistics on a country")]
        public async Task CountryTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }


        [SlashCommand("github", "find lyrics for a given song")]
        public async Task GithubTag(InteractionContext ctx, [Option("type", "Type `user` if you are looking for a user, `org` if you are looking for an organization, or `repo` to find a repository.")] string type, [Option("query", "What is the name of your search?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            using (var client = new WebClient()) {
                client.Headers.Add(HttpRequestHeader.UserAgent, "@CowsauceDev");
                client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Config.KEYS["github"])));

                JToken json;

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER)
                    .WithColor(new DiscordColor(Config.COLORLESS));

                if (type == "user") {
                    builder.WithDescription(query.Split(" ")[1]);
                    json = JObject.Parse(client.DownloadString($"https://api.github.com/users/{query.Split(" ")[1]}"));

                    try { builder.WithTitle  ($"{Convert.ToString(json["login"])} (ID:{Convert.ToString(json["id"])})"); } catch {}
                    try { builder.WithThumbnail (Convert.ToString(json["avatar_url"])); } catch {}

                    try { builder.WithDescription (Convert.ToString(json["bio"])); }      catch {}
                    try { builder.WithUrl         (Convert.ToString(json["html_url"])); } catch {}


                    try { builder.AddField("Repositories", Convert.ToString(json["public_repos"]), true); } catch {}
                    try { builder.AddField("Followers",    Convert.ToString(json["followers"]),    true); } catch {}
                    try { builder.AddField("Following",    Convert.ToString(json["following"]),    true); } catch {}

                    try { builder.AddField("Hireable?", Convert.ToString(json["hireable"]), true); } catch {}

                    try { builder.AddField("Name",     Convert.ToString(json["name"]),             true); } catch {}
                    try { builder.AddField("Company",  Convert.ToString(json["company"]),          true); } catch {}
                    try { builder.AddField("Location", Convert.ToString(json["location"]),         true); } catch {}
                    try { builder.AddField("Email",    Convert.ToString(json["email"]),            true); } catch {}
                    try { builder.AddField("Twitter",  Convert.ToString(json["twitter_username"]), true); } catch {}

                    try { builder.AddField("Joined",   Convert.ToString(json["created_at"]),  false); } catch {}
                    try { builder.AddField("Website",  $"**[{Convert.ToString(json["blog"])}]({Convert.ToString(json["blog"])})**", false); } catch {}
                }

                else if (type == "org") {
                    try { json = JObject.Parse(client.DownloadString($"https://api.github.com/orgs/{query.Split(" ")[1]}")); } catch { json = null; }

                    try { builder.WithTitle    ($"{Convert.ToString(json["login"])} (ID:{Convert.ToString(json["id"])})"); } catch {}
                    try { builder.WithThumbnail   (Convert.ToString(json["avatar_url"])); } catch { builder.WithImageUrl (Convert.ToString(json["avatar_url"])); } finally {}

                    try { builder.WithDescription (Convert.ToString(json["description"])); } catch {}
                    try { builder.WithUrl         (Convert.ToString(json["html_url"])); }    catch {}


                    try { builder.AddField("Repositories", Convert.ToString(json["public_repos"]), true); } catch {}
                    try { builder.AddField("Followers",    Convert.ToString(json["followers"]),    true); } catch {}
                    try { builder.AddField("Following",    Convert.ToString(json["following"]),    true); } catch {}

                    try { builder.AddField("verified?", Convert.ToString(json["verified"]), true); } catch {}

                    try { builder.AddField("Name",     Convert.ToString(json["name"]),             true); } catch {}
                    try { builder.AddField("Location", Convert.ToString(json["location"]),         true); } catch {}
                    try { builder.AddField("Email",    Convert.ToString(json["email"]),            true); } catch {}
                    try { builder.AddField("Twitter",  Convert.ToString(json["twitter_username"]), true); } catch {}

                    try { builder.AddField("Joined",  Convert.ToString(json["created_at"]), false); } catch {}
                    try { builder.AddField("Website", $"**[{Convert.ToString(json["blog"])}]({Convert.ToString(json["blog"])})**", false); } catch {}
                }

                else if (type == "repo") {
                    builder.WithDescription(query.Split(" ")[1]);
                    try { json = JObject.Parse(client.DownloadString($"https://api.github.com/repos/{query.Split(" ")[1]}")); } catch { json = null; }

                    try { builder.WithTitle    ($"{Convert.ToString(json["full_name"])} (ID:{Convert.ToString(json["id"])})"); } catch {}
                    try { builder.WithThumbnail   (Convert.ToString(json["avatar_url"])); } catch { builder.WithImageUrl (Convert.ToString(json["avatar_url"])); } finally {}

                    try { builder.WithDescription (Convert.ToString(json["description"]) + $"\n\n Click **[here]({Convert.ToString(json["clone_url"])})** to clone this repository."); } catch {}
                    try { builder.WithUrl         (Convert.ToString(json["html_url"])); }    catch {}


                    try { builder.AddField("Forks",    Convert.ToString(json["forks"]),            true); } catch {}
                    try { builder.AddField("Stars",    Convert.ToString(json["stargazers_count"]), true); } catch {}
                    try { builder.AddField("Watching", Convert.ToString(json["watchers_count"]),   true); } catch {}
                    try { builder.AddField("Issues",   Convert.ToString(json["open_issues"]),      true); } catch {}

                    try { builder.AddField("Is Forked?", Convert.ToString(json["fork"]), true); } catch {}

                    try { builder.AddField("Website",       $"**[{Convert.ToString(json["homepage"])}]({Convert.ToString(json["homepage"])})**", false); } catch {}
                    try { builder.AddField("Main Language", Convert.ToString(json["language"]), false); } catch {}
                }

                response.AddEmbed(builder);
                await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false);
            }
        }

        [SlashCommand("timenow", "get the current time in a given timezone")]
        public async Task TimenowTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("minecraft", "find a Minecraft user")]
        public async Task MinecraftTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var embed = new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }

        [SlashCommand("eightball", "get a snarky answer to a question")]
        public async Task EightballTag(InteractionContext ctx, [Option("query", "What should I search for?")] string query) {
            var response = new DiscordInteractionResponseBuilder();
            var message = Config.eightball[Config.random.Next(Config.eightball.Length)];
            var embed = new DiscordEmbedBuilder()
                .WithTitle(query)
                .WithDescription(message)
                .WithColor(new DiscordColor(Config.COLORLESS))
                .WithImageUrl("https://thumbs.gfycat.com/QuestionableMajesticBillygoat-size_restricted.gif")
                .WithFooter(Config.FOOTER)
                .Build();

            response.AddEmbed(embed);
            await ctx.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, response).ConfigureAwait(false); 
        }
    }
}