using System.ComponentModel.DataAnnotations;

namespace ThirdProject_BackEnd.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string user_name { get; set; }
        [Required]
        public string refresh_token { get; set; }
    }
}
