using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FantasyCup.Dtos;
using FantasyCup.Helpers;
using FantasyCup.Model;
using FantasyCup.Services;
using AutoMapper;

namespace FantasyCup.Controllers
{
    [Authorize(Roles = "Access")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BetsController : Controller
    {
        private IBetService _betService;
        private IMapper _mapper;

        public BetsController(IBetService betService, IMapper mapper)
        {
            _betService = betService;
            _mapper = mapper;
        }

        [HttpGet("games")]
        public IActionResult GetGamesToBet()
        {
            var games = _betService.GetGamesToBet(Convert.ToInt32(User.Identity.Name));
            var gamesDto = _mapper.Map<IList<GameUserBetDto>>(games);

            return Ok(gamesDto);
        }

        [HttpPost("games/place")]
        public IActionResult PlaceGameBets([FromBody]IList<UserBetDto> gameBetsDto)
        {
            try
            {
                var gameBets = _mapper.Map<IList<GameUserBet>>(gameBetsDto);
                _betService.PlaceBetGameResult(Convert.ToInt32(User.Identity.Name), gameBets.ToArray());

                return Ok();
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{cid:int}/competitionbets")]
        public IActionResult GetCompetitionBets([FromRoute]int cid)
        {
            return Ok(_betService.GetCompetitionBets(Convert.ToInt32(User.Identity.Name), cid));
        }

        [HttpPost("{cid:int}/place")]
        public IActionResult PlaceCompetitionBets([FromRoute]int cid, [FromBody]IList<CompetitionUserBetDto> competitionBetsDto)
        {
            try
            {
                var competitionBets = _mapper.Map<IList<CompetitionUserBet>>(competitionBetsDto);
                _betService.PlaceBetCompetition(Convert.ToInt32(User.Identity.Name), cid, competitionBets.ToArray());

                return Ok();
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("game/{gameId:int}/others")]
        public IActionResult GetOthersBets([FromRoute]int gameId)
        {
            try
            {
                var bets = _betService.GetOthersBets(Convert.ToInt32(User.Identity.Name), gameId);
                var betsDto = _mapper.Map<IList<GameUserBetDto>>(bets);

                return Ok(betsDto);
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}