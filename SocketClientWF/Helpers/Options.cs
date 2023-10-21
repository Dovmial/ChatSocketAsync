
using System.Text.Json.Serialization;

namespace SocketClientWF.Helpers
{
    public class Options
    {
        [JsonPropertyName("clientName")]
        public string ClientName { get; set; } = string.Empty;

        [JsonPropertyName("guid")]
        public string? Guid { get; set; }
        
        [JsonPropertyName("server_ip")]
        public string? ServerIP { get;set; }

        [JsonPropertyName("server_port")]
        public string? ServerPort { get; set; }

    }
}
