namespace OtpServer.Exception
{
    public class ConsumedOtpException : ApiException
    {
        public ConsumedOtpException(string message) : base(message, ErrorCodes.ConsumedOtp, ErrorStatusCodes.BadRequest)
        {
        }
    }
}
