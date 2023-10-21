
using System.Text.Json;
using System.Xml;

namespace SocketClientWF.Helpers
{
    internal class OptionsManager
    {
        private readonly string _optionsPath;
        public OptionsManager()
        {
            _optionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.json");
        }
        internal (bool, string, Options?) GetOptions()
        {
            if (File.Exists(_optionsPath) == false)
                return (false, "файл опций не найден", null);
            string jsonOptions =  File.ReadAllText(_optionsPath);
            return (true,string.Empty,JsonSerializer.Deserialize<Options>(jsonOptions));
        }
        internal async Task<(bool, string)> SaveOptions(Options options)
        {
            try
            {
                string json = JsonSerializer.Serialize(options, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });
                await File.WriteAllTextAsync(_optionsPath, json);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
