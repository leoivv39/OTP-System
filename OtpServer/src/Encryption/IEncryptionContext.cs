namespace OtpServer.Encryption
{
    public interface IEncryptionContext
    {
        string SecretKey { get; }
    }
}
