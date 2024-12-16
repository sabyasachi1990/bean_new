using Logger;
using Serilog;

namespace AppsWorld.Bean.WebAPI
{
    public class SeriLogConfig
    {
        public static void ConfigureLogs()
        {
            Log.Logger = LogManager.GetRollingFileSink();
        }
    }
}