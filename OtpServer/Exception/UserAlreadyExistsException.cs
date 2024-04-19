namespace OtpServer.Exception
{
    public class UserAlreadyExistsException : ApiException
    {
        public UserAlreadyExistsException(string message) : base(message, ErrorCodes.UserAlreadyExists, ErrorStatusCodes.BadRequest)
        {
        }
    }
}
