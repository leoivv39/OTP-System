namespace OtpServer.Exception
{
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException() : base(String.Empty, String.Empty, ErrorStatusCodes.Unauthorized)
        {
        }
    }
}
