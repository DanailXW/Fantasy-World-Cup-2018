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
    [Authorize]
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
        public IActionResult PlaceGameBets([FromBody]IList<GameUserBetDto> gameBetsDto)
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
    }
}