﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FantasyCup.Dtos;
using FantasyCup.Helpers;
using FantasyCup.Model;
using FantasyCup.Services;
using AutoMapper;

namespace FantasyCup.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private ILeagueService _leagueService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService userService, ILeagueService leagueService, IMapper mapper, IOptions<AppSettings> options)
        {
            _userService = userService;
            _leagueService = leagueService;
            _mapper = mapper;
            _appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.EmailAddress, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenString = this.CreateAccessToken(user.Id);
            var refresh_tokenString = this.CreateRefreshToken(user.Id);

            _userService.SaveRefreshToken(user.Id, refresh_tokenString);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                EmailAddress = user.EmailAddress,
                Token = tokenString,
                RefreshToken = refresh_tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            // save 
            _userService.Create(user, userDto.Password, "");

            try
            {
                var tokenString = this.CreateAccessToken(user.Id);
                var refresh_tokenString = this.CreateRefreshToken(user.Id);

                _userService.SaveRefreshToken(user.Id, refresh_tokenString);

                //Join Overall league
                League dfLeague = _leagueService.GetDefaultLeague();
                _leagueService.Join(dfLeague.Id, user.Id, dfLeague.Code, LeagueJoinMode.Auto);

                return Ok(new
                {
                    Id = user.Id,
                    Username = user.UserName,
                    EmailAddress = user.EmailAddress,
                    Token = tokenString,
                    RefreshToken = refresh_tokenString
                });
            }
            catch (FantasyException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("refresh")]
        [Authorize(Roles = "Auth")]
        public IActionResult RefreshAuth()
        {
            int userId = Convert.ToInt32(User.Identity.Name);
            if (!_userService.VerifyRefreshToken(userId, Request.Headers["Authorization"]))
                return Unauthorized();

            try
            {
                var tokenString = this.CreateAccessToken(userId);
                var refresh_tokenString = this.CreateRefreshToken(userId);

                _userService.SaveRefreshToken(userId, refresh_tokenString);

                return Ok(new
                {
                    Token = tokenString,
                    RefreshToken = refresh_tokenString
                });
            }
            catch (FantasyException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }


        }

        private string CreateAccessToken(int userId)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            //access token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                    new Claim(ClaimTypes.Role, "Access")
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private string CreateRefreshToken(int userId)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            //refresh token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString()),
                    new Claim(ClaimTypes.Role, "Auth")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
