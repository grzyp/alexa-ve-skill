using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Services
{
    public interface ISearchService
    {
        Task<List<Club>> ClubSearch(string searchPhrase, bool isMen);
        Task<List<League>> LeagueSearch(string searchPhrase, bool isMen);
    }
}
