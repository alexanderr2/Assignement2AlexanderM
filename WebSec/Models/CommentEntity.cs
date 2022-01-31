using System.ComponentModel.DataAnnotations;

namespace WebSec.Models
{
    public class CommentEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
