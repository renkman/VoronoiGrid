namespace VoronoiEngine.Utilities
{
    public class Logger
    {
        private const string VoronoiDirectoryName = "VoronoiEngine";
        private const string LogDirectoryName = "logs";
        
        private static Logger _instance;

        public static Logger Instance { get { return _instance ?? (_instance = new Logger()); } }

        public ICollection<string> Messages { get; }

        private Logger()
        {
            Messages = new List<string>();
            Messages.Add($"{DateTime.Now}: Start logging");
        }

        public void Log(string message)
        {
            Messages.Add($"{DateTime.Now}: {message}");
        }

        public void ToFile()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                VoronoiDirectoryName,
                LogDirectoryName);
            Directory.CreateDirectory(path);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"voronoi_{timestamp}.log";
            using (var file = File.CreateText(Path.Combine(path, fileName)))
            {
                foreach (var line in Messages)
                {
                    file.WriteLine(line);
                }
                file.Close();
            }
            Messages.Clear();
        }
    }
}