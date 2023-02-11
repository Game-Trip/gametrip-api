using GameTrip.Provider.IProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTrip.Provider
{
    public class StartupProvider : IStartupProvider
    {
        public string ping() => "pong";
    }
}
