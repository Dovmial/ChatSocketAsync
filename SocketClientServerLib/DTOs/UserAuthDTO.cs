
namespace SocketClientServerLib.DTOs
{
    public record UserAuthDataDTO(string login, string pass, string name = "")
    {
        public override string ToString() 
            => $"{login}{(char)29}{pass}{(char)29}{name}";
        public static UserAuthDataDTO CreateFromString(string messageAuthData)
        {
            string[] parts = messageAuthData.Split((char)29);
            return new(parts[0], parts[1], parts[2]);
        }
    }
}
