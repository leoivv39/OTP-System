namespace OtpServer.Otp
{
    public interface IOtpContext
    {
        public int OtpSecondsToExpire { get; }

        public int OtpLength { get; }
    }
}
