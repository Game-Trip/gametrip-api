using GameTrip.Provider.IProvider;

namespace GameTrip.Provider
{
    public class StartupProvider : IStartupProvider
    {
        public string ping() => "pong !!";
    }
}
