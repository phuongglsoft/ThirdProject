using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThirdProject_BackEnd.Models
{
    public class RefreshToken
    {
        [Key]
        [ForeignKey("User")]
        public string user_name { get; set; }
        public virtual User user { get; set; }
        public string token { get; set; } = string.Empty;
        public DateTime create_time { get; set; } = DateTime.Now;
        public DateTime expires_time { get; set; }
    }
}
