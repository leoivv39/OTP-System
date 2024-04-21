namespace OtpServer.Encryption
{
    public interface IEncryptionHandler
    {
        string Encrypt(string message);

        string Decrypt(string message);
    }
}
