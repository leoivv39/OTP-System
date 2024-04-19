namespace OtpServer.Exception
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException(string message) : base(message, ErrorCodes.UserNotFound, ErrorStatusCodes.NotFound)
        {
        }
    }
}
