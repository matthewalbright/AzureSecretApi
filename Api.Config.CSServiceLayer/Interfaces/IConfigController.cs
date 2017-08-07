using Microsoft.Extensions.Logging;
using Api.Config.CSServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Config.CSServiceLayer.Interfaces
{
    /// <summary>
    /// Config Service Interface
    /// </summary>
    public interface IConfigController<T> where T : class
    {
        /// <summary>
        /// Get Single Config
        /// </summary>
        ReturnConfigModel GetConfig(T configModel, ILogger logger);

    }
}
