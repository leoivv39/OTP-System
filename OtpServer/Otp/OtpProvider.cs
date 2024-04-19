using OtpNet;

namespace OtpServer.Otp
{
    public class OtpProvider : IOtpProvider
    {
        private readonly IOtpContext _context;

        public OtpProvider(IOtpContext context)
        {
            _context = context;
        }

        public string GenerateTotp(out DateTime expirationDate)
        {
            int step = _context.OtpSecondsToExpire;
            int length = _context.OtpLength;

            byte[] key = Base32Encoding.ToBytes(GenerateSecretKey());
            var totp = new Totp(key, step: step, totpSize: length);
            expirationDate = DateTime.Now.AddSeconds(step);
            return totp.ComputeTotp();
        }

        private string GenerateSecretKey()
        {
            var secretKey = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(secretKey);
        }
    }
}
