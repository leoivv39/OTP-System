namespace OtpServer.Mapper.Hash
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string storedHashedPassword);
    }
}
