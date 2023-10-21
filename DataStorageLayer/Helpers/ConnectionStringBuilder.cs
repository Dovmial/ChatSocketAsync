using Microsoft.Extensions.Configuration;

namespace DataStorageLayer.Helpers
{
    internal static class ConnectionStringBuilder
    {
        private static string? _connectionString;
        private static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string _path = Path.Combine(_baseDirectory, "database");
        internal static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    if (Directory.Exists(_path) == false)
                        Directory.CreateDirectory(_path);
                    _connectionString = GetConnectionString(_baseDirectory);
                }
                return _connectionString;
            }
        }

        private static string GetConnectionString(string baseDirectory)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(baseDirectory)
                .AddJsonFile("AppSettings.json")
                .Build();
            return config.GetConnectionString("DefaultConnection")!;
        }
    }
}