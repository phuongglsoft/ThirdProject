namespace ThirdProject_BackEnd.Models
{
    public class UserDTO
    {
        public string user_name { get; set; }
        public DateTime birth_day { get; set; }
        public DateTime create_time { get; set; }
        public DateTime? last_login { get; set; }
    }
}
