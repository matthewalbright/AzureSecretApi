using Api.Config.CSServiceLayer.Interfaces;
using Api.Config.CSServiceLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Api.Config.Controllers
{
    /// <summary>
    /// Config Controller
    /// </summary>
    [Route("v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfigService _configService;
        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigController(ILogger logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
        }

        /// <summary>
        /// Get Single Config
        /// </summary>
        [HttpPost("GetConfig")]
        [ProducesResponseType(typeof(ReturnConfigModel), 200)]
        [ProducesResponseType(typeof(ReturnConfigModel), 400)]
        public IActionResult GetConfig([FromBody] ConfigModel request)
        {
            try
            {
                _logger.LogDebug("{ControllerClass}.{ControllerMethod}() Called.", nameof(ConfigController),
                    nameof(GetConfig));
                return Ok(_configService.GetConfig(request));
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ReturnConfigModel()
                {
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Sets a Single Config (Create or Edit)
        /// </summary>
        [HttpPost("SetConfig")]
        [ProducesResponseType(typeof(ReturnConfigModel), 200)]
        [ProducesResponseType(typeof(ReturnConfigModel), 400)]
        public IActionResult SetConfig([FromBody] ConfigModel request)
        {
            try
            {
                _logger.LogDebug("{ControllerClass}.{ControllerMethod}() Called.", nameof(ConfigController),
                    nameof(SetConfig));
                return Ok(_configService.SetConfig(request));
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ReturnConfigModel()
                {
                    Error = ex.Message
                });
            }
        }

    }
}
