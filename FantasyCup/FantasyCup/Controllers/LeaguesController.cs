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
    public class LeaguesController : Controller
    {
        private ILeagueService _leagueService;
        private IMapper _mapper;

        public LeaguesController(ILeagueService leagueService, IMapper mapper)
        {
            _leagueService = leagueService;
            _mapper = mapper;
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            var leagues = _leagueService.GetAll();
            var leagueDtos = _mapper.Map<IList<LeagueDto>>(leagues);

            return Ok(leagueDtos);
        }

        [HttpGet("entered")]
        public IActionResult GetEntered()
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            var leagues = _leagueService.GetLeaguesByUserId(userId);
            var leagueDtos = _mapper.Map<IList<LeagueDto>>(leagues);

            return Ok(leagueDtos);
        }

        [HttpGet("{leagueId:int}")]
        public IActionResult Get([FromRoute]int leagueId)
        {
            try
            {
                var league = _leagueService.GetLeagueById(leagueId, Convert.ToInt32(User.Identity.Name));
                var leagueDto = _mapper.Map<LeagueDto>(league);

                return Ok(leagueDto);
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("find")]
        public IActionResult Find([FromBody]LeagueDto leagueDto)
        {
            try
            {
                var league = _leagueService.Find(leagueDto.Name);
                return Ok(new { Id = league.Id });
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody]LeagueDto leagueDto)
        {
            var league = _mapper.Map<League>(leagueDto);
            try
            {
                _leagueService.Create(league);
                _leagueService.Join(league.Id, Convert.ToInt32(User.Identity.Name), league.Code, LeagueJoinMode.Create);
                var retLeagueDto = _mapper.Map<LeagueDto>(league);
                return Ok(retLeagueDto);
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]LeagueDto leagueDto)
        {
            var league = _mapper.Map<League>(leagueDto);
            try
            {
                _leagueService.Update(league, Convert.ToInt32(User.Identity.Name));
                return Ok();
            }
            catch (FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("join")]
        public IActionResult Join([FromBody]LeagueDto leagueDto)
        {
            var league = _mapper.Map<League>(leagueDto);
            try
            {
                _leagueService.Join(league.Id, Convert.ToInt32(User.Identity.Name), league.Code, LeagueJoinMode.Join);
                return Ok();
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("leave/{leagueId:int}")]
        public IActionResult Leave([FromRoute]int leagueId)
        {
            try
            {
                _leagueService.Leave(leagueId, Convert.ToInt32(User.Identity.Name));
                return Ok();
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{leagueId:int}/members")]
        public IActionResult GetMembers([FromRoute]int leagueId)
        {
            try
            {
                var members = _leagueService.GetAllMembers(leagueId, Convert.ToInt32(User.Identity.Name));
                var memberDtos = _mapper.Map<IList<LeagueUserDto>>(members);

                return Ok(memberDtos);
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{leagueId:int}/members/update")]
        public IActionResult UpdateMembers([FromRoute]int leagueId, [FromBody]IList<LeagueUserDto> memberDtos)
        {
            try
            {
                var members = _mapper.Map<IList<LeagueUser>>(memberDtos);
                _leagueService.UpdateMembers(leagueId, Convert.ToInt32(User.Identity.Name), members.ToArray());

                return Ok();
            }
            catch(FantasyException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
