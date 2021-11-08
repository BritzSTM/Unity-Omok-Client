using log4net;

namespace WNet.Core
{
    public static class Logger
    {
        static private readonly ILog s_coreLogger = LogManager.GetLogger("WNet.Core");
        static private readonly ILog s_wnetLogger = LogManager.GetLogger("WNet");

        internal static ILog Core => s_coreLogger;
        public static ILog WNet => s_wnetLogger;
    }
}