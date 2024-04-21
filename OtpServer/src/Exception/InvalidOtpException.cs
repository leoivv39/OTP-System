namespace OtpServer.Exception
{
    public class InvalidOtpException : ApiException
    {
        public InvalidOtpException(string message) : base(message, ErrorCodes.InvalidOtp, ErrorStatusCodes.BadRequest)
        {
        }
    }
}
