namespace Epam.Internet_shop.Logger.Contracts
{
    public interface ILogger
    {
        void Info(string message);

        void Warn(string message);

        void Error(string message);
    }
}
