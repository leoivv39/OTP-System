namespace OtpServer.Otp
{
    public interface IOtpProvider
    {
        string GenerateTotp(out DateTime expirationDate);
    }
}
