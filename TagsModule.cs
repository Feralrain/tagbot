// copyright notice

using System;
using System.Net;
using System.Text;
using System.Threading;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json.Linq;

namespace TagBot.Commands {
    public class TagsModule {
        
        //TODO: switch over to CommandsNext
        public static async void RecieveTag(MessageCreateEventArgs e) {
            var tag = e.Message.Content.Split(" ")[0].Substring(1).ToLower();
            var query = e.Message.Content.Substring(tag.Length + 2, e.Message.Content.Length - tag.Length - 2);

            DiscordEmbedBuilder embed;

            if (tag == "gif")                                   embed = GifTag          (query);
            else if (tag == "image"        || tag == "img")     embed = ImageTag        (query);
            else if (tag == "youtube"      || tag == "yt")      embed = YoutubeTag      (query);
            else if (tag == "wikipedia"    || tag == "wiki")    embed = WikipediaTag    (query);
            else if (tag == "wolframalpha" || tag == "wolfram") embed = WolframalphaTag (query);
            else if (tag == "imdb"         || tag == "omdb")    embed = ImdbTag         (query);
            else if (tag == "lmgtfy")                           embed = LmgtfyTag       (query);
            else if (tag == "magic"        || tag == "mtg")     embed = MagicTag        (query);
            else if (tag == "hearthstone"  || tag == "hearth")  embed = HearthstoneTag  (query);
            else if (tag == "league"       || tag == "lol")     embed = LeagueTag       (query);
            else if (tag == "urban"        || tag == "ud")      embed = UrbanTag        (query);
            else if (tag == "define"       || tag == "def")     embed = DefineTag       (query);
            else if (tag == "reverseimage" || tag == "reverse") embed = ReverseimageTag (query);
            else if (tag == "steam")                            embed = SteamTag        (query);
            else if (tag == "anime"        || tag == "mal")     embed = AnimeTag        (query);
            else if (tag == "crypto")                           embed = CryptoTag       (query);
            else if (tag == "pokemon"      || tag == "mon")     embed = PokemonTag      (query);
            else if (tag == "xkcd")                             embed = XkcdTag         (query);
            else if (tag == "lyrics")                           embed = LyricsTag       (query);
            else if (tag == "stocks"       || tag == "stock")   embed = StocksTag       (query);
            else if (tag == "yugioh")                           embed = YugiohTag       (query);
            else if (tag == "supersmash"   || tag == "smash")   embed = SupersmashTag   (query);
            else if (tag == "countrystats" || tag == "country") embed = CountrystatsTag (query);
            else if (tag == "github"       || tag == "gh")      embed = GithubTag       (query);
            else if (tag == "timezone"     || tag == "time")    embed = TimezoneTag     (query);
            else if (tag == "minecraft"    || tag == "mc")      embed = MinecraftTag    (query);
            else if (tag == "eightball"    || tag == "8ball")   embed = EightballTag    (query);

            else return;
            try { await e.Message.RespondAsync(embed).ConfigureAwait(false); } catch {}
        }

        public static DiscordEmbedBuilder GifTag(string query) {
            using (WebClient client = new WebClient()) {
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://g.tenor.com/v1/search?q={query}&key=LIVDSRZULELA&limit=15")); } catch { json = null; }

                return new DiscordEmbedBuilder()
                    .WithTitle(query)
                    .WithImageUrl(Convert.ToString(json["results"][Config.random.Next(15)]["media"][0]["gif"]["url"]))
                    .WithColor(new DiscordColor(Config.COLORLESS))
                    .WithFooter(Config.FOOTER + " | Powered by Tenor");
            }
        }

        public static DiscordEmbedBuilder ImageTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder YoutubeTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder WikipediaTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder WolframalphaTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder ImdbTag(string query) {
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

                return builder;
            }
        }

        public static DiscordEmbedBuilder LmgtfyTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(query)
                .WithUrl($"https://www.lmgtfy.app/?q={query.Replace(" ", "%20")}")
                .WithDescription("This service is not intended to empower arrogant assholes who believe they are better than you. Google is one of the most powerful tools available to us today, and this tool is designed solely to help remind you of when it can be helpful.")
                .WithColor(new DiscordColor(Config.COLORLESS))
                .WithFooter(Config.FOOTER + " | Powered by LMGTFY");
        }

        public static DiscordEmbedBuilder MagicTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder HearthstoneTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder LeagueTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder UrbanTag(string query) {
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
                return builder;
            }
        }

        public static DiscordEmbedBuilder DefineTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder ReverseimageTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder SteamTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder AnimeTag(string query) {
            using (var client = new WebClient()) {
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

                return builder;
            }
        }

        public static DiscordEmbedBuilder CryptoTag(string query) {
            using (var client = new WebClient()) {
                JToken metrics;
                JToken profile;

                try { metrics = JObject.Parse(client.DownloadString($"https://data.messari.io/api/v1/assets/{query}/metrics"))["data"]; } catch { return null; }
                try { profile = JObject.Parse(client.DownloadString($"https://data.messari.io/api/v1/assets/{query}/profile"))["data"]; } catch { return null; }

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

                return builder;
            }
        }

        public static DiscordEmbedBuilder PokemonTag(string query) {
            using (var client = new WebClient()) {
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

                return builder;
            }
        }

        public static DiscordEmbedBuilder XkcdTag(string query) {
            using (var client = new WebClient()) {
                JToken json;
                try { json = JObject.Parse(client.DownloadString($"https://xkcd.com/{query}/info.0.json")); } catch { json = null; }

                var builder = new DiscordEmbedBuilder()
                    .WithFooter(Config.FOOTER)
                    .WithColor(new DiscordColor(Config.COLORLESS));

                try { builder.WithTitle    ($"{Convert.ToString(json["safe_title"])} (#{Convert.ToString(json["num"])})"); } catch {}

                try { builder.WithDescription (Convert.ToString(json["alt"])); } catch {}
                try { builder.WithImageUrl    (Convert.ToString(json["img"])); } catch {}

                return builder;
            }
        }

        public static DiscordEmbedBuilder LyricsTag(string query) {
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
                
                return builder;
            }
        }

        public static DiscordEmbedBuilder StocksTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder YugiohTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder SupersmashTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder CountrystatsTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder GithubTag(string query) {
            var builder = new DiscordEmbedBuilder()
                .WithFooter(Config.FOOTER)
                .WithColor(new DiscordColor(Config.COLORLESS));

            using (var client = new WebClient()) {
                client.Headers.Add(HttpRequestHeader.UserAgent, "@CowsauceDev");
                client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Config.KEYS["github"])));
                JToken json;

                if (query.Split(" ")[0] == "user") {
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

                else if (query.Split(" ")[0] == "org") {
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

                else if (query.Split(" ")[0] == "repo") {
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
                
                return builder;
            }
        }

        public static DiscordEmbedBuilder TimezoneTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder MinecraftTag(string query) {
            return new DiscordEmbedBuilder()
                .WithTitle(Config.UNIMPLEMENTED_ERROR)
                .WithColor(new DiscordColor(Config.RED_COLOR))
                .WithFooter(Config.FOOTER);
        }

        public static DiscordEmbedBuilder EightballTag(string query) {
            var message = Config.eightball[Config.random.Next(Config.eightball.Length)];
            return new DiscordEmbedBuilder()
                .WithTitle(query)
                .WithDescription(message)
                .WithColor(new DiscordColor(Config.COLORLESS))
                .WithImageUrl("https://thumbs.gfycat.com/QuestionableMajesticBillygoat-size_restricted.gif")
                .WithFooter(Config.FOOTER);
        }
    }
}