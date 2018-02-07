using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [Route("club")]
        [HttpGet]
        public async Task<IActionResult> SearchClub(string searchPhrase, bool isMen)
        {
            var result = await _searchService.ClubSearch(searchPhrase, isMen);
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Route("league")]
        [HttpGet]
        public async Task<IActionResult> SearchLeague(string searchPhrase, bool isMen)
        {
            var result = await _searchService.LeagueSearch(searchPhrase, isMen);
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}