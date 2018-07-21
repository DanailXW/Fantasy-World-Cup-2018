using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyCup.Model;

namespace FantasyCup.Services
{
    public interface IConfigService
    {
        IEnumerable<StageType> getStageTypes();
    }
    public class ConfigService : IConfigService
    {
        private FantasyCupContext _context;

        public ConfigService(FantasyCupContext context)
        {
            this._context = context;
        }

        public IEnumerable<StageType> getStageTypes()
        {
            return _context.StageType;
        }

    }
}
