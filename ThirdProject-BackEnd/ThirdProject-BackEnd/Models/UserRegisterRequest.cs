using System.ComponentModel.DataAnnotations;

namespace ThirdProject_BackEnd.Models
{
    public class UserRegisterRequest
    {
        [Required]
        public string user_name { get; set; }
        [Required]
        public string password { get; set; }
        public DateTime birth_day { get; set; }
    }
}
