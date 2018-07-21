using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FantasyCup.Dtos;
using FantasyCup.Services;
using AutoMapper;

namespace FantasyCup.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [AllowAnonymous]
    public class CompetitionController : Controller
    {
        private ICompetitionService _competitionService;
        private IMapper _mapper;

        public CompetitionController(ICompetitionService competitionService, IMapper mapper)
        {
            _competitionService = competitionService;
            _mapper = mapper;
        }

        [HttpGet("{cid:int}/teams")]
        public IActionResult GetTeams([FromRoute]int cid)
        {
            var teams = _competitionService.GetTeams(cid);
            var teamsDto = _mapper.Map<IList<TeamDto>>(teams);

            return Ok(teamsDto);
        }

        [HttpGet("{cid:int}/team/{tid:int}/players")]
        public IActionResult GetPlayersByTeam([FromRoute]int cid, [FromRoute]int tid)
        {
            var players = _competitionService.GetPlayersByTeam(cid, tid);
            var playersDto = _mapper.Map<IList<PlayerDto>>(players);

            return Ok(playersDto);
        }

        [HttpGet("{cid:int}/players")]
        public IActionResult GetPlayers([FromRoute]int cid)
        {
            var players = _competitionService.GetPlayers(cid);
            var playersDto = _mapper.Map<IList<PlayerDto>>(players);

            return Ok(playersDto);
        }

        [HttpGet("{cid:int}/games")]
        public IActionResult GetGames([FromRoute]int cid)
        {
            var games = _competitionService.GetGames(cid);
            var gamesDto = _mapper.Map<IList<GameDto>>(games);

            return Ok(gamesDto);
        }

        [HttpGet("{cid:int}/playerstats")]
        public IActionResult GetPlayerStats([FromRoute]int cid)
        {
            var playerStats = _competitionService.GetPlayerStats(cid);
            var playerStatsDto = _mapper.Map<IList<PlayerStatsDto>>(playerStats);

            return Ok(playerStatsDto);
        }

        [HttpGet("{cid:int}/groupstandings")]
        public IActionResult GetGroupStandings([FromRoute]int cid)
        {
            try
            {
                var standings = _competitionService.GetGroupStandings(cid);
                var standingsDto = _mapper.Map<IList<GroupStandingsDto>>(standings);

                return Ok(standingsDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}