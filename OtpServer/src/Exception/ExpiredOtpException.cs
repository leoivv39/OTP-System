namespace OtpServer.Exception
{
    public class ExpiredOtpException : ApiException
    {
        public ExpiredOtpException(string message) : base(message, ErrorCodes.ExpiredOtp, ErrorStatusCodes.BadRequest)
        {
        }
    }
}
