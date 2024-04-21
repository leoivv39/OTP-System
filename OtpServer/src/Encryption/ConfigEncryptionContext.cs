namespace OtpServer.Encryption
{
    public class ConfigEncryptionContext(IConfiguration config) : IEncryptionContext
    {
        public string SecretKey => config["Encryption:Base64Key"]!;
    }
}
