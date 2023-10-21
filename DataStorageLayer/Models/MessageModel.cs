
using System.ComponentModel.DataAnnotations;

namespace DataStorageLayer.Models
{
    public class MessageModel
    {
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// Текст сообщения в json
        /// </summary>
        /// 
        [StringLength(512)]
        public string Data { get; set; }
        public Guid SenderGuid { get; set; }
    }
}
