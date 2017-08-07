using Api.Config.SRServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Config.SRServiceLayer.Models;

namespace Api.Config.Repositories
{
    public class GenericRepository : IRepository
    {
        public Task<ReturnConfigModel> GetConfig(ConfigModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnConfigModel> SetConfig(ConfigModel model)
        {
            throw new NotImplementedException();
        }
    }
}
