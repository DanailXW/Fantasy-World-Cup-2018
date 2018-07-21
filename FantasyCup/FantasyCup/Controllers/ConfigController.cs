using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FantasyCup.Services;

namespace FantasyCup.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/config")]
    public class ConfigController : Controller
    {
        private IConfigService _configService;

        public ConfigController(IConfigService service)
        {
            this._configService = service;
        }

        [HttpGet("stagetype")]
        public IActionResult getStageTypes()
        {
            try
            {
                return Ok(_configService.getStageTypes());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}