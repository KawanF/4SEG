namespace _4SEG.Services
{
    public interface ILogService
    {
        void Log(string message);
    }

    public class LogService : ILogService
    {
        public void Log(string message)
        {
            var logPath = "logs/security.log";
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            File.AppendAllText(logPath, $"{DateTime.UtcNow}: {message}{Environment.NewLine}");
        }
    }

}
