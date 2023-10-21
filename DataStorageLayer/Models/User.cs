
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorageLayer.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Guid {get;set;}
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = null!;
        [Required]
        public string HashedPassword { get; set; } = null!;
        public string? Name { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
