

using System.ComponentModel.DataAnnotations;

namespace DataStorageLayer.Models
{
    public class UserMessageReceivers
    {
        [Key]
        public int Id { get; set; }
        public Guid UserReceiveGuid { get; set; }
        public long MessageModelId { get; set; }
    }
}
