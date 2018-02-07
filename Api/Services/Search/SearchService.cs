using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Api.Models;

namespace Api.Services
{
    public class SearchService : ISearchService
    {
        private readonly IClient _webClient;
        private IOptions<LeagueConfig> _leagueConfig;

        public SearchService(IClient webClient, IOptions<LeagueConfig> leagueConfig)
        {
            _webClient = webClient;
            _leagueConfig = leagueConfig;
        }

        public async Task<List<Club>> ClubSearch(string searchPhrase, bool isMen)
        {
            Object lockAdd = new Object();
            List<Club> result = new List<Club>();
            List<string> teams = isMen ? _leagueConfig.Value.Men : _leagueConfig.Value.Women;
            Task<Stream>[] tasks = new Task<Stream>[teams.Count];

            for (int i = 0; i < teams.Count; i++)
            {
                tasks[i] = _webClient.GetPage(_leagueConfig.Value.SearchURL + teams[i]);
            }
            Task.WaitAll(tasks);

            Parallel.ForEach(tasks, async (task) =>
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(task.Result);
                var league = doc.DocumentNode.SelectSingleNode("//div[@class='leaguetablehold']//text()[normalize-space()]").InnerText;
                var rows = doc.DocumentNode.SelectNodes("//table//tr");

                //First row contains labells for the table so we skip it
                for (int i = 1; i < rows.Count; i++)
                {
                    var tableCells = rows[i].SelectNodes(".//td");
                    if (tableCells[1].InnerHtml.ToLower().Contains(searchPhrase.ToLower()))
                    {
                        var club = new Club();
                        club.LeaguePosition = tableCells[0].InnerText;
                        club.Name = tableCells[1].InnerText;
                        club.GamesPlayed = tableCells[2].InnerText;
                        club.GamesWon = tableCells[3].InnerText;
                        club.GamesLost = tableCells[4].InnerText;
                        club.LeaguePoints = tableCells[11].InnerText;
                        club.League = league;

                        lock (lockAdd)
                        {
                            result.Add(club);
                        }
                    }
                }
            }
            );

            if(result.Count>1)
            {
                return result.OrderBy(c => c.League).ToList();
            }

            return result;
        }

        public async Task<List<League>> LeagueSearch(string searchPhrase, bool isMen)
        {
            Object lockAdd = new Object();
            List<League> result = new List<League>();
            Task<Stream>[] tasks = new Task<Stream>[_leagueConfig.Value.Men.Count];
            List<string> teams = isMen ? _leagueConfig.Value.Men : _leagueConfig.Value.Women;

            for (int i = 0; i < _leagueConfig.Value.Men.Count; i++)
            {
                tasks[i] = _webClient.GetPage(_leagueConfig.Value.SearchURL + teams[i]);
            }
            Task.WaitAll(tasks);

            Parallel.ForEach(tasks, async (task) =>
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(task.Result);
                var leagueName = doc.DocumentNode.SelectSingleNode("//div[@class='leaguetablehold']//text()[normalize-space()]").InnerText;
                if (leagueName.ToLower().Contains(searchPhrase))
                {
                    var newLeague = new League(leagueName);

                    var rows = doc.DocumentNode.SelectNodes("//table//tr");

                    //First row contains labells for the table so we skip it
                    for (int i = 1; i < rows.Count; i++)
                    {
                        var tableCells = rows[i].SelectNodes(".//td");

                        if (Convert.ToInt32(tableCells[0].InnerText) < 4)
                        {
                            var club = new Top3Club();
                            club.LeaguePosition = tableCells[0].InnerText;
                            club.Name = tableCells[1].InnerText;
                            club.GamesPlayed = tableCells[2].InnerText;
                            club.GamesWon = tableCells[3].InnerText;
                            club.GamesLost = tableCells[4].InnerText;
                            club.LeaguePoints = tableCells[11].InnerText;

                            newLeague.Top3Clubs.Add(club);

                        }
                        else
                        {
                            var club = new ClubSnippet();
                            club.Name = tableCells[1].InnerText;
                            club.LeaguePosition = tableCells[0].InnerText;
                            club.LeaguePoints = tableCells[11].InnerText;
                            newLeague.OtherClubs.Add(club);
                        }
                    }

                    lock (lockAdd)
                    {
                        result.Add(newLeague);
                    }
                }
            }
            );

            if (result.Count > 1)
            {
                return result.OrderBy(c => c.Name).ToList();
            }

            return result;
        }

    }
}
