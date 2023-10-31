using System.ComponentModel.DataAnnotations;

namespace ThirdProject_BackEnd.Models
{
    public class User
    {
        [Key]
        public string user_name { get; set; }
        public byte[] password_hash { get; set; }
        public byte[] password_salt { get; set; }
        public DateTime birth_day { get; set; }
        public DateTime create_time { get; set; }
        public DateTime? last_login { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
