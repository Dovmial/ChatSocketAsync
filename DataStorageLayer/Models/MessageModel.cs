
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
        [Required]
        public Guid SenderGuid { get; set; }
        
        [Required]
        public DateTime TimeStamp { get; set; }

        public MessageModel()
        { }
        public MessageModel(Guid sender, DateTime timeStamp, string data)
        {
            SenderGuid = sender;
            TimeStamp = timeStamp;
            Data = data;
        }

        public override string ToString() => $"{TimeStamp} sender<{SenderGuid}>: {Data}";
    }
}
