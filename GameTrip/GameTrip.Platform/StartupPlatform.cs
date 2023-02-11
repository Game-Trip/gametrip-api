using GameTrip.Platform.IPlatform;
using GameTrip.Provider.IProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Platform
{
    public class StartupPlatform : IStartupPlatform
    {
        private readonly IStartupProvider _startupProvider;

        public StartupPlatform(IStartupProvider startupProvider)
        {
            _startupProvider = startupProvider;
        }

        public string ping() => _startupProvider.ping();
    }
}
