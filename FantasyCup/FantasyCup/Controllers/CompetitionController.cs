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
    }
}