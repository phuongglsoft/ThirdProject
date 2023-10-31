namespace ThirdProject_BackEnd.Models
{
    public class AuthResponse
    {
        public string jwt { get; set; }
        public UserDTO user { get; set; }
        public string refresh_token { get; set; }
    }
}
