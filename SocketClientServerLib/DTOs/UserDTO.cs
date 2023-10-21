
namespace SocketClientServerLib.DTOs
{
    public record UserDTO(Guid guid, string name, bool isOnline)
    {
        public override string ToString() => name;

        public static UserDTO CreateInstance_fromString(string str, bool isOnline)
        {
            string[] parts = str.Split((char)29);
            Guid.TryParse(parts[0], out Guid guid);
            return new UserDTO(guid, parts[1], isOnline);
        }
    }
}
