namespace ThirdProject_BackEnd.Services.PasswordManager
{
    public interface IPasswordManager
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
        public string GenerateJWT(string userName);
    }
}
