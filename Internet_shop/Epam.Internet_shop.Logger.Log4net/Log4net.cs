using Epam.Internet_shop.Logger.Contracts;
using log4net;
using log4net.Config;

namespace Epam.Internet_shop.Logger.Log4net
{
    public class Log4net : ILogger
    {
        private readonly static ILog _log  = LogManager.GetLogger("LOGGER");

        static Log4net()
        {
            XmlConfigurator.Configure();
        }

        public void Info(string message) => _log.Info(message);

        public void Warn(string message) => _log.Warn(message);

        public void Error(string message) => _log.Error(message);
    }
}