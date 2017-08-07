using Api.Config.CSServiceLayer.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Api.Config.Controllers
{
    /// <summary>
    /// System Controller
    /// </summary>
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ExceptionController : ControllerBase
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionController(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Exception Handler
        /// </summary>
        [Route("handle")]
        public IActionResult HandleException()
        {
            var method = HttpContext.Request.Method;
            var path = HttpContext.Request.Path.Value;

            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;

            if (error == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            _logger.LogError(0, error, "Exception Occurred");

            var innerError = error.InnerException;

            return StatusCode((int)HttpStatusCode.InternalServerError, innerError != null ?
                new HttpExceptionModel(method, path, innerError.Message) : new HttpExceptionModel(method, path, error.Message));
        }
    }
}
